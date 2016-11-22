namespace Apiary.Tests.UnitTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.Utilities;

    /// <summary>
    /// Тестирование имитатора длительных операций.
    /// </summary>
    [TestClass]
    public class LongOperationsSimulatorTests
    {
        /// <summary>
        /// Общее количество одновременных операций.
        /// </summary>
        private const int OperationsCount = 1000;

        /// <summary>
        /// Общая погрешность в миллисекундах.
        /// </summary>
        private const int InaccuracyMs = 100;

        /// <summary>
        /// Симулятор выполнения длительных операций.
        /// </summary>
        private readonly ILongOperationSimulator simulator 
            = new LongOperationSimulator();

        /// <summary>
        /// Длительность выполнения одной операции.
        /// </summary>
        private readonly TimeSpan duration = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Счётчик выполнения операций.
        /// </summary>
        private int counter;

        /// <summary>
        /// Имитатор нормально выполняет операции за заданное время.
        /// </summary>
        [TestMethod]
        public void LongOperationsSimulator_PerformOperations()
        {
            this.counter = 0;

            for (int i = 0; i < LongOperationsSimulatorTests.OperationsCount; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    this.simulator.SimulateAsync(
                        this.duration,
                        this.IncrementCounter);
                });
            }

            int delayTime = (int)this.duration.TotalMilliseconds
                + LongOperationsSimulatorTests.InaccuracyMs;

            Task.Delay(delayTime).Wait();

            Assert.AreEqual(LongOperationsSimulatorTests.OperationsCount, this.counter);
        }

        /// <summary>
        /// Имитатор не выполняет операций быстрее, чем заданное время.
        /// </summary>
        [TestMethod]
        public void LongOperationsSimulator_DontPerformFasterThenNeed()
        {
            this.counter = 0;

            Task.Factory.StartNew(() =>
            {
                this.simulator.SimulateAsync(
                    this.duration,
                    this.IncrementCounter);
            });

            Task.Delay(100).Wait();
            Assert.AreEqual(0, this.counter);
            Assert.IsFalse(this.simulator.HasError);
        }

        /// <summary>
        /// Симуляция длительных операций без продолжающего действия.
        /// </summary>
        [TestMethod]
        public void LongOperationsSimulator_EmptyEndActions()
        {
            // ошибка вы
            Task.Factory.StartNew(() =>
            {
                this.simulator.SimulateAsync(
                    TimeSpan.FromMilliseconds(100), 
                    null);
            });

            Task.Delay(200).Wait();
            Assert.IsFalse(this.simulator.HasError);
        }

        /// <summary>
        /// Увеличение счётчика операций на 1.
        /// </summary>
        private void IncrementCounter()
        {
            Interlocked.Increment(ref this.counter);
        }
    }
}
