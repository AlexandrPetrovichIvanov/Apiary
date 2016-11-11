namespace Apiary.Client.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    /// <summary>
    /// Модель представления пасеки.
    /// </summary>
    public interface IApiaryVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Состояния всех ульев на пасеке.
        /// </summary>
        ObservableCollection<IBeehiveVM> Beehives { get; }

        /// <summary>
        /// Текущее количество всего собранного мёда.
        /// </summary>
        int HoneyCount { get; }

        /// <summary>
        /// Команда "Старт".
        /// </summary>
        ICommand StartCommand { get; }

        /// <summary>
        /// Команда "Стоп".
        /// </summary>
        ICommand StopCommand { get; }

        /// <summary>
        /// Команда "Собрать мёд".
        /// </summary>
        ICommand HarvestHoneyCommand { get; }
    }
}
