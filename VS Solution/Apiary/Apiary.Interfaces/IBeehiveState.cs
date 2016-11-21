namespace Apiary.Interfaces
{
    /// <summary>
    /// Состояние улья.
    /// </summary>
    public interface IBeehiveState
    {
        /// <summary>
        /// Получить номер улья.
        /// </summary>
        /// <returns>Номер улья.</returns>
        int BeehiveNumber { get; }

        /// <summary>
        /// Получить количество мёда в улье.
        /// </summary>
        /// <returns>Количество мёда.</returns>
        long HoneyCount { get; }

        /// <summary>
        /// Получить общее количество пчёл в улье.
        /// </summary>
        /// <returns>Общее количество пчёл.</returns>
        int BeesTotalCount { get; }

        /// <summary>
        /// Получить количество пчёл внутри улья.
        /// </summary>
        /// <returns>Количество пчёл внутри улья.</returns>
        int BeesInsideCount { get; }

        /// <summary>
        /// Получить количество рабочих пчёл в улье.
        /// </summary>
        /// <returns>Количество рабочих пчёл.</returns>
        int WorkerBeesCount { get; }

        /// <summary>
        /// Получить количество маток в улье.
        /// </summary>
        /// <returns>Количество маток.</returns>
        int QueensCount { get; }

        /// <summary>
        /// Получить количество пчёл-охранников в улье.
        /// </summary>
        /// <returns>Количество пчёл-охранников.</returns>
        int GuardsCount { get; }
    }
}