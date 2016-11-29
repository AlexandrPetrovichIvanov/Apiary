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
        private const double Inaccuracy = 0.000;

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
                BeesTotalCount = 30,
                BeesInsideCount = 30,
                WorkerBeesCount = 10,
                QueensCount = 0,
                GuardsCount = 20
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
                BeesTotalCount = 10,
                BeesInsideCount = 10,
                WorkerBeesCount = 0,
                QueensCount = 10,
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
                BeesTotalCount = 100,
                BeesInsideCount = 100,
                WorkerBeesCount = 99,
                QueensCount = 0,
                GuardsCount = 1
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
                BeesTotalCount = 100,
                BeesInsideCount = 100,
                WorkerBeesCount = 1,
                QueensCount = 0,
                GuardsCount = 99
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
                BeesTotalCount = 100,
                BeesInsideCount = 100,
                WorkerBeesCount = 90,
                QueensCount = 0,
                GuardsCount = 10
            };

            IBeehiveState newState = this.LaunchBeehiveForFiveSeconds(initialState);

            Assert.AreEqual(100, newState.BeesTotalCount);
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
                    (int)this.balance.WorkerBalance.TimeToHarvestHoney.TotalMilliseconds
                    + (int)this.balance.WorkerBalance.TimeToRestInBeehive.TotalMilliseconds
                    + (int)this.balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds;

                return (int)(5000 / timeForOnePortionMs);
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

                return (int)(5000 / timeForOneChild + 1); // при завершении работы пчела заканчивает производство пчелы.
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

                return (int)(5000 / timeToCheckOneBee);
            }
        }
    }
}