namespace Apiary.Tests.FunctionalTests.BeeWorkflowApiary
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.Implementation.Common;
    
    using Apiary.Interfaces.Balancing;
    using Apiary.Tests.TestDoubles.Balances;
    using Apiary.Utilities;
    using Apiary.Interfaces;
    using Apiary.Client.XmlStates;
    using Apiary.Tests.Common;
    using Apiary.BeeWorkflowApiary;
    using Apiary.BeeWorkflowApiary.Interfaces;

    /// <summary>
    /// Класс тестирования объектно-ориентированного улья.
    /// </summary>
    [TestClass]
    public class BeeWorkflowBeehiveTests
    {
        /// <summary>
        /// Допустимая погрешность.
        /// </summary>
        private const double Inaccuracy = 0.2;

        /// <summary>
        /// Баланс "быстрой" пасеки.
        /// </summary>
        private readonly FastApiaryBalance balance = new FastApiaryBalance(new DefaultApiaryBalance());

        /// <summary>
        /// Подготовить тесты к выполнению.
        /// </summary>
        public BeeWorkflowBeehiveTests()
        {
            ServiceLocator.Instance.RegisterService<IRandomizer>(new Randomizer());
            ServiceLocator.Instance.RegisterService<IBeeFactory>(new DefaultBeeFactory());
            ServiceLocator.Instance.RegisterService<IApiaryBalance>(this.balance);
        }

        /// <summary>
        /// Проверить сбор мёда при нормальных условиях.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_CorrectHoneyHarvesting()
        {
            IBeehiveState initialState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = 5000,
                BeesInsideCount = 5000,
                WorkerBeesCount = 1000,
                QueensCount = 0,
                GuardsCount = 4000
            };

            IBeehiveState newState = this.LaunchBeehiveForFiveSeconds(initialState);

            int expectedHoney = this.HoneyPerFiveSecondsFromOneBee 
                * initialState.WorkerBeesCount;

            Assert.IsTrue(Math.Abs(expectedHoney - newState.HoneyCount)
                <= expectedHoney * BeeWorkflowBeehiveTests.Inaccuracy);
        }

        /// <summary>
        /// Проверить воспроизводство пчёл при нормальных условиях.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_CorrectBeeProducing()
        {
            IBeehiveState initialState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = 1000,
                BeesInsideCount = 1000,
                WorkerBeesCount = 0,
                QueensCount = 1000,
                GuardsCount = 0
            };            

            IBeehiveState newState = this.LaunchBeehiveForFiveSeconds(initialState);

            int expectedNewBees = this.ChildrenFromOneQueenPerFiveSeconds
                * initialState.QueensCount;
                
            Assert.IsTrue(Math.Abs(expectedNewBees - newState.BeesTotalCount)
                <= expectedNewBees * BeeWorkflowBeehiveTests.Inaccuracy);
        }

        /// <summary>
        /// Проверить ограничение сбора мёда при недостаточном количестве охранников.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_CorrectGuardsLimitation()
        {
            IBeehiveState initialState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = 10000,
                BeesInsideCount = 10000,
                WorkerBeesCount = 9900,
                QueensCount = 0,
                GuardsCount = 100
            };

            IBeehiveState newState = this.LaunchBeehiveForFiveSeconds(initialState);

            int expectedHoney = this.ChecksFromOneGuardPerFiveSeconds 
                * initialState.GuardsCount;

            Assert.IsTrue(Math.Abs(expectedHoney - newState.HoneyCount)
                <= expectedHoney * BeeWorkflowBeehiveTests.Inaccuracy);
        }

        /// <summary>
        /// Проверить, не могут ли несколько охранников проверять одну и ту же пчелу,
        /// тем самым делая проверку пчелы быстрее, чем нужно согласно требованиям.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_OneGuardToOneBee()
        {
            IBeehiveState initialState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = 10000,
                BeesInsideCount = 10000,
                WorkerBeesCount = 200,
                QueensCount = 0,
                GuardsCount = 9800
            };

            IBeehiveState newState = this.LaunchBeehiveForFiveSeconds(initialState);

            int expectedHoney = this.HoneyPerFiveSecondsFromOneBee 
                * initialState.WorkerBeesCount;

            Assert.IsTrue(Math.Abs(expectedHoney - newState.HoneyCount)
                <= expectedHoney * BeeWorkflowBeehiveTests.Inaccuracy);
        }

        /// <summary>
        /// Проверить, не изменяется ли количество пчёл при работе улья без маток.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_BeesCountStable()
        {
            IBeehiveState initialState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = 10000,
                BeesInsideCount = 10000,
                WorkerBeesCount = 9000,
                QueensCount = 0,
                GuardsCount = 1000
            };

            IBeehiveState newState = this.LaunchBeehiveForFiveSeconds(initialState);

            Assert.AreEqual(initialState.BeesTotalCount, newState.BeesTotalCount);
        }

                /// <summary>
        /// Проверить сбор мёда пчёлами, которые на момент начала находятся вне улья.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_OutsideWorkersNormalStartTheirWork()
        {
            IBeehiveState initialState = new BeehiveXmlState
            {
                HoneyCount = 500,
                BeesTotalCount = 1900,
                BeesInsideCount = 1000,
                WorkerBeesCount = 900,
                QueensCount = 0,
                GuardsCount = 1000
            };

            IBeehiveState newState = this.LaunchBeehiveForFiveSeconds(initialState);

            long expectedHoney = this.HoneyPerFiveSecondsFromOneBee 
                * initialState.WorkerBeesCount
                + initialState.HoneyCount;

            Assert.IsTrue(Math.Abs(expectedHoney - newState.HoneyCount)
                <= expectedHoney * BeeWorkflowBeehiveTests.Inaccuracy);
        }

        /// <summary>
        /// Проверить, что улей не стартует при некорректных начальных условиях.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_NotStartWithIncorrectState()
        {
            foreach (IBeehiveState wrongState 
                in WrongBeehiveStates.GetWrongStates())
            {
                try
                {
                    BeeWorkflowBeehive beehive = new BeeWorkflowBeehive();
                    beehive.Start(wrongState);

                    throw new AssertFailedException(
                        "Улей не должен был запуститься с некорректным состоянием");
                }
                catch (ValidationException)
                {
                    // Так и должно быть
                }              
            }
        }

        /// <summary>
        /// Создать улей, запустить его на 5 секунд, и остановить.
        /// </summary>
        /// <param name="initialState">Изначальное состояние улья.</param>
        /// <returns>Состояние отработавшего улья.</returns>
        private IBeehiveState LaunchBeehiveForFiveSeconds(IBeehiveState initialState)
        {
            BeeWorkflowBeehive beehive = new BeeWorkflowBeehive();
            beehive.Start(initialState);
            Task.Delay(5000).GetAwaiter().GetResult();
            IBeehiveState state = beehive.Stop();
            state.Validate();
            return state;
        }

        /// <summary>
        /// Получить ожидаемое количество мёда, собранное одной пчелой
        /// в течение пяти секунд.
        /// </summary>
        /// <returns>Ожидаемое количество мёда (согласно балансу).</returns>
        private int HoneyPerFiveSecondsFromOneBee
        {
            get
            {
                int timeForOnePortionMs = 
                    (int)this.balance.WorkerBalance.TimeToHarvestHoney.TotalMilliseconds
                    + (int)this.balance.WorkerBalance.TimeToRestInBeehive.TotalMilliseconds
                    + (int)this.balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds;

                return (5000 / timeForOnePortionMs);
            }
        }

        /// <summary>
        /// Ожидаемое количество пчёл, произведенное одной маткой за 5 секунд.
        /// </summary>
        /// <returns>Ожидаемое количество пчёл (согласно балансу).</returns>
        private int ChildrenFromOneQueenPerFiveSeconds
        {
            get
            {
                int timeForOneChild = 
                    (int)this.balance.QueenBalance.TimeToProduceBee.TotalMilliseconds;

                return 5000 / timeForOneChild; // при завершении работы пчела заканчивает производство пчелы.
            }
        }

        /// <summary>
        /// Ожидаемое количество проверок пчёл одним охранником за 5 секунд.
        /// </summary>
        /// <returns>Количество проверок охранником за 5 секунд.</returns>
        private int ChecksFromOneGuardPerFiveSeconds
        {
            get
            {
                int timeToCheckOneBee = 
                    (int)this.balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds;

                return 5000 / timeToCheckOneBee;
            }
        }
    }
}