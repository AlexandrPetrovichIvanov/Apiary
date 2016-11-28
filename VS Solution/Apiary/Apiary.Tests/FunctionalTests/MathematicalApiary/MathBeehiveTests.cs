namespace Apiary.Tests.FunctionalTests.MathematicalApiary
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.Client.XmlStates;
    using Apiary.Interfaces;
    using Apiary.Interfaces.Balancing;
    using Apiary.Tests.TestDoubles.Balances;
    using Apiary.Utilities;
    using Apiary.Implementation.Common;
    using Apiary.MathematicalApiary;

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
        /// Проверить сбор мёда при нормальных условиях.
        /// </summary>
        [TestMethod]
        public void MathBeehive_CorrectHoneyHarvesting()
        {
            ServiceLocator.Instance.RegisterService<IApiaryBalance>(new DefaultApiaryBalance());

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
            ServiceLocator.Instance.RegisterService<IApiaryBalance>(
                new ApiaryBalanceDontProducingQueens(new DefaultApiaryBalance()));

            IBeehiveState testState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = 100,
                BeesInsideCount = 100,
                WorkerBeesCount = 0,
                QueensCount = 100,
                GuardsCount = 0
            };            

            MathBeehive beehive = this.CreateBeehiveWorkedForHour(testState);

            int expectedNewBees = this.ExpectedChildrenFromOneQueenPerHour
                * testState.QueensCount;
                
            Assert.AreEqual(expectedNewBees, beehive.BeesTotalCount);
        }

        /// <summary>
        /// Проверить ограничение сбора мёда при недостаточном количестве охранников.
        /// </summary>
        [TestMethod]
        public void MathBeehive_CorrectGuardsLimitation()
        {
            ServiceLocator.Instance.RegisterService<IApiaryBalance>(new DefaultApiaryBalance());

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
            ServiceLocator.Instance.RegisterService<IApiaryBalance>(
                new SlowGuardsApiaryBalance(new DefaultApiaryBalance()));

            IBeehiveState testState = new BeehiveXmlState
            {
                HoneyCount = 0,
                BeesTotalCount = 1,
                BeesInsideCount = 10000,
                WorkerBeesCount = 1,
                QueensCount = 0,
                GuardsCount = 9999
            };

            MathBeehive beehive = this.CreateBeehiveWorkedForHour(testState);

            int expectedHoney = this.ExpectedHoneyFromOneBeePerHour 
                * testState.GuardsCount;
                
            Assert.AreEqual(expectedHoney, beehive.HoneyCount);
        }   

        /// <summary>
        /// Проверить, не изменяется ли количество пчёл при работе улья без маток.
        /// </summary>
        [TestMethod]
        public void MathBeehive_BeesCountStable()
        {
            ServiceLocator.Instance.RegisterService<IApiaryBalance>(new DefaultApiaryBalance());

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
                    (int)this.Balance.WorkerBalance.TimeToHarvestHoney.TotalMilliseconds
                    + (int)this.Balance.WorkerBalance.TimeToRestInBeehive.TotalMilliseconds
                    + (int)this.Balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds;

                double result = (double)this.MillisecondsInHour / timeForOnePortionMs;

                return (int)(result - (result * MathBeehiveTests.Inaccuracy));
            }
        }

        /// <summary>
        /// Стандартный баланс пасеки.
        /// </summary>
        private IApiaryBalance Balance
            => ServiceLocator.Instance.GetService<IApiaryBalance>();

        /// <summary>
        /// Ожидаемое количество пчёл, произведенное одной маткой в час.
        /// </summary>
        /// <returns>Ожидаемое количество пчёл (согласно балансу).</returns>
        private int ExpectedChildrenFromOneQueenPerHour
        {
            get
            {
                int timeForOneChild = 
                    (int)this.Balance.QueenBalance.TimeToProduceBee.TotalMilliseconds;

                double result = (double)this.MillisecondsInHour / timeForOneChild;

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
                    (int)this.Balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds;

                double result = (double)this.MillisecondsInHour / timeToCheckOneBee;

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
            MathBeehive beehive = new MathBeehive(state);

            for (int i = 0; i < this.GetIterationsInHour(beehive); i++)
            {
                beehive.SingleIteration();
            }

            beehive.Stop();

            return beehive;
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