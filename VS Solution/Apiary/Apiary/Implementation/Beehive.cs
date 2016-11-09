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
