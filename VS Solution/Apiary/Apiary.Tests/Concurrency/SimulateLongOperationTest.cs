namespace Apiary.Tests.Concurrency
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Windows.System.Threading;

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
        private const int BeesCount = 15000;

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
                bees.Add(bee);
            }

            bees.ForEach(bee => bee.StartWork());

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
            /// Сколько мёда должна собрать одна пчела.
            /// </summary>
            internal const int HoneyFromSingleBee = 50; 
            // 100 - для проверки длительной стабильной работы больше полутора минут

            /// <summary>
            /// Продолжительность сбора одной порции мёда в 
            /// миллисекундах.
            /// </summary>
            internal const int HarvestHoneyDurationMs = 50;
            // для нормальной проверки - 1000

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
                // 10 000 пчёл при 1000мс на сбор единицы мёда
                //Task.Factory.StartNew(this.HarvestWithDelay);

                // 300 000 пчёл при 1000мс на сбор единицы мёда
                //Task.Factory.StartNew(this.HarvestWithTimer);

                // 700 000 пчёл при 1000мс на сбор единицы мёда
                //Task.Factory.StartNew(this.HarvestWithSimulator);

                // 500 000 пчёл при 1000мс на сбор единицы мёда
                // 200 000 пчёл при  100мс на сбор единицы мёда
                Task.Factory.StartNew(this.HarvestWithUtilitiesSimulator);
            }

            #region UsingDelay
            
            /// <summary>
            /// Сбор мёда. Cтандартная симуляция с помощью Task.Delay.
            /// максимальное количество пчёл при этом - 20000.
            /// </summary>
            private async void HarvestWithDelay()
            {
                for (int i = 0; i < HoneyFromSingleBee; i++)
                {
                    await Task.Delay(HarvestHoneyDurationMs);

                    this.OneHoneyHarvested?.Invoke(
                        this,
                        EventArgs.Empty);
                }
            }

            #endregion

            #region UsingTimer

            /// <summary>
            /// Собрать мёд с использованием таймера.
            /// </summary>
            private void HarvestWithTimer()
            {
                ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(
                    this.TimerElapsed,
                    TimeSpan.FromMilliseconds(HarvestHoneyDurationMs));
            }

            /// <summary>
            /// Одна итерация таймера.
            /// </summary>
            /// <param name="timer">Таймер.</param>
            private void TimerElapsed(ThreadPoolTimer timer)
            {
                if (this.currentHoney == HoneyFromSingleBee)
                {
                    timer.Cancel();
                    return;
                }

                Interlocked.Add(ref this.currentHoney, 1);
                this.OneHoneyHarvested?.Invoke(this, EventArgs.Empty);
            }

            #endregion

            #region Using Test Simulator

            /// <summary>
            /// Общий имитатор выполнения длительных операций.
            /// </summary>
            private static readonly LongOperationSimulator simulator
                = new LongOperationSimulator(BeeTestDouble.HarvestHoneyDurationMs);

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
                    this.EndHarvestSinglePortion,
                    this.HarvestWithSimulator);
            }

            /// <summary>
            /// Окончание сбора одной порции мёда.
            /// </summary>
            private void EndHarvestSinglePortion()
            {
                if (this.currentHoney == HoneyFromSingleBee)
                {
                    return;
                }

                this.OneHoneyHarvested?.Invoke(this, EventArgs.Empty);
                Interlocked.Add(ref this.currentHoney, 1);
            }

            #endregion

            #region Using LongOperationSumulator from Utilities

            /// <summary>
            /// Общий имитатор выполнения длительных операций.
            /// </summary>
            private static readonly Apiary.Utilities.LongOperationSimulator utilitiesSimulator
                = new Apiary.Utilities.LongOperationSimulator();

            /// <summary>
            /// Сбор мёда. Симуляция с помощью специального класса.
            /// </summary>
            private void HarvestWithUtilitiesSimulator()
            {
                BeeTestDouble.utilitiesSimulator.SimulateAsync(
                    TimeSpan.FromMilliseconds(HarvestHoneyDurationMs),
                    this.HarvestWithUtilitiesSimulatorRecursive);
            }

            /// <summary>
            /// Сбор мёда. Симуляция с помощью специального класса.
            /// </summary>
            private void HarvestWithUtilitiesSimulatorRecursive()
            {
                if (this.currentHoney == HoneyFromSingleBee)
                {
                    return;
                }

                this.OneHoneyHarvested?.Invoke(this, EventArgs.Empty);
                Interlocked.Add(ref this.currentHoney, 1);
                
                BeeTestDouble.utilitiesSimulator.SimulateAsync(
                    TimeSpan.FromMilliseconds(HarvestHoneyDurationMs),
                    this.HarvestWithUtilitiesSimulatorRecursive);
            }

            #endregion
        }

        /// <summary>
        /// Класс имитации длительных операций.
        /// </summary>
        private class LongOperationSimulator
        {
            /// <summary>
            /// Очередь действий для выполнения.
            /// </summary>
            /// <remarks>Именно ОЧЕРЕДЬ - первый пришел - первым обслужен!
            /// Чтобы пчелы, пришедшие раньше, получали мёд раньше.</remarks>
            private ConcurrentQueue<KeyValuePair<Action, Action>> queue
                = new ConcurrentQueue<KeyValuePair<Action, Action>>();

            /// <summary>
            /// Создать имитатор длительных операций.
            /// </summary>
            /// <param name="delayTimeMs">Время выполнения операции.</param>
            internal LongOperationSimulator(int delayTimeMs)
            {
                ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(
                    this.TimerElapsed,
                    TimeSpan.FromMilliseconds(delayTimeMs));
            }

            /// <summary>
            /// Имитировать длительную операцию.
            /// </summary>
            /// <param name="endWith">Действие при завершении операции.</param>
            /// <param name="continueWith">Действие после завершения операции.</param>
            internal void SimulateAsync(
                Action endWith,
                Action continueWith)
            {
                this.queue.Enqueue(new KeyValuePair<Action, Action>(continueWith, endWith));
            }

            /// <summary>
            /// Выполнение действий в одной итерации таймера.
            /// </summary>
            /// <param name="timer"></param>
            private void TimerElapsed(ThreadPoolTimer timer)
            {
                int currentCount = this.queue.Count;

                for (int i = 0; i < currentCount; i++)
                {
                    KeyValuePair<Action, Action> currentAction;

                    bool success = this.queue.TryDequeue(out currentAction);

                    if (success)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            currentAction.Key();
                            currentAction.Value();
                        });
                    }
                }
            }
        }
    }
}
