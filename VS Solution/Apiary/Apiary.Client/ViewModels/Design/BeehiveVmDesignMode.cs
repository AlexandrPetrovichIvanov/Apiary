using System.Threading;

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
        private readonly SynchronizationContext context;

        /// <summary>
        /// Создать модель представления улья.
        /// </summary>
        /// <param name="number">Номер улья.</param>
        internal BeehiveVmDesignMode(int number)
        {
            context = SynchronizationContext.Current;

            BeehiveNumber = number;

            Random rand = new Random(number);

            Task.Factory.StartNew(async () =>
            {
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

                    context.Post(o =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(GuardsCount)));
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(HoneyCount)));
                    },
                    null);

                    await Task.Delay(500);
                }
            });
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
