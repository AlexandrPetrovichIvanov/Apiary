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
    /// Рабочая пчела.
    /// </summary>
    public class WorkerBee : BeeBase
    {
        /// <summary>
        /// Баланс пасеки.
        /// </summary>
        private readonly IApiaryBalance balance;

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
            this.balance = ServiceLocator.Instance.GetService<IApiaryBalance>();
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
                    return this.EnterGuardPost;
                case BeeWorkingState.OnTheRest:
                    return this.HarvestHoney;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(this.initialState),
                        "Задано неверное изначальное состояние рабочей пчелы");
            }
        }

        /// <summary>
        /// Собрать мёд.
        /// </summary>
        private async void HarvestHoney()
        {
            if (!this.isWorking)
            {
                return;
            }

            this.SafePerformAction(new BeeActionNotification
            {
                SenderBee = this,
                ActionType = BeeActionType.LeftBeehiveToHarvestHoney
            });

            await Task.Delay(this.balance.WorkerBalance.TimeToHarvestHoney);

            this.EnterGuardPost();
        }

        /// <summary>
        /// Зайти на пост охраны.
        /// </summary>
        private async void EnterGuardPost()
        {
            if (!this.isWorking)
            {
                return;
            }

            this.SafePerformAction(new BeeActionNotification
            {
                SenderBee = this,
                ActionType = BeeActionType.EnterGuardPost
            });
            
            await Task.Delay(this.balance.GuardBalance.TimeToCheckOneBee);

            this.RequestToEnterBeehive();
        }

        /// <summary>
        /// Запросить вход в улей на посте охраны.
        /// </summary>
        private async void RequestToEnterBeehive()
        {
            if (!this.isWorking)
            {
                return;
            }

            BeeRequest request = new BeeRequest
            {
                RequestType = BeeRequestType.RequestToEnterBeehive
            };

            this.SafeSendRequest(request);

            // startnew вместо обычного вызова - для
            // предотвращения возможного зацикливания
            if (request.Succeed)
            {
                await Task.Factory.StartNew(this.GoToRest);
            }
            else
            {
                await Task.Delay(this.balance.GuardBalance.TimeToCheckOneBee);
                await Task.Factory.StartNew(this.RequestToEnterBeehive);
            }
        }

        /// <summary>
        /// Сдать мёд и пойти отдыхать.
        /// </summary>
        private async void GoToRest()
        {
            if (!this.isWorking)
            {
                return;
            }

            this.DeliverHoney();
            await Task.Delay(this.balance.WorkerBalance.TimeToRestInBeehive);
            this.HarvestHoney();
        }

        /// <summary>
        /// Сдать мёд.
        /// </summary>
        private void DeliverHoney()
        {
            if (!this.isWorking)
            {
                return;
            }

            this.SafePerformAction(new BeeActionNotification
            {
                SenderBee = this,
                ActionType = BeeActionType.EnterBeehiveWithHoney
            });
        }
    }
}