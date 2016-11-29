namespace Apiary.Tests.UnitTests.BeeWorkflowApiary
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.BeeWorkflowApiary.BeeActions;
    using Apiary.BeeWorkflowApiary.BeeRequests;
    using Apiary.BeeWorkflowApiary.Bees;
    using Apiary.BeeWorkflowApiary.Interfaces;
    using Apiary.Implementation.Common;
    using Apiary.Interfaces.Balancing;
    using Apiary.Tests.TestDoubles.Balances;
    using Apiary.Utilities;
    
    /// <summary>
    /// Класс тестирования рабочих пчёл.
    /// </summary>
    [TestClass]
    public class WorkerBeeTests
    {
        /// <summary>
        /// Баланс пасеки.
        /// </summary>
        private readonly IApiaryBalance apiaryBalance;

        /// <summary>
        /// Общая настройка тестов рабочей пчелы.
        /// </summary>
        public WorkerBeeTests()
        {
            this.apiaryBalance = new FastApiaryBalance(new DefaultApiaryBalance());

            ServiceLocator.Instance.RegisterService<IWorkerBeeBalance>(this.apiaryBalance.WorkerBalance);
            ServiceLocator.Instance.RegisterService<IGuardBeeBalance>(this.apiaryBalance.GuardBalance);
            ServiceLocator.Instance.RegisterService<ILongOperationSimulator>(new LongOperationSimulator());
            ServiceLocator.Instance.RegisterService<IApiaryBalance>(this.apiaryBalance);
        }

        /// <summary>
        /// Пчела нормально собирает одну порцию мёда.
        /// </summary>
        [TestMethod]
        public void WorkerBee_GetSingleHoney()
        {
            WorkerBee bee = new WorkerBee(BeeWorkingState.OnTheRest);

            int honey = 0;

            DateTime now = DateTime.Now;
            List<TimeSpan> spans = new List<TimeSpan>();

            bee.ActionPerformed += (sender, args) =>
            {
                if (args.ActionType == BeeActionType.EnterBeehiveWithHoney)
                {
                    spans.Add(DateTime.Now - now);
                    now = DateTime.Now;

                    Interlocked.Increment(ref honey);
                }
            };

            bee.RequestForBeehiveData += (sender, args) =>
            {
                if (args.RequestType == BeeRequestType.RequestToEnterBeehive)
                {
                    args.Succeed = true;
                }
            };

            TimeSpan timeToHarvestOneBee = this.apiaryBalance.WorkerBalance.TimeToHarvestHoney
                + this.apiaryBalance.WorkerBalance.TimeToRestInBeehive
                + this.apiaryBalance.GuardBalance.TimeToCheckOneBee;

            bee.StartWork();
            
            Task.Delay((int)timeToHarvestOneBee.TotalMilliseconds * 5)
                .GetAwaiter().GetResult();

            Assert.AreEqual(5, honey);
            bee.StopWork();
        }
    }
}