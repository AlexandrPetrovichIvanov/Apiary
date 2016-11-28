namespace Apiary.Tests.TestDoubles.Balances
{
    using Apiary.Interfaces.Balancing;

    /// <summary>
    /// Баланс пасеки, где операции происходят в разы быстрее.
    /// </summary>
    internal class FastApiaryBalance : IApiaryBalance
    {
        /// <summary>
        /// Создать экземпляр баланса.
        /// </summary>
        /// <param name="baseBalance">Базовый (исходный) баланс.</param>
        public FastApiaryBalance(IApiaryBalance baseBalance)
        {
            this.WorkerBalance = new FastWorkerBalance(baseBalance.WorkerBalance);
            this.GuardBalance = new FastGuardBeeBalance(baseBalance.GuardBalance);
            this.QueenBalance = new FastQueenBalance(baseBalance.QueenBalance);
        }

        /// <summary>
        /// Баланс пчёл-рабочих.
        /// </summary>
        /// <returns>Баланс пчёл-рабочих.</returns>
        public IWorkerBeeBalance WorkerBalance { get; }

        /// <summary>
        /// Баланс пчёл-охранников.
        /// </summary>
        /// <returns>Баланс пчёл-охранников.</returns>
        public IGuardBeeBalance GuardBalance { get; }

        /// <summary>
        /// Баланс пчёл-маток.
        /// </summary>
        /// <returns>Баланс пчёл-маток.</returns>
        public IQueenBeeBalance QueenBalance { get; }
    }
}