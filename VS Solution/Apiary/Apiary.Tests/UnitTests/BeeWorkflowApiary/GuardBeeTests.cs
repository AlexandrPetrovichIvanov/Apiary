namespace Apiary.Tests.UnitTests.BeeWorkflowApiary
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Apiary.BeeWorkflowApiary;
    using Apiary.BeeWorkflowApiary.BeeActions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.BeeWorkflowApiary.BeeRequests;
    using Apiary.BeeWorkflowApiary.Bees;
    using Apiary.Implementation.Common;
    using Apiary.Interfaces.Balancing;
    using Apiary.Tests.TestDoubles.Balances;
    using Apiary.Tests.TestDoubles.Bees;
    using Apiary.Utilities;
    
    /// <summary>
    /// Класс тестирования пчёл-охранников.
    /// </summary>
    [TestClass]
    public class GuardBeeTests
    {
        /// <summary>
        /// Допустимая погрешность.
        /// </summary>
        private const double Inaccuracy = 0.1;

        /// <summary>
        /// Баланс пасеки.
        /// </summary>
        private FastApiaryBalance balance = new FastApiaryBalance(new DefaultApiaryBalance());

        /// <summary>
        /// Общие настройки тестов класса.
        /// </summary>
        public GuardBeeTests()
        {
            ServiceLocator.Instance.RegisterService<IApiaryBalance>(balance);
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

        /// <summary>
        /// Охранник совершает несколько проверок за то время,
        /// за которое и должен.
        /// </summary>
        [TestMethod]
        public void GuardBee_WorkExpectedTime()
        {
            GuardBee bee = new GuardBee();

            GuardPostQueue queue = new GuardPostQueue();

            for (int i = 0; i < 1000; i++)
            {
                queue.Enqueue(new EmptyBaseBee());
            }

            int operationsCount = 0;

            bee.ActionPerformed += (sender, args) =>
            {
                if (args.ActionType == BeeActionType.AcceptBeeToEnter)
                {
                    Interlocked.Increment(ref operationsCount);
                }
            };

            bee.RequestForBeehiveData += (sender, args) =>
            {
                if (args.RequestType == BeeRequestType.RequestGuardPostQueue)
                {
                    args.Response = queue;
                }

                args.Succeed = true;
            };

            bee.StartWork();

            Task.Delay((int)this.balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds * 50)
                .GetAwaiter().GetResult();

            bee.StopWork();

            Assert.IsTrue(Math.Abs(50 - operationsCount) <= (50 * GuardBeeTests.Inaccuracy));
        }
    }
}