namespace Apiary.Client.ViewModels
{
    using System.ComponentModel;

    using Apiary.Interfaces;

    /// <summary>
    /// Интерфейс модели представления улья.
    /// </summary>
    public interface IBeehiveVM : IBeehiveState, INotifyPropertyChanged
    {
        /// <summary>
        /// Перестать посылать оповещения об изменениях свойств.
        /// </summary>
        void StopRaisingEvents();
    }
}
