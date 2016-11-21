namespace Apiary.Interfaces.Balancing
{
    using System;

    /// <summary>
    /// Баланс для рабочих пчёл.
    /// </summary>
    public class WorkerBeeBalance
    {
        /// <summary>
        /// Время сбора мёда (снаружи улья).
        /// </summary>
        /// <returns>Время сбора мёда (снаружи улья).</returns>
        public TimeSpan TimeToHarvestHoney { get; set; }

        /// <summary>
        /// Время отдыха в улье (перерывы между полётами за мёдом).
        /// </summary>
        /// <returns>Время отдыха в улье.</returns>
        public TimeSpan TimeToRestInBeehive { get; set; }

        /// <summary>
        /// Интервал между запросами к охране о входе в улей.
        /// </summary>
        /// <returns>Интервал между запросами к охране о входе в улей.</returns>
        public TimeSpan EnterRequestsInterval { get; set; }
    }
}