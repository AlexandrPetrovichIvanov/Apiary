namespace Apiary.Implementation.Common.DefaultBalance
{
    /// <summary>
    /// Стандартный баланс пчёл-маток.
    /// </summary>
    public class DefaultQueenBalance : IQueenBeeBalance
    {
        /// <summary>
        /// Время производства одной пчелы.
        /// </summary>
        /// <returns>Время производства одной пчелы.</returns>
        public TimeSpan TimeToProduceBee => TimeSpan.FromMilliseconds(3000);

        /// <summary>
        /// Тысячная доля вероятности произвести рабочую пчелу.
        /// </summary>
        /// <returns>Вероятность произвести рабочую пчелу.</returns>
        public int ThousandthPartToProduceWorker => 900;

        /// <summary>
        /// Тысячная доля вероятности произвести охранника.
        /// </summary>
        /// <returns>Вероятность произвести охранника.</returns>
        public int ThousandthPartToProduceGuard => 99;

        /// <summary>
        /// Тысячная доля вероятности произвести пчелу-матку.
        /// </summary>
        /// <returns>Вероятность произвести пчелу-матку.</returns>
        public int ThousandthPartToProduceQueen => 1;
    }
}