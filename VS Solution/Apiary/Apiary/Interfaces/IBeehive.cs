namespace Apiary.Interfaces
{
    using System;

    using Apiary.Interfaces.Events;

    /// <summary>
    /// Пчелиный улей.
    /// </summary>
    public interface IBeehive
    {
        /// <summary>
        /// Информация о состоянии улья на данный момент.
        /// </summary>
        IBeehiveState State { get; }

        /// <summary>
        /// Событие изменения состояния улья.
        /// </summary>
        event EventHandler<BeehiveStateChangedEventArgs> StateChanged;

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
