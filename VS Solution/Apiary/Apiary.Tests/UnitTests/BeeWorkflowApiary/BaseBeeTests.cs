namespace Apiary.Tests.UnitTests.BeeWorkflowApiary
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.BeeWorkflowApiary.Bees;
    using Apiary.Tests.TestDoubles.Bees;
    using Apiary.Utilities;

    /// <summary>
    /// Класс тестирования базового класса пчёл.
    /// </summary>
    [TestClass]
    public class BaseBeeTests
    {
        [TestMethod]
        public void BaseBee_NormalWork()
        {
            BeeBase bee = new EmptyBaseBee();

            int operationsCount = 0;

            bee.ActionPerformed += (sender, args) =>
            {
                Interlocked.Increment(ref operationsCount);
            };

            bee.RequestForBeehiveData += (sender, args) => { };

            bee.StartWork();

            Task.Delay(EmptyBaseBee.IntervalMs*20).GetAwaiter().GetResult();

            bee.StopWork();

            Assert.AreEqual(20, operationsCount);
        }

        /// <summary>
        /// Пчела не может начать работу, если не связана с ульем.
        /// </summary>
        [TestMethod]
        public void BaseBee_CantStartWithoutBeehive()
        {
            BeeBase bee = new EmptyBaseBee();

            try
            {
                bee.StartWork();
                throw new AssertFailedException(
                    "Пчела, не связанная с ульем, не должна была начать работу.");
            }
            catch (InvalidOperationException)
            {
                // так и должно быть
            }
        }

        /// <summary>
        /// Пчела больше не посылает сообщений, если работа пчелы остановлена.
        /// </summary>
        [TestMethod]
        public void BaseBee_DontSendMessagesWhenStopped()
        {
            BeeBase bee = new EmptyBaseBee();

            int operationsCount = 0;

            bee.ActionPerformed += (sender, args) =>
            {
                Interlocked.Increment(ref operationsCount);
            };

            bee.RequestForBeehiveData += (sender, args) =>
            {
                Interlocked.Increment(ref operationsCount);
            };

            bee.StartWork();
            bee.StopWork();

            int expectedOperationsCount = operationsCount;

            Task.Delay(200).GetAwaiter().GetResult();

            Assert.AreEqual(expectedOperationsCount, operationsCount);
        }
    }
}