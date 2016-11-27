namespace Apiary.BeeWorkflowApiary.Interfaces
{
    /// <summary>
    /// Текущее состояние пчелы.
    /// </summary>
    public enum BeeWorkingState
    {
        /// <summary>
        /// Пчела работает.
        /// </summary>
        OnTheWork = 0,

        /// <summary>
        /// Пчела отдыхает.
        /// </summary>
        OnTheRest = 1
    }
}