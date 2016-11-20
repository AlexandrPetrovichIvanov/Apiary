namespace Apiary.Interfaces
{
    /// <summary>
    /// Пасека.
    /// </summary>
    public interface IApiary : IApiaryState
    {
        /// <summary>
        /// "Запустить" пасеку.
        /// </summary>
        /// <param name="state">Исходное состояние пасеки.</param>
        void Start(IApiaryState state);
        
        /// <summary>
        /// Остановить работу пасеки.
        /// </summary>
        /// <returns>Состояние пасеки на момент остановки.</returns>
        IApiaryState Stop();

        /// <summary>
        /// Собрать накопившийся мёд.
        /// </summary>
        /// <returns>Количество собранного мёда.</returns>
        int CollectHoney();
    }
}