namespace Apiary.Tests.TestDoubles.Bees
{
    using System;
    using System.Threading.Tasks;
    using Apiary.BeeWorkflowApiary.BeeActions;
    using Apiary.BeeWorkflowApiary.BeeRequests;
    using Apiary.BeeWorkflowApiary.Bees;
    using Apiary.BeeWorkflowApiary.Interfaces;

    /// <summary>
    /// Пчела, просто посылающая сообщения и запросы,
    /// но не выполняющая реальной работы.
    /// </summary>
    public class EmptyBaseBee : BeeBase
    {
        /// <summary>
        /// Интервал между пустыми операциями.
        /// </summary>
        internal const int IntervalMs = 300;

        /// <summary>
        /// Получить тип пчелы.
        /// </summary>
        /// <returns>Тип пчелы.</returns>
        public override BeeType Type => BeeType.NotSet;

        /// <summary>
        /// Метод получения действия, с которого нужно начинать работу.
        /// </summary>
        /// <returns>Действие, с которого нужно начинать работу.</returns>
        protected override Action GetStartAction()
        {
            return this.SendMessageAndRequest;
        }

        /// <summary>
        /// Бесконечно посылать сообщения и запросы.
        /// </summary>
        private async void SendMessageAndRequest()
        {
            this.SafePerformAction(
                new BeeActionEventArgs
                {
                    SenderBee = this,
                    RelatedBee = null,
                    ActionType = BeeActionType.NotSet
                });

            await Task.Delay(TimeSpan.FromMilliseconds(EmptyBaseBee.IntervalMs));

            Task.Factory.StartNew(this.SendMessageAndRequest);
        }
    }
}