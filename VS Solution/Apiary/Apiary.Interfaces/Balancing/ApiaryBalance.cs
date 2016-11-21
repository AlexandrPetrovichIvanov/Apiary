namespace Apiary.Interfaces.Balancing
{
    /// <summary>
    /// Общий баланс "пасеки".
    /// </summary>
    public class ApiaryBalance
    {
        /// <summary>
        /// Баланс пчёл-рабочих.
        /// </summary>
        /// <returns>Баланс пчёл-рабочих.</returns>
        public WorkerBeeBalance WorkerBalance { get; set; }

        /// <summary>
        /// Баланс пчёл-охранников.
        /// </summary>
        /// <returns>Баланс пчёл-охранников.</returns>
        public GuardBeeBalance GuardBalance { get; set; }

        /// <summary>
        /// Баланс пчёл-маток.
        /// </summary>
        /// <returns>Баланс пчёл-маток.</returns>
        public QueenBeeBalance QueenBalance { get; set; }
    }
}