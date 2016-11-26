namespace Apiary.BeeWorkflowApiary.BeeRequests
{
    /// <summary>
    /// Возможные состояния выполнения запроса пчелы.
    /// </summary>
    public enum BeeRequestState
    {
        /// <summary>
        /// Не задано.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// Выполнено успешно.
        /// </summary>
        Success = 1,

        /// <summary>
        /// Временно не может быть выполнено.
        /// Необходимо попробовать позже.
        /// </summary>
        NeedToWait = 2
    }
}
