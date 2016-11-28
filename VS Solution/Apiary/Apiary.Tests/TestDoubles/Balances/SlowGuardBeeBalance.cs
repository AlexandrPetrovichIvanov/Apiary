namespace Apiary.Tests.TestDoubles.Balances
{
    using System;

    using Apiary.Interfaces.Balancing;

    /// <summary>
    /// Баланс работы пчёл-охранников, работающих очень медленно.
    /// </summary>
    internal class SlowGuardBeeBalance : IGuardBeeBalance
    {
        /// <summary>
        /// Время проверки одной пчелы.
        /// </summary>
        /// <returns>Время проверки одной пчелы.</returns>
        public TimeSpan TimeToCheckOneBee => TimeSpan.FromMilliseconds(10000);
    }
}