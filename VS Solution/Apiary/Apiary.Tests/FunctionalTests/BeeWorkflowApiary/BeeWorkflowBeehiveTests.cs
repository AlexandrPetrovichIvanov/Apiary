namespace Apiary.Tests.FunctionalTests.BeeWorkflowApiary
{
    using System;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    /// <summary>
    /// Класс тестирования объектно-ориентированного улья.
    /// </summary>
    [TestClass]
    public class BeeWorkflowBeehiveTests
    {
        /// <summary>
        /// Допустимая погрешность.
        /// </summary>
        private const double Inaccuracy = 0.000;

        /// <summary>
        /// Баланс "быстрой" пасеки.
        /// </summary>
        private readonly IApiaryBalance balance = new FastApiaryBalance();

        /// <summary>
        /// Подготовить тесты к выполнению.
        /// </summary>
        public BeeWorkflowBeehiveTests()
        {
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
                BeesTotalCount = 300,
                BeesInsideCount = 300,
                WorkerBeesCount = 100,
                QueensCount = 0,
                GuardsCount = 200
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
                BeesTotalCount = 100,
                BeesInsideCount = 100,
                WorkerBeesCount = 0,
                QueensCount = 100,
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
                BeesTotalCount = 1000,
                BeesInsideCount = 1000,
                WorkerBeesCount = 990,
                QueensCount = 0,
                GuardsCount = 10
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
                BeesTotalCount = 1000,
                BeesInsideCount = 1000,
                WorkerBeesCount = 1,
                QueensCount = 0,
                GuardsCount = 999
            };

            IBeehiveState newState = this.LaunchBeehiveForFiveSeconds(initialState);

            int expectedHoney = this.HoneyPerFiveSecondsFromOneBee;

            Assert.IsTrue(Math.Abs(expectedHoney - newState.HoneyCount)
                <= 0/*expectedHoney * BeeWorkflowBeehiveTests.Inaccuracy*/);
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
                BeesTotalCount = 1000,
                BeesInsideCount = 1000,
                WorkerBeesCount = 900,
                QueensCount = 0,
                GuardsCount = 100
            };

            IBeehiveState newState = this.LaunchBeehiveForFiveSeconds(initialState);

            Assert.AreEqual(1000, newState.BeesTotalCount);
        }

                /// <summary>
        /// Проверить сбор мёда пчёлами, которые на момент начала находятся вне улья.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_OutsideWorkersNormalStartTheirWork()
        {
            IBeehiveState initialState = new BeehiveXmlState
            {
                HoneyCount = 5,
                BeesTotalCount = 10,
                BeesInsideCount = 1,
                WorkerBeesCount = 9,
                QueensCount = 0,
                GuardsCount = 1
            };

            IBeehiveState newState = this.LaunchBeehiveForFiveSeconds(initialState);

            int expectedHoney = this.HoneyPerFiveSecondsFromOneBee 
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

                    throw new AssertionException(
                        "Улей не должен был запуститься с некорректным состоянием");
                }
                catch (ArgumentException)
                {
                    // Так и должно быть
                }              
            }
        }

        /// <summary>
        /// Создать улей, запустить его на 5 секунд, и остановить.
        /// </summary>
        /// <param name="state">Состояние улья.</param>
        /// <returns>Состояние отработавшего улья.</returns>
        private IBeehiveState LaunchBeehiveForFiveSeconds(IBeehiveState initialState)
        {
            BeeWorkflowBeehive beehive = new BeeWorkflowBeehive();
            beehive.Start(state);
            Task.Delay(5000).GetAwaiter().GetResult();
            return beehive.Stop();
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
                    (int)this.Balance.WorkerBalance.TimeToHarvestHoney.TotalMilliseconds
                    + (int)this.Balance.WorkerBalance.TimeToRestInBeehive.TotalMilliseconds
                    + (int)this.Balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds;

                double result = (double)5000 / timeForOnePortionMs;

                return (int)result;
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
                    (int)this.Balance.QueenBalance.TimeToProduceBee.TotalMilliseconds;

                double result = (double)5000 / timeForOneChild
                    + 1; // при завершении работы пчела заканчивает производство пчелы.

                return (int)result;
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
                    (int)this.Balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds;

                double result = (double)5000 / timeToCheckOneBee;

                return (int)result;
            }
        }
    }
}