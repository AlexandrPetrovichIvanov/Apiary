namespace Apiary.Client.ViewModels
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Модель представления пасеки.
    /// </summary>
    public interface IApiaryVM
    {
        /// <summary>
        /// Все ульи на пасеке.
        /// </summary>
        ObservableCollection<IBeehiveVM> Beehives { get; }
    }
}
