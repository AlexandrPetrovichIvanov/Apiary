namespace Apiary.Interfaces.Balancing
{
    using System;

    /// <summary>
    /// Баланс пчёл-охранников.
    /// </summary>
    public interface IGuardBeeBalance
    {
        /// <summary>
        /// Время проверки одной пчелы.
        /// </summary>
        /// <returns>Время проверки одной пчелы.</returns>
        TimeSpan TimeToCheckOneBee { get; }
    }
}