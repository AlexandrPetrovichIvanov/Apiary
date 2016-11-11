namespace Apiary.Client.Commands
{
    using System;

    using Apiary.Client.Mvvm;

    /// <summary>
    /// Команда остановки пасеки.
    /// </summary>
    public class StopCommand : SimpleCommand
    {
        /// <summary>
        /// Создать экземпляр команды остановки пасеки.
        /// </summary>
        /// <param name="execute">Действие остановки пасеки.</param>
        /// <param name="canExecute">Функция проверки возможности
        /// остановки пасеки.</param>
        public StopCommand(
            Action execute, 
            Func<bool> canExecute = null) 
            : base(execute, canExecute)
        {
        }
    }
}
