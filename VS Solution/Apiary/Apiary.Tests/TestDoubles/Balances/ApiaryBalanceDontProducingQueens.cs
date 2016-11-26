namespace Apiary.Tests.TestDoubles.Balances
{
    /// <summary>
    /// Баланс пасеки, в котором матки не производят маток.
    /// </summary>
    internal class ApiaryBalanceDontProducingQueens : IApiaryBalance
    {
        /// <summary>
        /// Базовый (исходный) баланс.
        /// </summary>
        private readonly IApiaryBalance baseBalance;

        /// <summary>
        /// Создать экземпляр баланса.
        /// </summary>
        /// <param name="baseBalance">Базовый (исходный) баланс.</param>
        public ApiaryBalanceDontProducingQueens(IApiaryBalance baseBalance)
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
            => this.baseBalance.GuardBalance;

        /// <summary>
        /// Баланс пчёл-маток.
        /// </summary>
        /// <returns>Баланс пчёл-маток.</returns>
        public IQueenBeeBalance QueenBalance { get; }
            = new QueenBalanceDontProducingQueens(this.baseBalance.QueenBalance);
    }
}