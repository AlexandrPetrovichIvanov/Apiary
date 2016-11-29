namespace Apiary.BeeWorkflowApiary.Bees
{
    using System;

    using Apiary.BeeWorkflowApiary.BeeActions;
    using Apiary.BeeWorkflowApiary.Interfaces;
    using Apiary.Interfaces.Balancing;
    using Apiary.Utilities;

    /// <summary>
    /// Пчела-матка.
    /// </summary>
    public class QueenBee : BeeBase
    {
        /// <summary>
        /// Баланс пчелы-матки.
        /// </summary>
        private readonly IQueenBeeBalance balance;

        /// <summary>
        /// Генератор случайных чисел.
        /// </summary>
        private readonly IRandomizer randomizer;

        /// <summary>
        /// Фабрика пчёл.
        /// </summary>
        private readonly IBeeFactory factory;

        /// <summary>
        /// Получить тип пчелы.
        /// </summary>
        /// <returns>Тип пчелы.</returns>
        public override BeeType Type => BeeType.Queen;

        /// <summary>
        /// Создать пчелу-матку.
        /// </summary>
        public QueenBee()
        {
            this.balance = ServiceLocator.Instance.GetService<IQueenBeeBalance>();
            this.randomizer = ServiceLocator.Instance.GetService<IRandomizer>();
            this.factory = ServiceLocator.Instance.GetService<IBeeFactory>();
        }

        /// <summary>
        /// Метод получения действия, с которого нужно начинать работу.
        /// </summary>
        /// <returns>Действие, с которого нужно начинать работу.</returns>
        protected override Action GetStartAction()
        {
            return this.StartProducingBees;
        }

        /// <summary>
        /// Начать производство пчёл.
        /// </summary>
        private void StartProducingBees()
        {
            this.PerformOperation(
                () => {},
                this.balance.TimeToProduceBee,
                this.ProduceBee);
        }

        /// <summary>
        /// Произвести одну пчелу.
        /// </summary>
        private void ProduceBee()
        {
            this.PerformOperation(
                () =>
                {
                    IBee newBee = this.CreateRandomBee();

                    this.ActionPerformedInternal(
                        this,
                        new BeeActionEventArgs
                        {
                            SenderBee = this,
                            RelatedBee = newBee,
                            ActionType = BeeActionType.ProduceBee
                        });
                },
                this.balance.TimeToProduceBee,
                this.ProduceBee);
        }

        /// <summary>
        /// Создать случайную пчелу (на основе баланса пчелы-матки).
        /// </summary>
        private IBee CreateRandomBee()
        {
            int random = this.randomizer.GetRandom(1, 1000);

            int workerLimit = this.balance.ThousandthPartToProduceWorker;
            int guardLimit = workerLimit + this.balance.ThousandthPartToProduceGuard;

            if (random <= workerLimit)
            {
                return this.factory.CreateWorkerBee(BeeWorkingState.OnTheRest);
            }
            else if (random <= guardLimit)
            {
                return this.factory.CreateGuardBee();
            }
            else 
            {
                return this.factory.CreateQueenBee();
            }
        }
    }
}