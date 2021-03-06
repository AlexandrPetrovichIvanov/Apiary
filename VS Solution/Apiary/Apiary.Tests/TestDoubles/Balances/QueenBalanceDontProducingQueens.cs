namespace Apiary.Tests.TestDoubles.Balances
{
    using System;
    using Apiary.Interfaces.Balancing;

    /// <summary>
    /// Баланс работы пчёл-маток, в которм матки не производят маток.
    /// </summary>
    internal class QueenBalanceDontProducingQueens : IQueenBeeBalance
    {
        /// <summary>
        /// Базовый (исходный) баланс.
        /// </summary>
        private readonly IQueenBeeBalance baseBalance;

        /// <summary>
        /// Создать баланс работы пчёл-маток, не производящих маток.
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
        public TimeSpan TimeToProduceBee => this.baseBalance.TimeToProduceBee;

        /// <summary>
        /// Тысячная доля вероятности произвести рабочую пчелу.
        /// </summary>
        /// <returns>Вероятность произвести рабочую пчелу.</returns>
        public int ThousandthPartToProduceWorker => this.baseBalance.ThousandthPartToProduceWorker
            + this.baseBalance.ThousandthPartToProduceQueen;

        /// <summary>
        /// Тысячная доля вероятности произвести охранника.
        /// </summary>
        /// <returns>Вероятность произвести охранника.</returns>
        public int ThousandthPartToProduceGuard => this.baseBalance.ThousandthPartToProduceGuard;

        /// <summary>
        /// Тысячная доля вероятности произвести пчелу-матку.
        /// </summary>
        /// <returns>Вероятность произвести пчелу-матку.</returns>
        public int ThousandthPartToProduceQueen => 0;
    }
}