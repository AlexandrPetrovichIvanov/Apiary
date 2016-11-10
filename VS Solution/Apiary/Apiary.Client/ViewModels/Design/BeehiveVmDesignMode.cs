using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Apiary.Client.ViewModels.Design
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    /// <summary>
    /// Модель представления улья (тестовая, для разработки UI).
    /// </summary>
    public class BeehiveVmDesignMode : IBeehiveVM
    {
        /// <summary>
        /// Создать модель представления улья.
        /// </summary>
        /// <param name="number">Номер улья.</param>
        internal BeehiveVmDesignMode(int number)
        {
            BeehiveNumber = number;
            Task.Factory.StartNew(PermanentChangePropertiesAsync);
        }

        private async void PermanentChangePropertiesAsync()
        {
            Random rand = new Random(BeehiveNumber);

            while (true)
            {
                int nextRandom = rand.Next(0, 1000);

                GuardsCount = nextRandom;
                HoneyCount = nextRandom;

                if (PropertyChanged == null)
                {
                    await Task.Delay(500);
                    continue;
                }

                //                CoreDispatcher disp1 = Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher;
                //                CoreDispatcher disp2 = Window.Current.Dispatcher;

                //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                //                App.CoreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                //                 {
                //                     PropertyChanged(this, new PropertyChangedEventArgs(nameof(GuardsCount)));
                //                     PropertyChanged(this, new PropertyChangedEventArgs(nameof(HoneyCount)));
                //                 });
                //#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                App.Dispatcher.BeginInvoke(() =>
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(GuardsCount)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(HoneyCount)));
                });

                await Task.Delay(500);
            }
        }

        /// <summary>
        /// Номер улья.
        /// </summary>
        public int BeehiveNumber { get; }

        /// <summary>
        /// Общее количество пчёл внутри улья.
        /// </summary>
        public int BeesInsideCount { get; } = 30;

        /// <summary>
        /// Количество охранников.
        /// </summary>
        public int GuardsCount { get; private set; } = 10;

        /// <summary>
        /// Количество мёда.
        /// </summary>
        public int HoneyCount { get; private set; } = 500;

        /// <summary>
        /// Количество пчёл-маток.
        /// </summary>
        public int QueensCount { get; } = 3;

        /// <summary>
        /// Общее количество пчёл.
        /// </summary>
        public int TotalBeesCount { get; } = 15;

        /// <summary>
        /// Количество рабочих пчёл.
        /// </summary>
        public int WorkersCount { get; } = 25;

        /// <summary>
        /// Событие изменения значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
