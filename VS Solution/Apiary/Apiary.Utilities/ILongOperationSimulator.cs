namespace Apiary.Utilities
{
    using System;

    /// <summary>
    /// Имитатор длительных операций.
    /// </summary>
    public interface ILongOperationSimulator
    {
        /// <summary>
        /// Имитировать длительную операцию.
        /// </summary>
        /// <param name="duration">Продолжительность операции.</param>
        /// <param name="continueWith">Действие после завершения операции.</param>
        void SimulateAsync(
            TimeSpan duration,
            Action continueWith);

        /// <summary>
        /// Признак, что в процессе работы произошла одна или несколько ошибок.
        /// </summary>
        bool HasError { get; }
    }
}