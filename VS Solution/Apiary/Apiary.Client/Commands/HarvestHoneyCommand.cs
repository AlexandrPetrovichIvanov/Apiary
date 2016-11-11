namespace Apiary.Client.Commands
{
    using System;

    using Apiary.Client.Mvvm;
    
    /// <summary>
    /// Команда сбора мёда.
    /// </summary>
    public class HarvestHoneyCommand : SimpleCommand
    {
        /// <summary>
        /// Создать экземпляр команды сбора мёда.
        /// </summary>
        /// <param name="execute">Действие - сбор мёда.</param>
        /// <param name="canExecute">Функция проверки
        /// возможности сбора мёда.</param>
        public HarvestHoneyCommand(
            Action execute, 
            Func<bool> canExecute = null) 
            : base(execute, canExecute)
        {
        }
    }
}
