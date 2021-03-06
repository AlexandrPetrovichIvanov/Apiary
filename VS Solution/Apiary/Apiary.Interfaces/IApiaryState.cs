namespace Apiary.Interfaces
{
    using System.Collections.Generic;

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
        long HoneyCount { get; }
    }
}