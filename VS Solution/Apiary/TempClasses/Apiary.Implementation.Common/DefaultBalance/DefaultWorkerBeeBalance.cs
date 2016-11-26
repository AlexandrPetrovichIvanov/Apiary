namespace Apiary.Implementation.Common.DefaultBalance
{
    using System;

    /// <summary>
    /// Стандартный баланс для рабочих пчёл.
    /// </summary>
    public class DefaultWorkerBeeBalance : IWorkerBeeBalance
    {
        /// <summary>
        /// Время сбора мёда (снаружи улья).
        /// </summary>
        /// <returns>Время сбора мёда (снаружи улья).</returns>
        public TimeSpan TimeToHarvestHoney => TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Время отдыха в улье (перерывы между полётами за мёдом).
        /// </summary>
        /// <returns>Время отдыха в улье.</returns>
        public TimeSpan TimeToRestInBeehive => TimeSpan.FromMilliseconds(1000);
    }
}