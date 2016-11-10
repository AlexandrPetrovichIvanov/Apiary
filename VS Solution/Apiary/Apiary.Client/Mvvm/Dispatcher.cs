namespace Apiary.Client.Mvvm
{
    using System;

    using System.Threading;

    /// <summary>
    /// Диспетчер. Выполняет действия в потоке пользовательского интерфейсаа.
    /// </summary>
    /// <remarks>Упрощенный аналог WPF-ного Dispatcher'а.</remarks>
    internal class Dispatcher
    {
        /// <summary>
        /// Контекст UI.
        /// </summary>
        private readonly SynchronizationContext uiContext;

        /// <summary>
        /// Создать экземпляр диспетчера.
        /// </summary>
        internal Dispatcher()
        {
            uiContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// Поставить действие в очередь на выполнение из потока UI.
        /// </summary>
        /// <param name="action">Действие.</param>
        internal void BeginInvoke(Action action)
        {
            uiContext.Post(o => action(), null);
        }
    }
}
