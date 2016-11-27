namespace Apiary.Tests.FunctionalTests.MathematicalApiary
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    /// <summary>
    /// Класс тестирования математической реализации ульев.
    /// </summary>
    [TestClass]
    public class MathBeehiveTests
    {
        /// <summary>
        /// Допустимая доля погрешности.
        /// </summary>
        private const double Inaccuracy = 0.001;

        /// <summary>
        /// Стандартный баланс пасеки.
        /// </summary>
        private IApiaryBalance balance;

        /// <summary>
        /// Создать экземпляр тестового класса.
        /// </summary>
        public MathBeehiveTests()
        {
            this.balance = new DefaultApiaryBalance();
        }

        /// <summary>
        /// Проверить сбор мёда при нормальных условиях.
        /// </summary>
        [TestMethod]
        public void MathBeehive_CorrectHoneyHarvesting()
        {
            IBeehiveState testState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = 300,
                BeesInsideCount = 300,
                WorkerBeesCount = 100,
                QueensCount = 0,
                GuardsCount = 200
            };

            MathBeehive beehive = this.CreateBeehiveWorkedForHour(testState);

            int expectedHoney = this.ExpectedHoneyFromOneBeePerHour 
                * testState.WorkerBeesCount;
            
            Assert.AreEqual(expectedHoney, beehive.HoneyCount);
        }

        /// <summary>
        /// Проверить воспроизводство пчёл при нормальных условиях.
        /// </summary>
        [TestMethod]
        public void MathBeehive_CorrectBeeProducing()
        {
            IBeehiveState testState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = 100,
                BeesInsideCount = 100,
                WorkerBeesCount = 0,
                QueensCount = 100,
                GuardsCount = 0
            };

            IApiaryBalance oldBalance = this.balance;

            this.balance = new ApiaryBalanceDontProducingQueens();

            try
            {
                MathBeehive beehive = this.CreateBeehiveWorkedForHour(testState);

                int expectedNewBees = this.ExpectedChildrenFromOneQueenPerHour
                    * testState.QueensCount;
                
                Assert.AreEqual(expectedNewBees, beehive.BeesTotalCount);
            }
            finally
            {
                this.balance = oldBalance;
            }
        }

        /// <summary>
        /// Проверить ограничение сбора мёда при недостаточном количестве охранников.
        /// </summary>
        [TestMethod]
        public void MathBeehive_CorrectGuardsLimitation()
        {
            IBeehiveState testState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = 10000,
                BeesInsideCount = 10000,
                WorkerBeesCount = 9990,
                QueensCount = 0,
                GuardsCount = 10
            };

            MathBeehive beehive = this.CreateBeehiveWorkedForHour(testState);

            int expectedHoney = this.ExpectedChecksPerHourFromOneGuard 
                * testState.GuardsCount;
            
            Assert.AreEqual(expectedHoney, beehive.HoneyCount);
        }

        /// <summary>
        /// Проверить, не могут ли несколько охранников проверять одну и ту же пчелу,
        /// тем самым делая проверку пчелы быстрее, чем нужно согласно требованиям.
        /// </summary>
        [TestMethod]
        public void MathBeehive_OneGuardToOneBee()
        {
            IBeehiveState testState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = 1,
                BeesInsideCount = 10000,
                WorkerBeesCount = 1,
                QueensCount = 0,
                GuardsCount = 9999
            };

            IApiaryBalance oldBalance = this.balance;

            this.balance = new SlowGuardApiaryBalance();

            try
            {
                MathBeehive beehive = this.CreateBeehiveWorkedForHour(testState);

                int expectedHoney = this.ExpectedHoneyFromOneBeePerHour 
                    * testState.GuardsCount;
                
                Assert.AreEqual(expectedHoney, beehive.HoneyCount);
            }
            finally
            {
                this.balance = oldBalance;
            }
        }   

        /// <summary>
        /// Проверить, не изменяется ли количество пчёл при работе улья без маток.
        /// </summary>
        [TestMethod]
        public void MathBeehive_BeesCountStable()
        {
            int initialBeesCount = 10000;

            IBeehiveState testState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = initialBeesCount,
                BeesInsideCount = initialBeesCount,
                WorkerBeesCount = initialBeesCount - 1000,
                QueensCount = 0,
                GuardsCount = 1000
            };

            MathBeehive beehive = this.CreateBeehiveWorkedForHour(testState);

            Assert.AreEqual(initialBeesCount, beehive.BeesTotalCount);
        }

        /// <summary>
        /// Получить ожидаемое количество мёда, собранное одной пчелой
        /// в течение часа.
        /// </summary>
        /// <returns>Ожидаемое количество мёда (согласно балансу).</returns>
        private int ExpectedHoneyFromOneBeePerHour
        {
            get
            {
                int timeForOnePortionMs = 
                    (int)this.balance.WorkerBalance.TimeToHarvestHoney.TotalMilliseconds()
                    + (int)this.balance.WorkerBalance.TimeToRestInBeehive.TotalMilliseconds()
                    + (int)this.balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds();

                int hourInMs = 1000 * 60 * 60;

                double result = this.MillisecondsInHour / timeForOnePortionMs;

                return (int)(result - (result * MathBeehiveTests.Inaccuracy));
            }
        }

        /// <summary>
        /// Ожидаемое количество пчёл, произведенное одной маткой в час.
        /// </summary>
        /// <returns>Ожидаемое количество пчёл (согласно балансу).</returns>
        private int ExpectedChildrenFromOneQueenPerHour
        {
            get
            {
                int timeForOneChild = 
                    (int)this.balance.QueenBalance.TimeToProduceOneBee.TotalMilliseconds();

                double result = this.MillisecondsInHour / timeForOneChild;

                return (int)(result - (result * MathBeehiveTests.Inaccuracy));
            }
        }

        /// <summary>
        /// Ожидаемое количество проверок пчёл охранниками.
        /// </summary>
        /// <returns>Максимальное количество работающих пчёл.</returns>
        private int ExpectedChecksPerHourFromOneGuard
        {
            get
            {
                int timeToCheckOneBee = 
                    (int)this.balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds();

                double result = this.MillisecondsInHour / timeToCheckOneBee;

                return (int)(result - (result * MathBeehiveTests.Inaccuracy));
            }
        }

        /// <summary>
        /// Количество миллисекунд в часе.
        /// </summary>
        private int MillisecondsInHour => 1000 * 60 * 60;

        /// <summary>
        /// Создать улей, и запустить его, как будто он работал
        /// в течение часа.
        /// </summary>
        /// <param name="state">Состояние улья.</param>
        /// <returns>Улей, отработавший час и остановленный.</returns>
        private MathBeehive CreateBeehiveWorkedForHour(IBeehiveState state)
        {
            MathBeehive beehive = new MathBeehive(state, this.balance);

            for (int i = 0; i < this.GetIterationsInHour(beehive); i++)
            {
                beehive.SingleIteration();
            }

            beehive.Stop();
        }

        /// <summary>
        /// Получить количество итераций, которые необходимо произвести
        /// для имитации работы улья в течение часа.
        /// </summary>
        /// <param name="beehive">Улей.</param>
        /// <returns>Количество итераций.</returns>
        private int GetIterationsInHour(MathBeehive beehive)
        {
            return 1000 / beehive.IntervalMs * 60 * 60;
        }
    }
}