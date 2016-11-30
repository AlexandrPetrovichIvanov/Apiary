namespace Apiary.BeeWorkflowApiary.Bees
{
    using System;
    using System.Threading.Tasks;
    using Apiary.BeeWorkflowApiary.BeeActions;
    using Apiary.BeeWorkflowApiary.BeeRequests;
    using Apiary.BeeWorkflowApiary.Interfaces;
    using Apiary.Interfaces.Balancing;
    using Apiary.Utilities;

    /// <summary>
    /// Пчела-охранник.
    /// </summary>
    public class GuardBee : BeeBase
    {
        /// <summary>
        /// Баланс пчелы-охранника.
        /// </summary>
        private readonly IApiaryBalance balance;

        /// <summary>
        /// Очередь пчёл на посту охраны.
        /// </summary>
        private GuardPostQueue guardPostQueue;

        /// <summary>
        /// Получить тип пчелы.
        /// </summary>
        /// <returns>Тип пчелы.</returns>
        public override BeeType Type => BeeType.Guard;

        /// <summary>
        /// Создать пчелу-охранника.
        /// </summary>
        public GuardBee()
        {
            this.balance = ServiceLocator.Instance.GetService<IApiaryBalance>();
        }

        /// <summary>
        /// Метод получения действия, с которого нужно начинать работу.
        /// </summary>
        /// <returns>Действие, с которого нужно начинать работу.</returns>
        protected override Action GetStartAction()
        {
            return this.RequestGuardPostQueue;
        }

        /// <summary>
        /// Запросить у улья очередь поста охраны и начать работу.
        /// </summary>
        private void RequestGuardPostQueue()
        {
            var request = new BeeRequestEventArgs<GuardPostQueue>
            {
                RequestType = BeeRequestType.RequestGuardPostQueue
            };

            this.SafeSendRequest(request);

            if (!request.Succeed
                || request.TypedResponse == null)
            {
                throw new InvalidOperationException(
                    "Не удалось получить очередь поста охраны пчёл.");
            }

            this.guardPostQueue = request.TypedResponse;

            this.CheckOneBee();
        }

        /// <summary>
        /// Проверить одну пчелу.
        /// </summary>
        private async void CheckOneBee()
        {
            IBee nextBee = this.guardPostQueue.Dequeue();

            await Task.Delay(this.balance.GuardBalance.TimeToCheckOneBee);

            if (!this.isWorking)
            {
                return;
            }

            if (nextBee != null 
                && nextBee.BeehiveNumber == this.BeehiveNumber)
            {
                this.SafePerformAction(new BeeActionEventArgs
                {
                    SenderBee = this,
                    RelatedBee = nextBee,
                    ActionType = BeeActionType.AcceptBeeToEnter
                });
            }   

            Task.Factory.StartNew(this.CheckOneBee);
        }
    }
}