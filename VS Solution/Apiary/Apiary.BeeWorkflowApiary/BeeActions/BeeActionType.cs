namespace Apiary.BeeWorkflowApiary.BeeActions
{
    /// <summary>
    /// Тип действия пчелы.
    /// </summary>
    internal enum BeeActionType
    {
        /// <summary>
        /// Не установлено.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// Пчела улетела собирать мёд.
        /// </summary>
        LeftBeehiveToHarvestHoney = 1,

        /// <summary>
        /// Пчела вернулась с мёдом и подошла на пост охраны.
        /// </summary>
        EnterGuardPost = 2,

        /// <summary>
        /// Пчела зашла в улей с мёдом (пройдя пост охраны).
        /// </summary>
        EnterBeehiveWithHoney = 3,

        /// <summary>
        /// Охранник разрешил пчеле пройти в улей.
        /// </summary>
        AcceptBeeToEnter = 4,

        /// <summary>
        /// Матка произвела на свет пчелу.
        /// </summary>
        ProduceBee = 5
    }
}