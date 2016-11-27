namespace Apiary.BeeWorkflowApiary
{
    /// <summary>
    /// Очередь поста охраны улья (потокобезопасная).
    /// </summary>
    internal class GuardPostQueue
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
        internal IBee Dequeue()
        {
            IBee bee;
            this.concurrentQueue.TryDequeue(out bee);
            return bee;
        }

        /// <summary>
        /// Поставить пчелу в очередь на пост охраны.
        /// </summary>
        /// <param name="bee">Пчела, прилетевшая на пост охраны.</param>
        internal Enqueue(IBee bee)
        {
            this.concurrentQueue.Enqueue(bee);
        }
    }
}