namespace Apiary.Tests.Concurrency
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    /// <summary>
    /// Тестирование создания и одновременного выполнения миллионов задач.
    /// </summary>
    [TestClass]
    public class MillionTasksTest
    {
        /// <summary>
        /// Количество одновременных задач.
        /// </summary>
        private const int TasksNumber = 1000000;

        /// <summary>
        /// Количество повторений в одной задаче.
        /// </summary>
        private const int IterationsInsideSingleTask = 20;

        /// <summary>
        /// Счетчик выполнения задач.
        /// </summary>
        private long counter;

        /// <summary>
        /// Создание и одновременное выполнение миллионов задач.
        /// </summary>
        [TestMethod]
        public void CreateMillionTasks()
        {
            Task[] tasks = new Task[TasksNumber];

            for (int i = 0; i < TasksNumber; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < IterationsInsideSingleTask; j++)
                    {
                        Interlocked.Add(ref this.counter, 1);
                    }
                });
            }

            Task.Delay(3000).Wait(TimeSpan.FromMilliseconds(3500));

            Assert.IsTrue(this.counter == TasksNumber * (long)IterationsInsideSingleTask);
            tasks.ToList().ForEach(task => Assert.IsTrue(task.IsCompleted));
        }
    }
}
