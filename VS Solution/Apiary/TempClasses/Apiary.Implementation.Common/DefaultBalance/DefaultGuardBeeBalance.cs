namespace Apiary.Implementation.Common.DefaultBalance
{
    using System;

    /// <summary>
    /// Стандартный баланс пчёл-охранников.
    /// </summary>
    public class DefaultGuardBeeBalance : IGuardBeeBalance
    {
        /// <summary>
        /// Время проверки одной пчелы.
        /// </summary>
        /// <returns>Время проверки одной пчелы.</returns>
        public TimeSpan TimeToCheckOneBee => TimeSpan.FromMilliseconds(100);
    }
}