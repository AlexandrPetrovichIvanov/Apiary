namespace Apiary.Implementation.Common.DefaultBalance
{
    /// <summary>
    /// Баланс рабочих пчёл, работающих в 20 раз быстрее.
    /// </summary>
    internal class FastGuardBeeBalance : IGuardBeeBalance
    {
        /// <summary>
        /// Базовый (исходный) баланс.
        /// </summary>
        private readonly IWorkerBeeBalance baseBalance;

        /// <summary>
        /// Создать баланс рабочих пчёл, работающих в 20 раз быстрее.
        /// </summary>
        /// <param name="baseBalance">Исходный баланс.</param>
        public FastGuardBeeBalance(
            IWorkerBeeBalance baseBalance)
        {
            this.baseBalance = baseBalance;
        }
        
        /// <summary>
        /// Время сбора мёда (снаружи улья).
        /// </summary>
        /// <returns>Время сбора мёда (снаружи улья).</returns>
        public TimeSpan TimeToHarvestHoney => this.baseBalance.TimeToHarvestHoney / 20;

        /// <summary>
        /// Время отдыха в улье (перерывы между полётами за мёдом).
        /// </summary>
        /// <returns>Время отдыха в улье.</returns>
        public TimeSpan TimeToRestInBeehive => this.baseBalance.TimeToRestInBeehive / 20;
    }
}