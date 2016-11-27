namespace Apiary.BeeWorkflowApiary.BeeRequests
{
    /// <summary>
    /// Тип запроса пчелы к улью.
    /// </summary>
    internal enum BeeRequestType
    {
        /// <summary>
        /// Не установлено.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// Запрос на вход в улей (можно дать разрешение, если
        /// пчела успешно прошла пост охраны).
        /// </summary>
        RequestToEnterBeehive = 1,
    }
}