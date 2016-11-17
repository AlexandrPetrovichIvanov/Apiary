namespace Apiary.Tests.Concurrency
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    /// <summary>
    /// Тест корректности имитации длительных операций
    /// (одновременных в множестве потоков/задач)
    /// </summary>
    [TestClass]
    public class SimulateLongOperationTest
    {
        /// <summary>
        /// Количество пчел.
        /// </summary>
        private const int BeesCount = 25000;

        /// <summary>
        /// Погрешность по времени сбора всего мёда всеми
        /// пчелами (в миллисекундах).
        /// </summary>
        private const int InaccuracyMs = 1500;

        /// <summary>
        /// Количество собранного мёда.
        /// </summary>
        private int harvestedHoney = 0;
        
        /// <summary>
        /// Тест корректности имитации длительных операций
        /// (одновременных в множестве потоков/задач)
        /// </summary>
        [TestMethod]
        public void SimulateLongOperations()
        {
            List<BeeTestDouble> bees = new List<BeeTestDouble>(); 

            for (int i = 0; i < BeesCount; i++)
            {
                BeeTestDouble bee = new BeeTestDouble();
                bee.OneHoneyHarvested += this.BeeOnOneHoneyHarvested;
                bee.StartWork();
                bees.Add(bee);
            }

            DateTime startTime = DateTime.Now;

            int delayTime = BeeTestDouble.HoneyFromSingleBee*BeeTestDouble.HarvestHoneyDurationMs
                            + InaccuracyMs;

            Task.Delay(delayTime).Wait();

            int realDelayTime = (int)(DateTime.Now - startTime).TotalMilliseconds;

            Assert.AreEqual(
                BeesCount * BeeTestDouble.HoneyFromSingleBee,
                this.harvestedHoney);

            Assert.IsTrue((realDelayTime - delayTime) <= 1000);

            bees.ForEach(bee => bee.OneHoneyHarvested 
                -= this.BeeOnOneHoneyHarvested);
            bees.Clear();
        }

        /// <summary>
        /// Обработка события завершения сбора одной порции мёда одной пчелой.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="eventArgs">Аргументы события.</param>
        private void BeeOnOneHoneyHarvested(
            object sender, 
            EventArgs eventArgs)
        {
            Interlocked.Add(ref this.harvestedHoney, 1);
        }

        /// <summary>
        /// Простой имитатор пчелы.
        /// </summary>
        private class BeeTestDouble
        {
            /// <summary>
            /// Общий имитатор выполнения длительных операций.
            /// </summary>
            private static readonly LongOperationSimulator simulator 
                = new LongOperationSimulator();

            /// <summary>
            /// Сколько мёда должна собрать одна пчела.
            /// </summary>
            /// <remarks>Не влияет на предельно допустимое 
            /// количество пчёл.</remarks>
            internal const int HoneyFromSingleBee = 5;

            /// <summary>
            /// Продолжительность сбора одной порции мёда в 
            /// миллисекундах.
            /// </summary>
            internal const int HarvestHoneyDurationMs = 1000;

            /// <summary>
            /// Событие сбора одной порции мёда.
            /// </summary>
            internal event EventHandler OneHoneyHarvested;

            /// <summary>
            /// Текущее количество собранного мёда.
            /// </summary>
            private int currentHoney;

            /// <summary>
            /// Начать работу по сбору мёда.
            /// </summary>
            internal void StartWork()
            {
                //Task.Factory.StartNew(this.Harvest);
                Task.Factory.StartNew(this.HarvestWithSimulator);
            }

            /// <summary>
            /// Сбор мёда. Cтандартная симуляция с помощью Task.Delay.
            /// максимальное количество пчёл при этом - 20000.
            /// </summary>
            private async void Harvest()
            {
                for (int i = 0; i < HoneyFromSingleBee; i++)
                {
                    await Task.Delay(HarvestHoneyDurationMs);

                    this.OneHoneyHarvested?.Invoke(
                        this,
                        EventArgs.Empty);
                }
            }

            /// <summary>
            /// Сбор мёда. Симуляция с помощью специального класса.
            /// </summary>
            private void HarvestWithSimulator()
            {
                if (this.currentHoney == HoneyFromSingleBee)
                {
                    return;
                }

                BeeTestDouble.simulator.SimulateAsync(
                    HarvestHoneyDurationMs,
                    this.EndHarvestSinglePortion,
                    this.HarvestWithSimulator);
            }

            /// <summary>
            /// Окончание сбора одной порции мёда.
            /// </summary>
            private void EndHarvestSinglePortion()
            {
                this.OneHoneyHarvested?.Invoke(this, EventArgs.Empty);
                Interlocked.Add(ref this.currentHoney, 1);
            }
        }

        /// <summary>
        /// Класс имитации длительных операций.
        /// </summary>
        private class LongOperationSimulator
        {
            /// <summary>
            /// Имитировать длительную операцию.
            /// </summary>
            /// <param name="delayTimeMs">Продолжительность в миллисекундах.</param>
            /// <param name="endWith">Действие при завершении операции.</param>
            /// <param name="continueWith">Действие после завершения операции.</param>
            internal async void SimulateAsync(
                int delayTimeMs,
                Action endWith,
                Action continueWith)
            {
                await Task.Delay(delayTimeMs);
                endWith();
                continueWith();
            }
        }
    }
}
