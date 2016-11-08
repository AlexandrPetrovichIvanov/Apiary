namespace Apiary.Interfaces.Events
{
    using System;

    /// <summary>
    /// Аргументы события изменения состояния улья.
    /// </summary>
    public class BeehiveStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Измененное состояние улья.
        /// </summary>
        public IBeehiveState State { get; internal set; }
    }
}
