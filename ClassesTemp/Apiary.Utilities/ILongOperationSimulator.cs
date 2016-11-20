namespace Apiary.Utilities
{
    using System;

    public interface ILongOperationSimulator
    {
        /// <summary>
        /// Имитировать длительную операцию.
        /// </summary>
        /// <param name="duration">Продолжительность операции.</param>
        /// <param name="endWith">Действие при завершении операции.</param>
        /// <param name="continueWith">Действие после завершения операции.</param>
        internal void SimulateAsync(
            TimeSpan duration,
            Action endWith,
            Action continueWith);
    }
}