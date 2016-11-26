namespace Apiary.Client.ViewModels.Work
{
    /// <summary>
    /// Варианты обновления View-модели пасеки.
    /// </summary>
    internal enum RefreshApiaryVmOptions
    {
        /// <summary>
        /// Обновить в соответствии с данными реальной пасеки.
        /// </summary>
        ShowActualApiary = 0,

        /// <summary>
        /// Показать сохраненное в кэше состояние пасеки.
        /// </summary>
        ShowTempSavedState = 1
    }
}