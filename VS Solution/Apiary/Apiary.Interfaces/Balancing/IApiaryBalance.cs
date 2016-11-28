namespace Apiary.Interfaces.Balancing
{
    /// <summary>
    /// Общий баланс "пасеки".
    /// </summary>
    public interface IApiaryBalance
    {
        /// <summary>
        /// Баланс пчёл-рабочих.
        /// </summary>
        /// <returns>Баланс пчёл-рабочих.</returns>
        IWorkerBeeBalance WorkerBalance { get; }

        /// <summary>
        /// Баланс пчёл-охранников.
        /// </summary>
        /// <returns>Баланс пчёл-охранников.</returns>
        IGuardBeeBalance GuardBalance { get; }

        /// <summary>
        /// Баланс пчёл-маток.
        /// </summary>
        /// <returns>Баланс пчёл-маток.</returns>
        IQueenBeeBalance QueenBalance { get; }
    }
}