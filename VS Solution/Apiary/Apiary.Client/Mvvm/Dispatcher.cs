namespace Apiary.Client.Mvvm
{
    using System;

    using Windows.ApplicationModel.Core;
    using Windows.UI.Core;

    /// <summary>
    /// Диспетчер. Выполняет действия в потоке пользовательского интерфейсаа.
    /// </summary>
    /// <remarks>Упрощенный аналог WPF-ного Dispatcher'а.</remarks>
    internal static class Dispatcher
    {
        /// <summary>
        /// Поставить действие в очередь на выполнение из потока UI.
        /// </summary>
        /// <param name="action">Действие.</param>
        internal static void BeginInvoke(Action action)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            CoreApplication.MainView.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, 
                () => action());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}
