namespace Apiary.Client.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Простая реализация команды.
    /// </summary>
    public class SimpleCommand : ICommand
    {
        /// <summary>
        /// Делегат проверки доступности команды.
        /// </summary>
        private readonly Func<object, bool> canExecute;

        /// <summary>
        /// Делегат выполнения команды.
        /// </summary>
        private readonly Action<object> execute;

        /// <summary>
        /// Событие изменения возможности выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Создать экземпляр простой команды.
        /// </summary>
        /// <param name="execute">Выполнение команды.</param>
        /// <param name="canExecute">Проверка доступности команды.</param>
        protected SimpleCommand(
            Action<object> execute,
            Func<object, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(
                    nameof(execute),
                    "Необходимо указать функцию выполнения команды.");
            }

            this.canExecute = canExecute ?? (obj => true);
            this.execute = execute;
        }

        /// <summary>
        /// Возможно ли выполнить команду.
        /// </summary>
        /// <param name="parameter">Параметр проверки доступности команды.</param>
        /// <returns>True - команду можно выполнить, false - нет.</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute(parameter);
        }

        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object parameter)
        {
            if (this.canExecute(parameter))
            {
                this.execute(parameter);
            }
            else
            {
                this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
