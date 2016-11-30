namespace Apiary.MathematicalApiary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Windows.System.Threading;

    using Apiary.Interfaces;

    /// <summary>
    /// Пасека, реализованная на основе итеративных математических
    /// вычислений.
    /// </summary>
    public class MathApiary : IApiary
    {
        /// <summary>
        /// Объект для блокировки.
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// Пасека в данный момент работает.
        /// </summary>
        private bool isWorking;

        /// <summary>
        /// Ульи.
        /// </summary>
        private List<MathBeehive> beehives;

        /// <summary>
        /// Состояния ульев.
        /// </summary>
        public IEnumerable<IBeehiveState> BeehiveStates => this.beehives;

        /// <summary>
        /// Количество мёда.
        /// </summary>
        public long HoneyCount { get; private set; }

        /// <summary>
        /// Запустить пасеку.
        /// </summary>
        /// <param name="state">Исходное состояние пасеки.</param>
        public void Start(IApiaryState state)
        {
            if (this.isWorking)
            {
                throw new InvalidOperationException(
                    "Пасека уже работает.");
            }

            this.isWorking = true;
            this.HoneyCount = state.HoneyCount;

            lock (lockObject)
            {
                this.beehives = new List<MathBeehive>(state.BeehiveStates.Select(
                    bhState => new MathBeehive(bhState)));

                ThreadPoolTimer.CreatePeriodicTimer(
                    this.TimerElapsed,
                    TimeSpan.FromMilliseconds(this.beehives.First().IntervalMs));
            }
        }

        /// <summary>
        /// Остановить пасеку.
        /// </summary>
        /// <returns>Состояние пасеки после остановки.</returns>
        public IApiaryState Stop()
        {
            lock (lockObject)
            {
                if (!this.isWorking)
                {
                    throw new InvalidOperationException(
                        "Пасека и так не работает.");
                }

                this.isWorking = false;
            }

            return this;
        }

        /// <summary>
        /// Собрать мёд.
        /// </summary>
        public void CollectHoney()
        {
            lock (lockObject)
            {
                this.HoneyCount += this.beehives.Sum(bh => bh.HoneyCount);
                this.beehives.ForEach(bh => bh.HoneyCount = 0);
            }
        }

        /// <summary>
        /// Одна итерация работы пасеки.
        /// </summary>
        /// <param name="timer">Таймер, по которому работает пасека.</param>        
        private void TimerElapsed(ThreadPoolTimer timer)
        {
            lock (lockObject)
            {
                if (this.isWorking)
                {
                    this.beehives.ForEach(bh => bh.SingleIteration());
                }
            }
        }
    }
}
