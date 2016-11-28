namespace Apiary.BeeWorkflowApiary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apiary.Interfaces;

    /// <summary>
    /// Пасека, основанная на непрерывной работе пчёл.
    /// </summary>
    public class BeeWorkflowApiary : IApiary
    {
        /// <summary>
        /// Ульи.
        /// </summary>
        private readonly List<BeeWorkflowBeehive> beehives
            = new List<BeeWorkflowBeehive>();

        /// <summary>
        /// Пасека работает в данный момент.
        /// </summary>
        private bool isWorking;

        /// <summary>
        /// Получить состояния всех ульев на пасеке.
        /// </summary>
        /// <returns>Состояния всех ульев.</returns>
        public IEnumerable<IBeehiveState> BeehiveStates =>
            this.beehives.Select(bh => bh.State);

        /// <summary>
        /// Получить количество мёда на пасеке.
        /// </summary>
        /// <returns>Количество мёда.</returns>
        public long HoneyCount { get; private set; }

        /// <summary>
        /// "Запустить" пасеку.
        /// </summary>
        /// <param name="state">Исходное состояние пасеки.</param>
        public void Start(IApiaryState state)
        {
            if (this.isWorking)
            {
                throw new InvalidOperationException(
                    "Ошибка запуска пасеки. Пасека уже работает.");
            }

            this.HoneyCount = state.HoneyCount;

            this.beehives.Clear();
            
            foreach (IBeehiveState beehiveState in state.BeehiveStates)
            {
                var newBeehive = new BeeWorkflowBeehive();
                this.beehives.Add(newBeehive);
                newBeehive.Start(beehiveState);
            }

            this.isWorking = true;
        }
        
        /// <summary>
        /// Остановить работу пасеки.
        /// </summary>
        /// <returns>Состояние пасеки на момент остановки.</returns>
        public IApiaryState Stop()
        {
            if (!this.isWorking)
            {
                throw new InvalidOperationException(
                    "Ошибка остановки пасеки. Пасека не запущена.");
            }

            foreach(var beehive in this.beehives)
            {
                beehive.Stop();
            }

            this.isWorking = false;
            
            return this;
        }

        /// <summary>
        /// Собрать накопившийся мёд.
        /// </summary>
        /// <returns>Количество собранного мёда.</returns>
        public void CollectHoney()
        {
            foreach (var beehive in this.beehives)
            {
                this.HoneyCount += beehive.CollectHoney();
            }
        }
    }
}