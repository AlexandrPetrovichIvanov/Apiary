namespace Apiary.BeeWorkflowApiary.Bees
{
    /// <summary>
    /// Пчела (общая часть реализации всех видов пчёл).
    /// </summary>
    public abstract class BeeBase : IBee
    {
        /// <summary>
        /// Получить тип пчелы.
        /// </summary>
        /// <returns>Тип пчелы.</returns>
        public abstract BeeType Type;

        /// <summary>
        /// Имитатор выполнения длительных операций.
        /// </summary>
        private readonly ILongOperationSimulator longOperationSimulator;

        /// <summary>
        /// Текущее выполняемое действие.
        /// </summary>
        private Task currentAction;

        /// <summary>
        /// Общая часть создания пчелы.
        /// </summary>
        internal BeeBase()
        {
            this.longOperationSimulator = ServiceLocator.Instance.GetService<ILongOperationSimulator>();
        }

        /// <summary>
        /// Событие выполнения пчелой какого-либо действия.
        /// </summary>
        public event EventHandler<BeeActionEventArgs> ActionPerformed
        {
            remove { this.ThrowIfStillWorking(); }
        }

        /// <summary>
        /// Делегат запроса пчелой каких-либо данных из улья.
        /// </summary>
        public event EventHandler<BeeRequestEventArgs> RequestForBeehiveData
        {
            remove { this.ThrowIfStillWorking(); }
        }

        /// <summary>
        /// Начать работу.
        /// </summary>
        public virtual void StartWork()
        {
            this.CheckLinkWithBeehive();
            Task.Factory.StartNew(this.GetStartAction());
            this.isWorking = true;
        }

        /// <summary>
        /// Завершить работу.
        /// </summary>
        public void StopWork()
        {            
            this.isWorking = false;
            this.currentAction.GetAwaiter().Await();
        }

        /// <summary>
        /// Метод получения действия, с которого нужно начинать работу.
        /// </summary>
        /// <returns>Действие, с которого нужно начинать работу.</returns>
        protected abstract Action GetStartAction();

        /// <summary>
        /// Выполнить действие, подождать, и выполнить следующее.
        /// </summary>
        /// <param name="action">Действие для выполнения.</param>
        /// <param name="timeBeforeNextAction">Задержка перед последующим действием.</param>
        /// <param name="nextAction">Последующее действие.</param>
        protected void PerformOperation(
            Action action,
            TimeSpan timeBeforeNextAction,
            Action nextAction)
        {
            this.DoAsCurrentAction(() =>
            {
                action();
                this.longOperationSimulator.SimulateAsync(time, nextAction);
            });
        }

        /// <summary>
        /// Выполнить действие, подождать, и выполнить следующее.
        /// </summary>
        /// <typeparam name="T">Параметр для вычисления следующего действия.</typeparam>
        /// <param name="action">Действие для выполнения (результат - параметр 
        /// для выбора следующего действия).</param>
        /// <param name="timeBeforeNextAction">Задержка перед следующим действием.</param>
        /// <param name="selectNextAction">Функция выбора следующего действия.</param>
        protected void PerformOperation<T>(
            Func<T> action,
            TimeSpan timeBeforeNextAction,
            Func<Action, T> selectNextAction)
        {
            this.DoAsCurrentAction(() =>
            {
                T actionResult = action();
                this.longOperationSimulator.SimulateAsync(
                    time, 
                    selectNextAction(actionResult));
            });
        }

        /// <summary>
        /// Выполнить как текущее действие (при остановке работы текущее 
        /// действие будет выполнено до конца).
        /// </summary>
        /// <param name="action">Действие.</param>
        private void DoAsCurrentAction(Action action)
        {
            this.currentAction = Task.Factory.StartNew(() =>
            {
                if (!this.isWorking)
                {
                    return;
                }

                action();
            });
        }

        /// <summary>
        /// Сгенерировать исключение, если пчела в данный момент работает.
        /// </summary>
        private void ThrowIfStillWorking()
        {
            if (this.isWorking)
            {
                throw new InvalidOperationException(
                    "Нельзя обрывать связь с ульем, пока работа пчелы не остановлена.");
            }
        }
    }
}