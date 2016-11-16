using System.Threading;
using Windows.System.Threading;

namespace Apiary.Tests.Concurrency
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    /// <summary>
    /// Тесты различных вопросов многопоточности.
    /// </summary>
    [TestClass]
    public class ConcurrencyTests
    {
        /// <summary>
        /// Число, которое будет изменяться из разных потоков
        /// одновременно.
        /// </summary>
        private int threadUnsafeValue = 0;

        /// <summary>
        /// Проверка, будет ли нормально параллельно отрабатывать
        /// Task.Delay, если вызывается сразу из многих потоков.
        /// </summary>
        [TestMethod]
        public void DelayInMultipleTasks()
        {
            List<Task> tasks = new List<Task>();
            CancellationToken token = new CancellationToken();
            
            DateTime startTime = DateTime.Now;

            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    Timer timer = new Timer(
                        (state => { Interlocked.Add(ref this.threadUnsafeValue, 1); }), 
                        null, 
                        TimeSpan.Zero, 
                        TimeSpan.FromSeconds(1));

                    

                    //Task.Delay(1000).Wait();
                    //token.WaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                }, token));
            }
            
            Task.WaitAll(tasks.ToArray());

            DateTime endTime = DateTime.Now;

            double periodMilliseconds = 
                (endTime - startTime).TotalMilliseconds;
            int inaccuracyMilliseconds = 500;
            
            Assert.IsTrue(periodMilliseconds > 999);
            Assert.IsTrue(periodMilliseconds < (1000 + inaccuracyMilliseconds));
        }

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
