namespace Apiary.Tests.Temp
{
    using System.Threading;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    /// <summary>
    /// Тест инкрементирования поля из разных потоков.
    /// </summary>
    [TestClass]
    public class ConcurrencyIncrementingTest
    {
        /// <summary>
        /// Число, которое будет изменяться из разных потоков
        /// одновременно.
        /// </summary>
        private int threadUnsafeValue;

        /// <summary>
        /// Проверка, не будет ли ошибок при инкрементировании
        /// значения числового поля из разных потоков.
        /// </summary>
        [TestMethod]
        public void ConcurrencyIncrementing()
        {
            List<Task> tasks = new List<Task>();

            int tasksCount = 10000;

            for (int i = 0; i < tasksCount; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    Interlocked.Add(ref this.threadUnsafeValue, 1);
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Assert.AreEqual(tasksCount, this.threadUnsafeValue);
        }
    }
}
