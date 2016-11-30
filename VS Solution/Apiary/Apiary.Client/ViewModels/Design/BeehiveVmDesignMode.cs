namespace Apiary.Client.ViewModels.Design
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    using Apiary.Client.Mvvm;

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
            this.BeehiveNumber = number;
            Task.Factory.StartNew(PermanentChangePropertiesAsync);
        }

        /// <summary>
        /// Бесконечное случайное изменение свойств.
        /// </summary>
        private async void PermanentChangePropertiesAsync()
        {
            Random rand = new Random(BeehiveNumber);

            while (true)
            {
                int nextRandom = rand.Next(0, 1000);

                this.GuardsCount = nextRandom;
                this.HoneyCount = nextRandom;

                if (PropertyChanged == null)
                {
                    await Task.Delay(500);
                    continue;
                }

                Dispatcher.BeginInvoke(() =>
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(this.GuardsCount)));
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(this.HoneyCount)));
                });

                await Task.Delay(500);
            }
            // ReSharper disable once FunctionNeverReturns так и надо для тестовой view-модели
        }

        public void StopRaisingEvents()
        {
            throw new NotSupportedException();
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
        public long HoneyCount { get; private set; } = 500;

        /// <summary>
        /// Количество пчёл-маток.
        /// </summary>
        public int QueensCount { get; } = 3;

        /// <summary>
        /// Общее количество пчёл.
        /// </summary>
        public int BeesTotalCount { get; } = 15;

        /// <summary>
        /// Количество рабочих пчёл.
        /// </summary>
        public int WorkerBeesCount { get; } = 25;

        /// <summary>
        /// Событие изменения значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
