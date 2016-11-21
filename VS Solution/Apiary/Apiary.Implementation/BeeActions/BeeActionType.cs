namespace Apiary.Implementation.BeeActions
{
    /// <summary>
    /// Тип действия пчелы.
    /// </summary>
    public enum BeeActionType
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
        /// Пчела зашла в улей (пройдя пост охраны).
        /// </summary>
        EnterBeehive = 3,

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