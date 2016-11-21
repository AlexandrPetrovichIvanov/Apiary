namespace Apiary.Implementation.Interfaces
{
    /// <summary>
    /// Текущее состояние пчелы.
    /// </summary>
    public enum BeeWorkingState
    {
        /// <summary>
        /// Не задано.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// Пчела работает.
        /// </summary>
        OnTheWork = 1,

        /// <summary>
        /// Пчела отдыхает.
        /// </summary>
        OnTheRest = 2
    }
}