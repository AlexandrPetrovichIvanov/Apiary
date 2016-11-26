namespace Apiary.Client.Mvvm
{
    using System;

    using Windows.ApplicationModel.Core;
    using Windows.UI.Core;

    /// <summary>
    /// Диспетчер. Выполняет действия в контексте пользовательского интерфейсаа.
    /// </summary>
    /// <remarks>Упрощенный аналог WPF-ного Dispatcher'а.</remarks>
    internal static class Dispatcher
    {
        /// <summary>
        /// До этого Dispatcher не использовался.
        /// </summary>
        private static bool isFirstInvoke = true;

        /// <summary>
        /// Поставить действие в очередь на выполнение из потока UI.
        /// </summary>
        /// <param name="action">Действие.</param>
        internal static void BeginInvoke(Action action)
        {
            // TODO: При вызовах метода в самом начале работы приложения 
            // UWP иногда выкидывает исключение "Метод вызван в неподходящее время" 
            // (видимо, основное представление - MainView - еще не успевает 
            // до конца инициализирроваться). Пока ошибка не устранена.
            // 
            // Временное решение:
            if (Dispatcher.isFirstInvoke)
            {
                Task.Delay(BeehiveVM.IntervalMs).GetAwaiter().Await();
                Dispatcher.isFirstInvoke = false;
            }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            CoreApplication.MainView.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, 
                () => action());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}
