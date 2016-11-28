namespace Apiary.BeeWorkflowApiary.Interfaces
{
    /// <summary>
    /// Тип пчелы.
    /// </summary>
    public enum BeeType
    {
        /// <summary>
        /// Не установлено.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// Рабочая пчела.
        /// </summary>
        Worker = 1,

        /// <summary>
        /// Пчела-охранник.
        /// </summary>
        Guard = 2,

        /// <summary>
        /// Пчела-матка.
        /// </summary>
        Queen = 3
    }
}