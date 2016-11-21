namespace Apiary.Interfaces.Balancing
{
    using System;

    /// <summary>
    /// Баланс пчёл-охранников.
    /// </summary>
    public class GuardBeeBalance
    {
        /// <summary>
        /// Время проверки одной пчелы.
        /// </summary>
        /// <returns>Время проверки одной пчелы.</returns>
        public TimeSpan TimeToCheckOneBee { get; set; }
    }
}