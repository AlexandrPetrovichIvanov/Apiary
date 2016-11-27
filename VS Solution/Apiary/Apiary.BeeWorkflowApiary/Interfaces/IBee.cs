namespace Apiary.BeeWorkflowApiary.Interfaces
{
    using System;

    using Apiary.BeeWorkflowApiary.BeeActions;
    using Apiary.BeeWorkflowApiary.BeeRequests;

    /// <summary>
    /// Пчела.
    /// </summary>
    public interface IBee
    {
        /// <summary>
        /// Получить тип пчелы.
        /// </summary>
        /// <returns>Тип пчелы.</returns>
        BeeType Type { get; }

        /// <summary>
        /// Событие выполнения пчелой какого-либо действия.
        /// </summary>
        event EventHandler<BeeActionEventArgs> ActionPerformed;

        /// <summary>
        /// Делегат запроса пчелой каких-либо данных из улья.
        /// </summary>
        event EventHandler<BeeRequestEventArgs> RequestForBeehiveData;

        /// <summary>
        /// Начать работу.
        /// </summary>
        void StartWork();

        /// <summary>
        /// Завершить работу.
        /// </summary>
        void StopWork(); 
    }
}