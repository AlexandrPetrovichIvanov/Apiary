namespace Apiary.Implementation.Interfaces
{
    using System;

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
        /// <param name="initialState">С какого момента начинать работу.</param>
        void StartWork(BeeWorkingState initialState = BeeState.NotSet);

        /// <summary>
        /// Завершить работу.
        /// </summary>
        void StopWork(); 
    }
}