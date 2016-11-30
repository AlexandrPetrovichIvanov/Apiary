namespace Apiary.BeeWorkflowApiary
{
    using System.Collections.Concurrent;

    using Apiary.BeeWorkflowApiary.Interfaces;

    /// <summary>
    /// Очередь поста охраны улья (потокобезопасная).
    /// </summary>
    public class GuardPostQueue
    {
        /// <summary>
        /// Потокобезопасная очередь.
        /// </summary>
        private readonly ConcurrentQueue<IBee> concurrentQueue
            = new ConcurrentQueue<IBee>();

        /// <summary>
        /// Принять следующую в очереди пчелу.
        /// </summary>
        /// <returns>Пчела (в порядке очереди) или null, если очередь пуста.</returns>
        public IBee Dequeue()
        {
            IBee bee;
            this.concurrentQueue.TryDequeue(out bee);
            return bee;
        }

        /// <summary>
        /// Поставить пчелу в очередь на пост охраны.
        /// </summary>
        /// <param name="bee">Пчела, прилетевшая на пост охраны.</param>
        public void Enqueue(IBee bee)
        {
            this.concurrentQueue.Enqueue(bee);
        }
    }
}