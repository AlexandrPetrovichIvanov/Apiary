namespace Apiary.Implementation
{
    using Apiary.Interfaces;

    /// <summary>
    /// Состояние улья.
    /// </summary>
    public class BeehiveState : IBeehiveState
    {
        /// <summary>
        /// Номер улья.
        /// </summary>
        public int BeehiveNumber { get; internal set; }

        /// <summary>
        /// Количество мёда.
        /// </summary>
        public int HoneyCount { get; internal set; }

        /// <summary>
        /// Общее количество пчёл.
        /// </summary>
        public int TotalBeesCount { get; internal set; }

        /// <summary>
        /// Общее количество пчёл внутри улья.
        /// </summary>
        public int BeesInsideCount { get; internal set; }

        /// <summary>
        /// Количество рабочих пчёл (внутри и снаружи).
        /// </summary>
        public int WorkersCount { get; internal set; }

        /// <summary>
        /// Количество пчёл-маток.
        /// </summary>
        public int QueensCount { get; internal set; }

        /// <summary>
        /// Количество пчёл-охранников.
        /// </summary>
        public int GuardsCount { get; internal set; }
    }
}
