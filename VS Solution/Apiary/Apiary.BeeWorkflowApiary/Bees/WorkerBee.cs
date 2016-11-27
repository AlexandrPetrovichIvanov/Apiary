namespace Apiary.BeehiveWorkflowApiary.Bees
{
    using System;

    using Apiary.BeeWorkflowApiary.BeeActions;
    using Apiary.BeeWorkflowApiary.BeeRequests;
    using Apiary.Utilities;

    /// <summary>
    /// Рабочая пчела.
    /// </summary>
    public class WorkerBee : BeeBase
    {
        /// <summary>
        /// Баланс рабочей пчелы.
        /// </summary>
        private readonly IWorkerBeeBalance balance;

        /// <summary>
        /// Баланс работы охранников.
        /// </summary>
        private readonly IGuardBeeBalance guardBalance;

        /// <summary>
        /// Изначальное состояние рабочей пчелы.
        /// </summary>
        private readonly BeeWorkingState initialState;

        /// <summary>
        /// Работа пчелы остановлена.
        /// </summary>
        private bool isWorking = false;

        /// <summary>
        /// Получить тип пчелы.
        /// </summary>
        /// <returns>Тип пчелы.</returns>
        public BeeType Type => BeeType.Worker;

        /// <summary>
        /// Создать рабочую пчелу.
        /// </summary>
        /// <param name="initialState">Изначальное состояние рабочей пчелы.</param>
        public WorkerBee(BeeWorkingState initialState)
            : base()
        {
            this.balance = ServiceLocator.Instance.GetService<IWorkerBeeBalance>();
            this.guardBalance = ServiceLocator.Instance.GetService<IGuardBeeBalance>();
            this.initialState = initialState;
        }

        /// <summary>
        /// Получить действие, с которого необходимо начать работу.
        /// </summary>
        private override GetStartAction()
        {
            switch (this.initialState)
            {
                case BeeWorkingState.OnTheWork:
                    return this.HarvestHoney;
                case BeeWorkingState.OnTheRest:
                    return this.GoToRest;
                default:
                    throw new ArgumentOutOfRangeException(
                        "Задано неверное изначальное состояние рабочей пчелы",
                        nameof(this.initialState))
            }
        }

        /// <summary>
        /// Собрать мёд.
        /// </summary>
        private void HarvestHoney()
        {
            this.PerformOperation(
                Action.Empty,
                this.balance.TimeToHarvestHoney,
                this.EnteringGuardPost);
        }

        /// <summary>
        /// Зайти на пост охраны.
        /// </summary>
        private void EnteringGuardPost()
        {
            this.PerformOperation(
                () => 
                {
                    this.ActionPerformed.Invoke(
                        this,
                        new BeeActionEventArgs
                        {
                            SenderBee = this,
                            ActionType = BeeActionType.EnterGuardPost
                        });
                },
                this.guardBalance.TimeToCheckOneBee,
                this.RequestToEnterBeehive);
        }

        /// <summary>
        /// Запросить вход в улей на посте охраны.
        /// </summary>
        private void RequestToEnterBeehive()
        {
            this.PerformOperation(
                () => 
                {
                    BeeRequestEventArgs request = new BeeRequestEventArgs
                    {
                        RequestType = BeeRequestType.RequestToEnterBeehive
                    };

                    this.RequestForBeehiveData(this, request)

                    return request;
                },
                this.guardBalance.TimeToCheckOneBee,
                this.SelectActionAfterEnterRequest);
        }

        /// <summary>
        /// Решить, что делать дальше, в зависимости от 
        /// результата запроса на вход в улей.
        /// </summary>
        /// <param name="request">Рассмотренный запрос на вход в улей.</param>
        /// <returns>Действие, которое необходимо делать дальше.</returns>
        private Action SelectActionAfterEnterRequest(
            BeeRequestEventArgs request)
        {
            return request.Succeed 
                ? this.GoToRest
                : this.RequestToEnterBeehive;
        }

        /// <summary>
        /// Сдать мёд и пойти отдыхать.
        /// </summary>
        private void GoToRest()
        {
            this.PerformOperation(
                this.DeliverHoney,
                this.balance.TimeToRestInBeehive,
                this.HarvestHoney);
        }

        /// <summary>
        /// Сдать мёд.
        /// </summary>
        private void DeliverHoney()
        {
            this.ActionPerformed.Invoke(
                this,
                new BeeActionEventArgs
                {
                    SenderBee = this,
                    ActionType = BeeActionType.EnterBeehiveWithHoney
                });
        }

        /// <summary>
        /// Проверить связь пчелы с ульем.
        /// </summary>
        private void CheckLinkWithBeehive()
        {
            if (this.ActionPerformed == null
                || this.RequestForBeehiveData == null)
            {
                throw new InvalidOperationException(
                    "Невозможно начать работу, т.к. связь с ульем не установлена.");
            }
        }
    }
}