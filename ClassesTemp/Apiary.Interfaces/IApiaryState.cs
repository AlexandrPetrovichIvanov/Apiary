namespace Apiary.Interfaces
{
    /// <summary>
    /// Состояние пасеки.
    /// </summary>
    public interface IApiaryState
    {
        /// <summary>
        /// Получить состояния всех ульев на пасеке.
        /// </summary>
        /// <returns>Состояния всех ульев.</returns>
        IEnumerable<IBeehiveState> BeehiveStates { get; }

        /// <summary>
        /// Получить количество пчёл на пасеке.
        /// </summary>
        /// <returns>Количество пчёл.</returns>
        int BeesCount { get; }

        /// <summary>
        /// Получить количество ульев на пасеке.
        /// </summary>
        /// <returns>Количество ульев.</returns>
        int BeehivesCount { get; }

        /// <summary>
        /// Получить количество мёда на пасеке.
        /// </summary>
        /// <returns>Количество мёда.</returns>
        int HoneyCount { get; }
    }
}