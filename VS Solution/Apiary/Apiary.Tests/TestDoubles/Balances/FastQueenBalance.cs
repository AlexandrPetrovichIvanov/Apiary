namespace Apiary.Tests.TestDoubles.Balances
{
    using System;

    using Apiary.Interfaces.Balancing;

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
        public FastQueenBalance(
            IQueenBeeBalance baseBalance)
        {
            this.baseBalance = baseBalance;
            this.TimeToProduceBee = TimeSpan.FromMilliseconds(
                this.baseBalance.TimeToProduceBee.TotalMilliseconds / 5);
        }

        /// <summary>
        /// Время производства одной пчелы.
        /// </summary>
        /// <returns>Время производства одной пчелы.</returns>
        public TimeSpan TimeToProduceBee { get; }

        /// <summary>
        /// Тысячная доля вероятности произвести рабочую пчелу.
        /// </summary>
        /// <returns>Вероятность произвести рабочую пчелу.</returns>
        public int ThousandthPartToProduceWorker => this.baseBalance.ThousandthPartToProduceWorker;

        /// <summary>
        /// Тысячная доля вероятности произвести охранника.
        /// </summary>
        /// <returns>Вероятность произвести охранника.</returns>
        public int ThousandthPartToProduceGuard => this.baseBalance.ThousandthPartToProduceGuard;

        /// <summary>
        /// Тысячная доля вероятности произвести пчелу-матку.
        /// </summary>
        /// <returns>Вероятность произвести пчелу-матку.</returns>
        public int ThousandthPartToProduceQueen => this.baseBalance.ThousandthPartToProduceQueen;
    }
}