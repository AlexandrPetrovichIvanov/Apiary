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
        /// Получить количество мёда на пасеке.
        /// </summary>
        /// <returns>Количество мёда.</returns>
        int HoneyCount { get; }
    }
}