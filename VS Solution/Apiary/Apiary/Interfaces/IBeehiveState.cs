namespace Apiary.Interfaces
{
    /// <summary>
    /// Состояние улья.
    /// </summary>
    public interface IBeehiveState
    {
        /// <summary>
        /// Номер улья.
        /// </summary>
        int BeehiveNumber { get; }

        /// <summary>
        /// Количество мёда.
        /// </summary>
        int HoneyCount { get; }

        /// <summary>
        /// Общее количество пчёл.
        /// </summary>
        int TotalBeesCount { get; }

        /// <summary>
        /// Общее количество пчёл внутри улья.
        /// </summary>
        int BeesInsideCount { get; }

        /// <summary>
        /// Количество рабочих пчёл (внутри и снаружи).
        /// </summary>
        int WorkersCount { get; }

        /// <summary>
        /// Количество пчёл-маток.
        /// </summary>
        int QueensCount { get; }

        /// <summary>
        /// Количество пчёл-охранников.
        /// </summary>
        int GuardsCount { get; }
    }
}
