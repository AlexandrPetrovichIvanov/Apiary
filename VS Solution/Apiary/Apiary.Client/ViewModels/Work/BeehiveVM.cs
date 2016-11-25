namespace Apiary.Client.ViewModels.Work
{
    using System.ComponentModel;
    using System.Threading.Tasks;

    using Apiary.Client.Mvvm;
    using Apiary.Interfaces;

    public class BeehiveVM : IBeehiveVM
    {
        private readonly IBeehiveState state;

        private bool isStopped;

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

        public event PropertyChangedEventHandler PropertyChanged;

        public BeehiveVM(IBeehiveState state)
        {
            this.state = state;
            Task.Factory.StartNew(PermanentChangePropertiesAsync);
        }

        public int BeehiveNumber => state.BeehiveNumber;
        public long HoneyCount => state.HoneyCount;
        public int BeesTotalCount => state.BeesTotalCount;
        public int BeesInsideCount => state.BeesInsideCount;
        public int WorkerBeesCount => state.WorkerBeesCount;
        public int QueensCount => state.QueensCount;
        public int GuardsCount => state.GuardsCount;

        /// <summary>
        /// Бесконечное чтение свойств.
        /// </summary>
        private async void PermanentChangePropertiesAsync()
        {
            await Task.Delay(500);

            while (!this.isStopped)
            {
                if (PropertyChanged == null)
                {
                    await Task.Delay(500);
                    continue;
                }

                Dispatcher.BeginInvoke(() =>
                {
                    foreach (string name in this.namesOfProperties)
                    {
                        this.PropertyChanged(
                            this,
                            new PropertyChangedEventArgs(name));
                    }
                });

                await Task.Delay(500);
            }
        }

        public void StopRaisingEvents()
        {
            this.isStopped = true;
        }
    }
}
