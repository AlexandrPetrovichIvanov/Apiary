namespace Apiary.Interfaces.Balancing
{
    using System;

    /// <summary>
    /// Баланс для рабочих пчёл.
    /// </summary>
    public interface IWorkerBeeBalance
    {
        /// <summary>
        /// Время сбора мёда (снаружи улья).
        /// </summary>
        /// <returns>Время сбора мёда (снаружи улья).</returns>
        public TimeSpan TimeToHarvestHoney { get; }

        /// <summary>
        /// Время отдыха в улье (перерывы между полётами за мёдом).
        /// </summary>
        /// <returns>Время отдыха в улье.</returns>
        public TimeSpan TimeToRestInBeehive { get; }
    }
}