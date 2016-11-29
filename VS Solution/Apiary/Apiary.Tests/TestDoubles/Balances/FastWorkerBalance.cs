namespace Apiary.Tests.TestDoubles.Balances
{
    using System;
    using Apiary.Interfaces.Balancing;

    /// <summary>
    /// Баланс рабочих пчёл, работающих в 20 раз быстрее.
    /// </summary>
    internal class FastWorkerBalance : IWorkerBeeBalance
    {
        /// <summary>
        /// Создать баланс рабочих пчёл, работающих в 20 раз быстрее.
        /// </summary>
        /// <param name="baseBalance">Исходный баланс.</param>
        public FastWorkerBalance(
            IWorkerBeeBalance baseBalance)
        {
            this.TimeToHarvestHoney = TimeSpan.FromMilliseconds(
                baseBalance.TimeToHarvestHoney.TotalMilliseconds/5);
            this.TimeToRestInBeehive = TimeSpan.FromMilliseconds(
                baseBalance.TimeToRestInBeehive.TotalMilliseconds/5);
        }
        
        /// <summary>
        /// Время сбора мёда (снаружи улья).
        /// </summary>
        /// <returns>Время сбора мёда (снаружи улья).</returns>
        public TimeSpan TimeToHarvestHoney { get; }

        /// <summary>
        /// Время отдыха в улье (перерывы между полётами за мёдом).
        /// </summary>
        /// <returns>Время отдыха в улье.</returns>
        public TimeSpan TimeToRestInBeehive { get; }
    }
}