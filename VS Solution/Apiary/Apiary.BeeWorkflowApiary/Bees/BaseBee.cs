namespace Apiary.BeeWorkflowApiary.Bees
{
    using System;
    using System.Threading.Tasks;

    using Apiary.BeeWorkflowApiary.BeeActions;
    using Apiary.BeeWorkflowApiary.BeeRequests;
    using Apiary.BeeWorkflowApiary.Interfaces;

    /// <summary>
    /// Пчела (общая часть реализации всех видов пчёл).
    /// </summary>
    public abstract class BeeBase : IBee
    {
        /// <summary>
        /// Сообщение об ошибке повторной связи с ульем.
        /// </summary>
        private const string AlreadyHasBeehiveLinked = "Пчела уже связана с этим или другим ульем.";

        /// <summary>
        /// Признак того, что пчела работает в данный момент.
        /// </summary>
        protected bool isWorking;

        /// <summary>
        /// Получить тип пчелы.
        /// </summary>
        /// <returns>Тип пчелы.</returns>
        public abstract BeeType Type { get; }

        /// <summary>
        /// Получить или задать номер улья.
        /// </summary>
        /// <returns>Номер улья.</returns>
        public int BeehiveNumber { get; set; }

        /// <summary>
        /// Делегат выполнения пчелой какого-либо действия.
        /// </summary>
        private EventHandler<BeeActionNotification> actionPerformedInternal;

        /// <summary>
        /// Делегат запроса пчелы к улью.
        /// </summary>
        private EventHandler<BeeRequest> requestForBeehiveDataInternal;

        /// <summary>
        /// Внешняя точка доступа к событию выполнения пчелой какого-либо действия.
        /// </summary>
        public event EventHandler<BeeActionNotification> ActionPerformed
        {
            add
            {
                this.ThrowIfStillWorking();

                if (this.actionPerformedInternal != null)
                {
                    throw new InvalidOperationException(BeeBase.AlreadyHasBeehiveLinked);
                }

                this.actionPerformedInternal = value;
            }
            remove
            {
                if (this.actionPerformedInternal != value)
                {
                    return;
                }

                this.ThrowIfStillWorking();
                this.actionPerformedInternal = null;
            }
        }

        /// <summary>
        /// Внешняя точка доступа с событию запроса пчелой каких-либо данных из улья.
        /// </summary>
        public event EventHandler<BeeRequest> RequestForBeehiveData
        {
            add
            {
                this.ThrowIfStillWorking();

                if (this.requestForBeehiveDataInternal != null)
                {
                    throw new InvalidOperationException(BeeBase.AlreadyHasBeehiveLinked);
                }

                this.requestForBeehiveDataInternal = value;
            }
            remove
            {
                if (this.requestForBeehiveDataInternal != value)
                {
                    return;
                }

                this.ThrowIfStillWorking();
                this.requestForBeehiveDataInternal = null;
            }
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
        }

        /// <summary>
        /// Метод получения действия, с которого нужно начинать работу.
        /// </summary>
        /// <returns>Действие, с которого нужно начинать работу.</returns>
        protected abstract Action GetStartAction();

        /// <summary>
        /// Отправить в улей отчет о действии, если работа еще не остановлена.
        /// </summary>
        /// <param name="args">Описание действия.</param>
        protected void SafePerformAction(BeeActionNotification args)
        {
            if (!this.isWorking)
            {
                return;
            }

            this.actionPerformedInternal(this, args);
        }

        /// <summary>
        /// Отправить запрос улью, если работа еще не остановлена.
        /// </summary>
        /// <param name="args">Запрос.</param>
        protected void SafeSendRequest(BeeRequest args)
        {
            if (!this.isWorking)
            {
                return;
            }

            this.requestForBeehiveDataInternal(this, args);
        }

        /// <summary>
        /// Проверить, связана ли пчела с ульем.
        /// </summary>
        private void CheckLinkWithBeehive()
        {
            if (this.actionPerformedInternal == null
                || this.requestForBeehiveDataInternal == null)
            {
                throw new InvalidOperationException(
                    "Пчела не связана с ульем.");
            }
        }

        /// <summary>
        /// Сгенерировать исключение, если пчела в данный момент работает.
        /// </summary>
        private void ThrowIfStillWorking()
        {
            if (this.isWorking)
            {
                throw new InvalidOperationException(
                    "Нельзя устанавливать или обрывать связь с ульем, пока пчела работает.");
            }
        }
    }
}