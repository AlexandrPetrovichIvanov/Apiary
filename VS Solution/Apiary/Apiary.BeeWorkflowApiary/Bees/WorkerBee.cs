namespace Apiary.BeeWorkflowApiary.Bees
{
    using System;
    using Apiary.BeeWorkflowApiary.BeeActions;
    using Apiary.BeeWorkflowApiary.BeeRequests;
    using Apiary.BeeWorkflowApiary.Interfaces;
    using Apiary.Interfaces.Balancing;
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
        /// Получить тип пчелы.
        /// </summary>
        /// <returns>Тип пчелы.</returns>
        public override BeeType Type => BeeType.Worker;

        /// <summary>
        /// Создать рабочую пчелу.
        /// </summary>
        /// <param name="initialState">Изначальное состояние рабочей пчелы.</param>
        public WorkerBee(BeeWorkingState initialState)
        {
            this.balance = ServiceLocator.Instance.GetService<IWorkerBeeBalance>();
            this.guardBalance = ServiceLocator.Instance.GetService<IGuardBeeBalance>();
            this.initialState = initialState;
        }

        /// <summary>
        /// Получить действие, с которого необходимо начать работу.
        /// </summary>
        protected override Action GetStartAction()
        {
            switch (this.initialState)
            {
                case BeeWorkingState.OnTheWork:
                    return this.HarvestHoney;
                case BeeWorkingState.OnTheRest:
                    return this.EnteringGuardPost;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(this.initialState),
                        "Задано неверное изначальное состояние рабочей пчелы");
            }
        }

        /// <summary>
        /// Собрать мёд.
        /// </summary>
        private void HarvestHoney()
        {
            this.PerformOperation(
                () =>
                {
                    this.ActionPerformedInternal(
                        this,
                        new BeeActionEventArgs
                        {
                            SenderBee = this,
                            ActionType = BeeActionType.LeftBeehiveToHarvestHoney
                        });
                },
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
                    this.ActionPerformedInternal(
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
            BeeRequestEventArgs request = new BeeRequestEventArgs
            {
                RequestType = BeeRequestType.RequestToEnterBeehive
            };

            this.RequestForBeehiveDataInternal(this, request);

            if (request.Succeed)
            {
                this.GoToRest();
            }
            else
            {
                this.PerformOperation(
                    null,
                    this.guardBalance.TimeToCheckOneBee,
                    this.RequestToEnterBeehive);
            }
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
            this.ActionPerformedInternal(
                this,
                new BeeActionEventArgs
                {
                    SenderBee = this,
                    ActionType = BeeActionType.EnterBeehiveWithHoney
                });
        }
    }
}