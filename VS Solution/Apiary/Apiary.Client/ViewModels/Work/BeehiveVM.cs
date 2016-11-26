namespace Apiary.Client.ViewModels.Work
{
    using System.ComponentModel;
    using System.Threading.Tasks;

    using Apiary.Client.Mvvm;
    using Apiary.Interfaces;

    /// <summary>
    /// Модель представления улья.
    /// </summary>
    public class BeehiveVM : ViewModelBase, IBeehiveVM
    {
        /// <summary>
        /// Стандартный интервал между отправками оповещений
        /// об изменении значений свойств (в миллисекундах).
        /// </summary>
        private const int IntervalMs = 500;

        /// <summary>
        /// Реальное состояние улья.
        /// </summary>
        private readonly IBeehiveState state;

        /// <summary>
        /// Признак остановки view-модели (именно view-модели, 
        /// т.е. процессов, связанных с UI).
        /// </summary>
        private bool isStopped;

        /// <summary>
        /// Имена всех свойств.
        /// </summary>
        private readonly string[] namesOfProperties = new[]
        {
            nameof(BeehiveNumber),
            nameof(BeesInsideCount),
            nameof(BeesTotalCount),
            nameof(GuardsCount),
            nameof(HoneyCount),
            nameof(QueensCount),
            nameof(WorkerBeesCount)
        };

        /// <summary>
        /// Создать модель представления улья.
        /// </summary>
        /// <param name="state">Состояние улья.</param>
        public BeehiveVM(IBeehiveState state)
        {
            this.state = state;
            Task.Factory.StartNew(PermanentChangePropertiesAsync);
        }

        /// <summary>
        /// Номер улья.
        /// </summary>
        public int BeehiveNumber => state.BeehiveNumber;

        /// <summary>
        /// Количество мёда.
        /// </summary>
        public long HoneyCount => state.HoneyCount;

        /// <summary>
        /// Общее количество пчёл.
        /// </summary>
        public int BeesTotalCount => state.BeesTotalCount;

        /// <summary>
        /// Количество пчёл внутри улья.
        /// </summary>
        public int BeesInsideCount => state.BeesInsideCount;

        /// <summary>
        /// Количество рабочих пчёл.
        /// </summary>
        public int WorkerBeesCount => state.WorkerBeesCount;

        /// <summary>
        /// Количество пчёл-маток.
        /// </summary>
        public int QueensCount => state.QueensCount;

        /// <summary>
        /// Количество пчёл-охранников.
        /// </summary>
        public int GuardsCount => state.GuardsCount;

        /// <summary>
        /// Бесконечное оповещение об изменении свойств.
        /// </summary>
        private async void PermanentChangePropertiesAsync()
        {
            while (!this.isStopped)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    foreach (string name in this.namesOfProperties)
                    {
                        this.RaisePropertyChanged(name);
                    }
                });

                await Task.Delay(BeehiveVM.IntervalMs);
            }
        }

        /// <summary>
        /// Остановить бесконечную отправку оповещений об изменении свойств.
        /// </summary>
        public void StopRaisingEvents()
        {
            this.isStopped = true;
        }
    }
}