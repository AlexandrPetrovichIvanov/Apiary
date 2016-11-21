namespace Apiary.Interfaces.Balancing
{
    using System;

    /// <summary>
    /// Баланс пчёл-маток.
    /// </summary>
    public class QueenBeeBalance
    {
        /// <summary>
        /// Время производства одной пчелы.
        /// </summary>
        /// <returns>Время производства одной пчелы.</returns>
        public TimeSpan TimeToProduceBee { get; set; }

        /// <summary>
        /// Тысячная доля вероятности произвести рабочую пчелу.
        /// </summary>
        /// <returns>Вероятность произвести рабочую пчелу.</returns>
        public int ThousandthPartToProduceWorker { get; set; }

        /// <summary>
        /// Тысячная доля вероятности произвести охранника.
        /// </summary>
        /// <returns>Вероятность произвести охранника.</returns>
        public int ThousandthPartToProduceGuard { get; set; }

        /// <summary>
        /// Тысячная доля вероятности произвести пчелу-матку.
        /// </summary>
        /// <returns>Вероятность произвести пчелу-матку.</returns>
        public int ThousandthPartToProduceQueen { get; set; }
    }
}