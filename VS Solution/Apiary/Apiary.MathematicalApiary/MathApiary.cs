namespace Apiary.MathematicalApiary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Windows.System.Threading;

    using Apiary.Interfaces;
    using Apiary.Interfaces.Balancing;
    using Apiary.Utilities;

    public class MathApiary : IApiary
    {
        private readonly object lockObject = new object();

        private bool isWorking;

        private List<MathBeehive> beehives;

        public IEnumerable<IBeehiveState> BeehiveStates => this.beehives;

        public long HoneyCount { get; private set; }

        public void Start(IApiaryState state)
        {
            if (this.isWorking)
            {
                throw new InvalidOperationException(
                    "Пасека уже работает.");
            }

            this.isWorking = true;

            ApiaryBalance balance = ServiceLocator.Instance.GetService<ApiaryBalance>();
            this.HoneyCount = state.HoneyCount;

            lock (lockObject)
            {
                this.beehives = new List<MathBeehive>(state.BeehiveStates.Select(
                    bhState => new MathBeehive(bhState, balance)));

                ThreadPoolTimer.CreatePeriodicTimer(
                    this.TimerElapsed,
                    TimeSpan.FromMilliseconds(this.beehives.First().IntervalMs));
            }
        }

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

        public IApiaryState Stop()
        {
            lock (lockObject)
            {
                if (!this.isWorking)
                {
                    throw new InvalidOperationException(
                        "Пасека и так не работает.");
                }

                this.beehives.ForEach(bh => bh.Stop());
                this.isWorking = false;
            }

            return this;
        }

        public void CollectHoney()
        {
            lock (lockObject)
            {
                this.HoneyCount += this.beehives.Sum(bh => bh.HoneyCount);
                this.beehives.ForEach(bh => bh.HoneyCount = 0);
            }
        }
    }
}
