namespace Apiary.Tests.TestDoubles.Balances
{
    /// <summary>
    /// Баланс пасеки с медленными охранниками.
    /// </summary>
    internal class SlowGuardsApiaryBalance : IApiaryBalance
    {
        /// <summary>
        /// Базовый (исходный) баланс.
        /// </summary>
        private readonly IApiaryBalance baseBalance;

        /// <summary>
        /// Создать экземпляр баланса.
        /// </summary>
        /// <param name="baseBalance">Базовый (исходный) баланс.</param>
        public SlowGuardsApiaryBalance(IApiaryBalance baseBalance)
        {
            this.baseBalance = baseBalance;
        }

        /// <summary>
        /// Баланс пчёл-рабочих.
        /// </summary>
        /// <returns>Баланс пчёл-рабочих.</returns>
        public IWorkerBeeBalance WorkerBalance 
            => this.baseBalance.WorkerBalance;

        /// <summary>
        /// Баланс пчёл-охранников.
        /// </summary>
        /// <returns>Баланс пчёл-охранников.</returns>
        public IGuardBeeBalance GuardBalance 
            => new SlowGuardBeeBalance();

        /// <summary>
        /// Баланс пчёл-маток.
        /// </summary>
        /// <returns>Баланс пчёл-маток.</returns>
        public IQueenBeeBalance QueenBalance { get; }
            = this.baseBalance.QueenBalance;
    }
}