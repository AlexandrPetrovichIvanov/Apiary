namespace Apiary.BeeWorkflowApiary.Bees
{
    /// <summary>
    /// Пчела-охранник.
    /// </summary>
    public class GuardBee : BeeBase
    {
        /// <summary>
        /// Баланс пчелы-охранника.
        /// </summary>
        private readonly IGuardBeeBalance balance;

        /// <summary>
        /// Очередь пчёл на посту охраны.
        /// </summary>
        private GuardPostQueue guardPostQueue;

        /// <summary>
        /// Получить тип пчелы.
        /// </summary>
        /// <returns>Тип пчелы.</returns>
        public BeeType Type => BeeType.Guard;

        /// <summary>
        /// Создать пчелу-охранника.
        /// </summary>
        public GuardBee()
            : base()
        {
            this.balance = ServiceLocator.Instance.GetService<IGuardBeeBalance>();
        }

        /// <summary>
        /// Метод получения действия, с которого нужно начинать работу.
        /// </summary>
        /// <returns>Действие, с которого нужно начинать работу.</returns>
        private override Action GetStartAction()
        {
            return () =>
            {
                this.RequestGuardPostQueue();
                this.CheckOneBee();
            }
        }

        /// <summary>
        /// Запросить у улья и получить очередь поста охраны.
        /// </summary>
        private void RequestGuardPostQueue()
        {
            var request = new BeeRequestEventArgs<GuardPostQueue>
            {
                RequestType = BeeRequestType.RequestGuardPostQueue
            };

            this.RequestForBeehiveData(this, request)

            if (!request.Succeed
                || request.TypedResponse == null)
            {
                throw new InvalidOperationException(
                    "Не удалось получить очередь поста охраны пчёл.");
            }

            this.guardPostQueue = request.TypedResponse;
        }

        /// <summary>
        /// Проверить одну пчелу.
        /// </summary>
        private void CheckOneBee()
        {
            this.PerformOperation(
                () =>
                {
                    IBee nextBee = this.guardPostQueue.Dequeue();

                    if (nextBee == null 
                        || nextBee.BeehiveNumber != this.BeehiveNumber)
                    {
                        return;
                    }

                    this.ActionPerformed.Invoke(
                        this,
                        new BeeActionEventArgs
                        {
                            SenderBee = this,
                            RelatedBee = nextBee,
                            ActionType = BeeActionType.AcceptBeeToEnter
                        });
                }),
                this.balance.TimeToCheckOneBee,
                this.CheckOneBee);
        }
    }
}