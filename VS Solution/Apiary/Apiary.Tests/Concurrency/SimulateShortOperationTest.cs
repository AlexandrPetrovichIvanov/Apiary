namespace Apiary.Tests.Concurrency
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    /// <summary>
    /// Класс тестирования имитации коротких операций.
    /// </summary>
    [TestClass]
    public class SimulateShortOperationTest
    {
        /// <summary>
        /// Количество одновременных операций.
        /// </summary>
        private const int OperationsCount = 20000;

        /// <summary>
        /// Длительность операции в миллисекундах.
        /// </summary>
        private const int DurationMs = 50;

        /// <summary>
        /// Погрешность в миллисекундах.
        /// </summary>
        private const int InaccuracyMs = 1000;

        /// <summary>
        /// Количество выполненных операций.
        /// </summary>
        private int operationsPerformed;

        /// <summary>
        /// Имитировать множество одновременных коротких операций.
        /// </summary>
        [TestMethod]
        public void SimulateShortOperations()
        {
            for (int i = 0; i < SimulateShortOperationTest.OperationsCount; i++)
            {
                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(SimulateShortOperationTest.DurationMs);
                    Interlocked.Increment(ref this.operationsPerformed);
                });
            }

            Task.Delay(DurationMs + InaccuracyMs).Wait();

            Assert.AreEqual(
                SimulateShortOperationTest.OperationsCount, 
                this.operationsPerformed);
        }
    }
}
