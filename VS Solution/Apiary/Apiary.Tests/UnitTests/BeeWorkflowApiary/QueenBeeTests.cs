namespace Apiary.Tests.UnitTests.BeeWorkflowApiary
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.BeeWorkflowApiary;
    using Apiary.BeeWorkflowApiary.BeeActions;
    using Apiary.BeeWorkflowApiary.Bees;
    using Apiary.BeeWorkflowApiary.Interfaces;
    using Apiary.Implementation.Common;
    using Apiary.Interfaces.Balancing;
    using Apiary.Tests.TestDoubles.Balances;
    using Apiary.Utilities;
    
    /// <summary>
    /// Класс тестирования пчёл-маток.
    /// </summary>
    [TestClass]
    public class QueenBeeTests
    {
        /// <summary>
        /// Баланс быстрой пчелы-матки.
        /// </summary>
        private readonly IQueenBeeBalance queenBalance;

        /// <summary>
        /// Общая настройка тестов пчелы-матки.
        /// </summary>
        public QueenBeeTests()
        {
            FastApiaryBalance balanceFast = new FastApiaryBalance(new DefaultApiaryBalance());
            this.queenBalance = balanceFast.QueenBalance;

            ServiceLocator.Instance.RegisterService<IApiaryBalance>(balanceFast);
            ServiceLocator.Instance.RegisterService<IRandomizer>(new Randomizer());
            ServiceLocator.Instance.RegisterService<IBeeFactory>(new DefaultBeeFactory());
        }

        /// <summary>
        /// Матка не производит пчёл раньше времени.
        /// </summary>
        [TestMethod]
        public void QueenBee_DontProduceBeeBeforeTheTime()
        {
            QueenBee bee = new QueenBee();

            int children = 0;

            bee.ActionPerformed += (sender, args) =>
            {
                if (args.ActionType == BeeActionType.ProduceBee)
                {
                    Interlocked.Increment(ref children);
                }
            };

            bee.RequestForBeehiveData += (sender, args) => { };

            bee.StartWork();

            Task.Delay((int)(this.queenBalance.TimeToProduceBee.TotalMilliseconds / 2))
                .GetAwaiter().GetResult();

            Assert.AreEqual(0, children);

            bee.StopWork();
        }

        /// <summary>
        /// Нормально производит пчёл.
        /// </summary>
        [TestMethod]
        public void QueenBee_ProduceBees()
        {
            QueenBee bee = new QueenBee();

            int children = 0;

            bee.ActionPerformed += (sender, args) =>
            {
                if (args.ActionType == BeeActionType.ProduceBee)
                {
                    Interlocked.Increment(ref children);
                }
            };

            bee.RequestForBeehiveData += (sender, args) => { };

            bee.StartWork();

            Task.Delay((int)this.queenBalance.TimeToProduceBee.TotalMilliseconds * 10).GetAwaiter().GetResult();

            bee.StopWork();

            Assert.AreEqual(10, children);
        }
    }
}