namespace Apiary.Tests.UnitTests.BeeWorkflowApiary
{
    using System.Threading;
    using System.Threading.Tasks;
    using Apiary.BeeWorkflowApiary.BeeRequests;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.BeeWorkflowApiary.Bees;
    using Apiary.Implementation.Common;
    using Apiary.Interfaces.Balancing;
    using Apiary.Tests.TestDoubles.Balances;
    using Apiary.Utilities;
    
    /// <summary>
    /// Класс тестирования пчёл-охранников.
    /// </summary>
    [TestClass]
    public class GuardBeeTests
    {
        /// <summary>
        /// Общие настройки тестов класса.
        /// </summary>
        public GuardBeeTests()
        {
            ServiceLocator.Instance.RegisterService<ILongOperationSimulator>(new LongOperationSimulator());
            ServiceLocator.Instance.RegisterService<IGuardBeeBalance>(
                new FastGuardBeeBalance(new DefaultGuardBeeBalance()));
        }

        /// <summary>
        /// Охранник не работает без очереди ожидания поста охраны.
        /// </summary>
        [TestMethod]
        public void GuardBee_DontWorkWithoutGuardPostQueue()
        {
            GuardBee bee = new GuardBee();

            int operationsCount = 0;

            bee.ActionPerformed += (sender, args) =>
            {
                Interlocked.Increment(ref operationsCount);
            };

            bee.RequestForBeehiveData += (sender, args) =>
            {
            };

            bee.StartWork();
            Task.Delay(200).GetAwaiter().GetResult();
            bee.StopWork();

            Assert.AreEqual(0, operationsCount);
        }
    }
}