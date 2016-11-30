namespace Apiary.Tests.TestDoubles.Balances
{
    using System;
    using Apiary.Interfaces.Balancing;

    /// <summary>
    /// Баланс работы пчёл-охранников, работающих в 2 раза быстрее.
    /// </summary>
    internal class FastGuardBeeBalance : IGuardBeeBalance
    {
        /// <summary>
        /// Создать баланс работы охранников, проверяющих в 2 раза быстрее.
        /// </summary>
        /// <param name="baseBalance">Исходный баланс.</param>
        public FastGuardBeeBalance(
            IGuardBeeBalance baseBalance)
        {
            this.TimeToCheckOneBee = TimeSpan.FromMilliseconds(
                (baseBalance.TimeToCheckOneBee.TotalMilliseconds));
        }
        
        /// <summary>
        /// Время проверки одной пчелы.
        /// </summary>
        /// <returns>Время проверки одной пчелы.</returns>
        public TimeSpan TimeToCheckOneBee { get; }
    }
}