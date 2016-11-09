namespace Apiary.Client.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    /// <summary>
    /// Модель представления пасеки.
    /// </summary>
    public interface IApiaryVM
    {
        /// <summary>
        /// Состояния всех ульев на пасеке.
        /// </summary>
        ObservableCollection<IBeehiveVM> Beehives { get; }

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
