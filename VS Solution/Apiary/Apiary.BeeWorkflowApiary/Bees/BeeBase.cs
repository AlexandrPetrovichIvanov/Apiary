namespace Apiary.BeeWorkflowApiary.Bees
{
    using System;
    using System.Threading.Tasks;

    using Apiary.BeeWorkflowApiary.BeeActions;
    using Apiary.BeeWorkflowApiary.BeeRequests;
    using Apiary.BeeWorkflowApiary.Interfaces;
    using Apiary.Utilities;

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
        /// Имитатор выполнения длительных операций.
        /// </summary>
        private readonly ILongOperationSimulator longOperationSimulator;

        /// <summary>
        /// Признак того, что пчела работает в данный момент.
        /// </summary>
        private bool isWorking;

        /// <summary>
        /// Текущее выполняемое действие.
        /// </summary>
        private Task currentAction;

        /// <summary>
        /// Общая часть создания пчелы.
        /// </summary>
        protected BeeBase()
        {
            this.longOperationSimulator = ServiceLocator.Instance.GetService<ILongOperationSimulator>();
        }

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
        protected EventHandler<BeeActionEventArgs> ActionPerformedInternal;

        /// <summary>
        /// Делегат запроса пчелы к улью.
        /// </summary>
        protected EventHandler<BeeRequestEventArgs> RequestForBeehiveDataInternal;

        /// <summary>
        /// Внешняя точка доступа к событию выполнения пчелой какого-либо действия.
        /// </summary>
        public event EventHandler<BeeActionEventArgs> ActionPerformed
        {
            add
            {
                this.ThrowIfStillWorking();

                if (this.ActionPerformedInternal != null)
                {
                    throw new InvalidOperationException(BeeBase.AlreadyHasBeehiveLinked);
                }

                this.ActionPerformedInternal = value;
            }
            remove
            {
                if (this.ActionPerformedInternal != value)
                {
                    return;
                }

                this.ThrowIfStillWorking();
                this.ActionPerformedInternal = null;
            }
        }

        /// <summary>
        /// Внешняя точка доступа с событию запроса пчелой каких-либо данных из улья.
        /// </summary>
        public event EventHandler<BeeRequestEventArgs> RequestForBeehiveData
        {
            add
            {
                this.ThrowIfStillWorking();

                if (this.RequestForBeehiveDataInternal != null)
                {
                    throw new InvalidOperationException(BeeBase.AlreadyHasBeehiveLinked);
                }

                this.RequestForBeehiveDataInternal = value;
            }
            remove
            {
                if (this.RequestForBeehiveDataInternal != value)
                {
                    return;
                }

                this.ThrowIfStillWorking();
                this.RequestForBeehiveDataInternal = null;
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
            this.currentAction.GetAwaiter().GetResult();
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
                this.longOperationSimulator.SimulateAsync(timeBeforeNextAction, nextAction);
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
            Func<T, Action> selectNextAction)
        {
            this.DoAsCurrentAction(() =>
            {
                T actionResult = action();
                this.longOperationSimulator.SimulateAsync(
                    timeBeforeNextAction, 
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
        /// Проверить, связана ли пчела с ульем.
        /// </summary>
        private void CheckLinkWithBeehive()
        {
            if (this.ActionPerformedInternal == null
                || this.RequestForBeehiveDataInternal == null)
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