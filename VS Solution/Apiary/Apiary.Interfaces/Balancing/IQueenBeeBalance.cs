namespace Apiary.Interfaces.Balancing
{
    using System;

    /// <summary>
    /// Баланс пчёл-маток.
    /// </summary>
    public interface IQueenBeeBalance
    {
        /// <summary>
        /// Время производства одной пчелы.
        /// </summary>
        /// <returns>Время производства одной пчелы.</returns>
        TimeSpan TimeToProduceBee { get; }

        /// <summary>
        /// Тысячная доля вероятности произвести рабочую пчелу.
        /// </summary>
        /// <returns>Вероятность произвести рабочую пчелу.</returns>
        int ThousandthPartToProduceWorker { get; }

        /// <summary>
        /// Тысячная доля вероятности произвести охранника.
        /// </summary>
        /// <returns>Вероятность произвести охранника.</returns>
        int ThousandthPartToProduceGuard { get; }

        /// <summary>
        /// Тысячная доля вероятности произвести пчелу-матку.
        /// </summary>
        /// <returns>Вероятность произвести пчелу-матку.</returns>
        int ThousandthPartToProduceQueen { get; }
    }
}