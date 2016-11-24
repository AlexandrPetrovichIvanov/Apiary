namespace Apiary.Client.Mvvm
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Windows.ApplicationModel;
    using Windows.UI.Xaml;

    /// <summary>
    /// Привязка команды к приостановке (suspending) приложения.
    /// </summary>
    /// <remarks>Может быть привязано сколько угодно команд.</remarks>
    public class AppSuspendingCommandBinding : DependencyObject
    {
        /// <summary>
        /// Обработчик события приостановки приложения уже присоединен.
        /// </summary>
        private static bool alreadySubscribed;

        /// <summary>
        /// Список команд для выполнения при приостановке приложения.
        /// </summary>
        private static readonly List<ICommand> commandsForExit = new List<ICommand>();

        /// <summary>
        /// Присоединяемое свойство.
        /// </summary>
        public static readonly DependencyProperty ExitCommandProperty
            = DependencyProperty.RegisterAttached(
                "ExitCommand",
                typeof (ICommand),
                typeof (AppSuspendingCommandBinding),
                new PropertyMetadata(
                    null,
                    AppSuspendingCommandBinding.ExitCommandOnPropertyChanged));

        /// <summary>
        /// Установить значение присоединяемого свойства для конкретного элемента.
        /// </summary>
        /// <param name="element">Элемент интерфейса.</param>
        /// <param name="value">Устанавливаемое значение.</param>
        public static void SetExitCommand(UIElement element, ICommand value)
        {
            element.SetValue(
                AppSuspendingCommandBinding.ExitCommandProperty, 
                value);
        }

        /// <summary>
        /// Получить значение, присоединенное к конкретному элементу.
        /// </summary>
        /// <param name="element">Элемент интерфейса.</param>
        /// <returns>Присоединенное к нему значение.</returns>
        public static ICommand GetExitCommand(UIElement element)
        {
            return (ICommand)element.GetValue(
                AppSuspendingCommandBinding.ExitCommandProperty);
        }

        /// <summary>
        /// Обработка события изменения присоединяемого свойства.
        /// </summary>
        /// <param name="dependencyObject">Объект, к которому присоединено изменное св-во.</param>
        /// <param name="args">Аргументы события изменения св-ва.</param>
        private static void ExitCommandOnPropertyChanged(
            DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs args)
        {
            if (!AppSuspendingCommandBinding.alreadySubscribed)
            {
                Application.Current.Suspending 
                    += AppSuspendingCommandBinding.ApplicationOnSuspending;
                AppSuspendingCommandBinding.alreadySubscribed = true;
            }

            AppSuspendingCommandBinding.commandsForExit.Remove(
                args.OldValue as ICommand);

            ICommand newCommand = args.NewValue as ICommand;

            if (newCommand != null)
            {
                AppSuspendingCommandBinding.commandsForExit.Add(newCommand);
            }
        }

        /// <summary>
        /// Обработчик события приостановки приложения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="suspendingEventArgs">Аргументы события.</param>
        private static void ApplicationOnSuspending(
            object sender, 
            SuspendingEventArgs suspendingEventArgs)
        {
            foreach (ICommand command in AppSuspendingCommandBinding.commandsForExit)
            {
                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }
        }
    }
}
