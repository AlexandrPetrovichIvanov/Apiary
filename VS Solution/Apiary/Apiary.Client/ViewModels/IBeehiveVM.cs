namespace Apiary.Client.ViewModels
{
    using System.ComponentModel;

    using Apiary.Interfaces;

    /// <summary>
    /// Интерфейс модели представления улья.
    /// </summary>
    public interface IBeehiveVM : IBeehiveState, INotifyPropertyChanged
    {
    }
}
