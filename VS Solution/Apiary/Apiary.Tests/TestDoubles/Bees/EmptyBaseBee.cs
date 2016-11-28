namespace Apiary.Tests.TestDoubles.Bees
{
    /// <summary>
    /// Пчела, просто посылающая сообщения и запросы,
    /// но не выполняющая реальной работы.
    /// </summary>
    public class EmptyBaseBee : BeeBase
    {
        /// <summary>
        /// Получить тип пчелы.
        /// </summary>
        /// <returns>Тип пчелы.</returns>
        public BeeType Type => BeeType.NotSet;

        /// <summary>
        /// Метод получения действия, с которого нужно начинать работу.
        /// </summary>
        /// <returns>Действие, с которого нужно начинать работу.</returns>
        private override Action GetStartAction()
        {
            return this.SendMessageAndRequest();
        }

        /// <summary>
        /// Послать сообщение и запрос.
        /// </summary>
        private void SendMessageAndRequest()
        {
            this.PerformOperation(
                () =>
                {
                    this.ActionPerformed.Invoke(
                        this,
                        new BeeActionEventArgs
                        {
                            SenderBee = this,
                            RelatedBee = null,
                            ActionType = BeeActionType.NotSet
                        });

                    BeeRequestEventArgs request = new BeeRequestEventArgs
                    {
                        RequestType = BeeRequestType.NotSet
                    };

                    this.RequestForBeehiveData(this, request);
                }),
                TimeSpan.FromMilliseconds(30),
                this.EmptyWork);
        }
    }
}