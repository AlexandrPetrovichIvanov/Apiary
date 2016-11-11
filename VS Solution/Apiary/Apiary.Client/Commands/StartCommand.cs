namespace Apiary.Client.Commands
{
    using System;

    using Apiary.Client.Mvvm;

    /// <summary>
    /// Команда запуска пасеки.
    /// </summary>
    public class StartCommand : SimpleCommand
    {
        /// <summary>
        /// Создать экземпляр команды запуска пасеки.
        /// </summary>
        /// <param name="execute">Действие запуска пасеки.</param>
        /// <param name="canExecute">Функция, вычисляющая признак
        /// возможности запуска.</param>
        public StartCommand(
            Action execute, 
            Func<bool> canExecute = null) 
            : base(execute, canExecute)
        {
        }
    }
}
