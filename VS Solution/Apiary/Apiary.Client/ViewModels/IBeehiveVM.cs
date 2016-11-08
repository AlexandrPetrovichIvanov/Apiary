namespace Apiary.Client.ViewModels
{
    using System.ComponentModel;

    using Apiary.Interfaces;

    /// <summary>
    /// Модель представления улья.
    /// </summary>
    public interface IBeehiveVM : IBeehiveState, INotifyPropertyChanged
    {
    }
}
