namespace Apiary.Implementation.Common.DefaultBalance
{
    /// <summary>
    /// Баланс работы пчёл-маток, работающих в 40 раз быстрее.
    /// </summary>
    internal class FastQueenBalance : IQueenBeeBalance
    {
        /// <summary>
        /// Базовый (исходный) баланс.
        /// </summary>
        private readonly IQueenBeeBalance baseBalance;

        /// <summary>
        /// Создать баланс работы пчёл-маток, работающих в 40 раз быстрее.
        /// </summary>
        /// <param name="baseBalance">Исходный баланс.</param>
        public QueenBalanceDontProducingQueens(
            IQueenBeeBalance baseBalance)
        {
            this.baseBalance = baseBalance;
        }

        /// <summary>
        /// Время производства одной пчелы.
        /// </summary>
        /// <returns>Время производства одной пчелы.</returns>
        public TimeSpan TimeToProduceBee => baseBalance.TimeToProduceBee / 40;

        /// <summary>
        /// Тысячная доля вероятности произвести рабочую пчелу.
        /// </summary>
        /// <returns>Вероятность произвести рабочую пчелу.</returns>
        public int ThousandthPartToProduceWorker => baseBalance.ThousandthPartToProduceWorker;

        /// <summary>
        /// Тысячная доля вероятности произвести охранника.
        /// </summary>
        /// <returns>Вероятность произвести охранника.</returns>
        public int ThousandthPartToProduceGuard => baseBalance.ThousandthPartToProduceGuard;

        /// <summary>
        /// Тысячная доля вероятности произвести пчелу-матку.
        /// </summary>
        /// <returns>Вероятность произвести пчелу-матку.</returns>
        public int ThousandthPartToProduceQueen => baseBalance.ThousandthPartToProduceQueen;
    }
}