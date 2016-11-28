namespace Apiary.Client.Mvvm
{
    using System.ComponentModel;

    /// <summary>
    /// Общая часть всех View-моделей.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Оповестить подписчиков об изменении значения свойства,
        /// если подписчики имеются.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}