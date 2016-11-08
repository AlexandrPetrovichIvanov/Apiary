namespace Apiary.Implementation
{
    using System;

    using Apiary.Interfaces;
    using Apiary.Interfaces.Events;

    /// <summary>
    /// Пчелиный улей.
    /// </summary>
    internal class Beehive : IBeehive
    {
        /// <summary>
        /// Состояние улья на данный момент.
        /// </summary>
        public IBeehiveState State { get; private set; }

        /// <summary>
        /// Событие изменения состояния улья.
        /// </summary>
        public event EventHandler<BeehiveStateChangedEventArgs> StateChanged;

        public void Start(IBeehiveState state)
        {
            throw new NotImplementedException();
        }

        public IBeehiveState Stop()
        {
            throw new NotImplementedException();
        }
    }
}
