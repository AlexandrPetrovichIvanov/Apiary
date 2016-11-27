namespace Apiary.BeeWorkflowApiary
{
    /// <summary>
    /// Улей, основанный на непрерывной работе пчёл и обработке
    /// их запросов и действий, без прямого вызова методов пчёл.
    /// </summary>
    public class BeeWorkflowBeehive : IBeehive
    {
        /// <summary>
        /// Фабрика пчёл.
        /// </summary>
        private readonly IBeeFactory beeFactory;

        /// <summary>
        /// Улей в данный момент работает.
        /// </summary>
        private bool isWorking;

        /// <summary>
        /// Потокобезопасное состояние улья.
        /// </summary>
        private BeehiveStateThreadSafe state;

        /// <summary>
        /// Очередь пчёл на посту охраны.
        /// </summary>
        private GuardPostQueue guardPostQueue;

        /// <summary>
        /// Список пчёл, допущенных в улей.
        /// </summary>
        private GuardPostAcceptedList acceptedList;

        /// <summary>
        /// Все пчёлы улья.
        /// </summary>
        private ConcurrentQueue<IBee> allBees;

        /// <summary>
        /// Создать улей.
        /// </summary>
        public BeeWorkflowBeehive()
        {
            this.factory = ServiceLocator.Instance.GetService<IBeeFactory>();
        }

        /// <summary>
        /// Получить текущее состояние улья.
        /// </summary>
        /// <returns>Состояние улья.</returns>
        public IBeehiveState State => this.state;

        /// <summary>
        /// "Запустить" улей.
        /// </summary>
        /// <param name="state">Исходное состояние улья.</param>
        public void Start(IBeehiveState state)
        {
            if (this.isWorking)
            {
                throw new InvalidOperationException(
                    "Улей нельзя запустить, если он уже работает.");
            }

            this.isWorking = true;

            this.state = new BeehiveStateThreadSafe(state);
            this.guardPostQueue = new GuardPostQueue();
            this.acceptedList = new GuardPostAcceptedList();

            this.StartWork();
        }

        /// <summary>
        /// Завершить работу улья.
        /// </summary>
        /// <returns>Состояние улья на момент завершения работы.</returns>
        public IBeehiveState Stop()
        {
            if (!this.isWorking)
            {
                throw new InvalidOperationException(
                    "Нельзя остановить не запущенный улей.");
            }

            IBee nextBee;
            while(this.allBees.TryDequeue(out nextBee))
            {
                nextBee.StopWork();
                this.UnregisterBee(nextBee);
            }

            this.isWorking = false;

            return this.state;
        }

        /// <summary>
        /// Собрать накопленный в улье мёд.
        /// </summary>
        /// <returns>Количество собранного мёда.</returns>
        public long CollectHoney()
        {
            return this.state.ResetHoneyCount();
        }

        /// <summary>
        /// Начать работу улья.
        /// </summary>
        private void StartWork()
        {
            List<IBee> initialBees = new List<IBee>();

            initialBees.AddRange(this.GetInitialQueens());
            ihitialBees.AddRange(this.GetInitialGuards());
            initialBees.AddRange(this.GetInitialWorkers());         

            initialBees.ForEach(bee =>
            {
                this.RegisterBee(bee);
                bee.StartWork();
            });
        }

        /// <summary>
        /// Получить изначально заданных пчёл-охранников.
        /// </summary>
        private List<IBee> GetInitialGuards()
        {
            List<IBee> result = new List<IBee>();

            for (int i = 0; i < this.state.GuardsCount; i++)
            {
                result.Add(this.factory.CreateGuardBee());
            }

            return result;
        }

        /// <summary>
        /// Получить изначально заданных пчёл-маток.
        /// </summary>
        private List<IBee> GetInitialQueens()
        {
            List<IBee> result = new List<IBee>();

            for (int i = 0; i < this.state.QueensCount; i++)
            {
                result.Add(this.factory.CreateQueenBee());
            }

            return result;
        }

        /// <summary>
        /// Получить изначально заданных рабочих пчёл (внутри и снаружи улья).
        /// </summary>
        private List<IBee> GetInitialWorkers()
        {
            List<IBee> result = new List<IBee>();

            int workersInside = this.state.BeesInsideCount
                - this.state.QueensCount - this.state.GuardsCount;

            for (int i = 0; i < workersInside; i++)
            {
                result.Add(this.factory.CreateWorkerBee(BeeWorkingState.OnTheRest));
            }

            int workersOutside = this.state.WorkerBeesCount - workersInside;

            for (int i = 0; i < workersOutside; i++)
            {
                result.Add(this.factory.CreateWorkerBee(BeeWorkingState.OnTheWork));
            }

            return result;
        }

        /// <summary>
        /// Зарегистрировать пчелу в улье, т.е. установить её связь с ульем.
        /// </summary>
        /// <param name="bee">Пчела.</param>
        private void RegisterBee(IBee bee)
        {            
            this.allBees.Enqueue(bee);
            bee.BeehiveNumber = this.BeehiveNumber;

            bee.ActionPerformed += this.BeeOnActionPerformed;
            bee.RequestForBeehiveData += this.BeeOnRequestBeehiveData;
        }

        /// <summary>
        /// Разрегистрировать пчелу, т.е. прервать её связь с ульем.
        /// </summary>
        /// <param name="bee">Пчела.</param>
        private void UnregisterBee(IBee bee)
        {
            bee.ActionPerformed -= this.BeeOnActionPerformed;
            bee.RequestForBeehiveData -= this.BeeOnRequestBeehiveData;
        }

        /// <summary>
        /// Обновить состояние согласно типу только
        /// что рожденной пчелы.
        /// </summary>
        /// <param name="newBeeType">Тип рожденной пчелы.</param>
        private void ChangeStateAccordingToNewBee(BeeType newBeeType)
        {
            switch (newBeeType)
            {
                case BeeType.Worker:
                    this.state.IncrementWorkerBeesCount();

                case BeeType.Guard:
                    this.state.IncrementGuardsCount();

                case BeeType.Queen:
                    this.state.IncrementQueensCount();

                default:
                    throw new ArgumentOutOfRangeException(
                        $"Неизвестный тип пчелы - {newBeeType}",
                        nameof(newBeeType));
            }

            this.state.IncrementBeesInsideCount();
        }

        /// <summary>
        /// Обработка действий зарегистрированных пчёл.
        /// </summary>
        /// <param name="sender">Пчела-исполнитель действия.</param>
        /// <param name="args">Аргументы действия пчелы.</param>
        private void BeeOnActionPerformed(
            object sender,
            BeeActionEventArgs args)
        {
            switch (args.ActionType)
            {
                case BeeActionType.LeftBeehiveToHarvestHoney:
                    this.state.DecrementBeesInsideCount();                    

                case BeeActionType.EnterGuardPost:
                    this.guardPostQueue.Enqueue(args.SenderBee);

                case BeeActionType.EnterBeehiveWithHoney:
                    this.state.IncrementHoneyCount();
                    this.state.IncrementBeesInsideCount(); 
                    this.acceptedList.ResetBeeAcceptedState(args.SenderBee);             

                case BeeActionType.AcceptBeeToEnter:
                    this.acceptedList.AcceptBeeToEnter(args.RelatedBee);

                case BeeActionType.ProduceBee:
                    this.RegisterBee(args.RelatedBee);
                    args.RelatedBee.StartWork();
                    this.ChangeStateAccordingToNewBee(args.RelatedBee.Type);

                default:
                    throw new ArgumentOutOfRangeException(
                        $"Неизвестное действие пчелы - {args.ActionType}",
                        nameof(args));
            }
        }

        /// <summary>
        /// Обработка запросов зарегистрированных пчёл.
        /// </summary>
        /// <param name="sender">Пчела-отправитель запроса.</param>
        /// <param name="args">Аргументы запроса пчелы к улью.</param>
        private void BeeOnRequestBeehiveData(
            object sender,
            BeeRequestEventArgs args)
        {
            switch (args.RequestType)
            {
                case BeeRequestType.RequestToEnterBeehive:
                    args.Succeed = this.acceptedList.IsBeeAcceptedToEnter(args.SenderBee);
                    args.Response = true;

                case BeeRequestType.RequestGuardPostQueue:
                    args.Succeed = args.SenderBee.Type == BeeType.Guard
                        && args.SenderBee.BeehiveNumber == this.state.BeehiveNumber;
                    
                    if (args.Succeed)
                    {
                        args.Response = this.guardPostQueue;
                    }

                default:
                    throw new ArgumentOutOfRangeException(
                        $"Неизвестный тип запроса пчелы - {args.RequestType}",
                        nameof(args));
            }
        }
    }
}