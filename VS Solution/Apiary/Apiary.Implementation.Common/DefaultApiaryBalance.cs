namespace Apiary.Implementation.Common
{
    using Apiary.Interfaces.Balancing;

    /// <summary>
    /// Стандартный баланс "пасеки".
    /// </summary>
    public class DefaultApiaryBalance : IApiaryBalance
    {
        /// <summary>
        /// Баланс пчёл-рабочих.
        /// </summary>
        /// <returns>Баланс пчёл-рабочих.</returns>
        public IWorkerBeeBalance WorkerBalance { get; }
            = new DefaultWorkerBeeBalance();

        /// <summary>
        /// Баланс пчёл-охранников.
        /// </summary>
        /// <returns>Баланс пчёл-охранников.</returns>
        public IGuardBeeBalance GuardBalance { get; }
            = new DefaultGuardBeeBalance();

        /// <summary>
        /// Баланс пчёл-маток.
        /// </summary>
        /// <returns>Баланс пчёл-маток.</returns>
        public IQueenBeeBalance QueenBalance { get; }
            = new DefaultQueenBalance();
    }
}