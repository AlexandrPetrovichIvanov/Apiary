namespace Apiary.Implementation.Common.DefaultBalance
{
    /// <summary>
    /// Баланс работы пчёл-охранников, работающих в 2 раза быстрее.
    /// </summary>
    internal class FastGuardBeeBalance : IGuardBeeBalance
    {
        /// <summary>
        /// Базовый (исходный) баланс.
        /// </summary>
        private readonly IGuardBeeBalance baseBalance;

        /// <summary>
        /// Создать баланс работы охранников, проверяющих в 2 раза быстрее.
        /// </summary>
        /// <param name="baseBalance">Исходный баланс.</param>
        public FastGuardBeeBalance(
            IGuardBeeBalance baseBalance)
        {
            this.baseBalance = baseBalance;
        }
        
        /// <summary>
        /// Время проверки одной пчелы.
        /// </summary>
        /// <returns>Время проверки одной пчелы.</returns>
        public TimeSpan TimeToCheckOneBee => this.baseBalance.TimeToCheckOneBee / 2;
    }
}