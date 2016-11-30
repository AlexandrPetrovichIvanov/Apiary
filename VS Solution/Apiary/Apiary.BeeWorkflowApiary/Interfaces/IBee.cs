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
        /// Номер улья.
        /// </summary>
        int BeehiveNumber { get; set; }

        /// <summary>
        /// Событие выполнения пчелой какого-либо действия.
        /// </summary>
        event EventHandler<BeeActionNotification> ActionPerformed;

        /// <summary>
        /// Делегат запроса пчелой каких-либо данных из улья.
        /// </summary>
        event EventHandler<BeeRequest> RequestForBeehiveData;

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