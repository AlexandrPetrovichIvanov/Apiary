namespace Apiary.Interfaces
{
    /// <summary>
    /// Пчелиный улей.
    /// </summary>
    public interface IBeehive
    {
        /// <summary>
        /// Текущее состояние улья.
        /// </summary>
        IBeehiveState State { get; }

        /// <summary>
        /// Начать работу улья.
        /// </summary>
        /// <param name="state">Состояние улья.</param>
        void Start(IBeehiveState state);

        /// <summary>
        /// Завершить работу улья.
        /// </summary>
        /// <returns>Состояние улья.</returns>
        IBeehiveState Stop();
    }
}
