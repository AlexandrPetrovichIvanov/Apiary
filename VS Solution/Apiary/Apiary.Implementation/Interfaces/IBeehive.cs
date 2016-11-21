namespace Apiary.Implementation.Interfaces
{
    using Apiary.Interfaces;

    /// <summary>
    /// Улей.
    /// </summary>
    public interface IBeehive
    {
        /// <summary>
        /// Получить текущее состояние улья.
        /// </summary>
        /// <returns>Состояние улья.</returns>
        IBeehiveState State { get; }

        /// <summary>
        /// "Запустить" улей.
        /// </summary>
        /// <param name="state">Исходное состояние улья.</param>
        void Start(IBeehiveState state);

        /// <summary>
        /// Завершить работу улья.
        /// </summary>
        /// <returns>Состояние улья на момент завершения работы.</returns>
        IBeehiveState Stop();

        /// <summary>
        /// Собрать накопленный в улье мёд.
        /// </summary>
        /// <returns>Количество собранного мёда.</returns>
        int CollectHoney();
    }
}