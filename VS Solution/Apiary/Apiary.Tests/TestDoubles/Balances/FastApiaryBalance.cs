namespace Apiary.Tests.TestDoubles.Balances
{
    /// <summary>
    /// Баланс пасеки, где операции происходят в разы быстрее.
    /// </summary>
    internal class FastApiaryBalance : IApiaryBalance
    {
        /// <summary>
        /// Базовый (исходный) баланс.
        /// </summary>
        private readonly IApiaryBalance baseBalance;

        /// <summary>
        /// Создать экземпляр баланса.
        /// </summary>
        /// <param name="baseBalance">Базовый (исходный) баланс.</param>
        public FastApiaryBalance(IApiaryBalance baseBalance)
        {
            this.baseBalance = baseBalance;
        }

        /// <summary>
        /// Баланс пчёл-рабочих.
        /// </summary>
        /// <returns>Баланс пчёл-рабочих.</returns>
        public IWorkerBeeBalance WorkerBalance 
            => new FastWorkerBalance(this.baseBalance.WorkerBalance);

        /// <summary>
        /// Баланс пчёл-охранников.
        /// </summary>
        /// <returns>Баланс пчёл-охранников.</returns>
        public IGuardBeeBalance GuardBalance 
            => new FastGuardBalance(this.baseBalance.GuardBalance);

        /// <summary>
        /// Баланс пчёл-маток.
        /// </summary>
        /// <returns>Баланс пчёл-маток.</returns>
        public IQueenBeeBalance QueenBalance { get; }
            = new FastQueenBalance(this.baseBalance.QueenBalance);
    }
}