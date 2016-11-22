namespace Apiary.BeeWorkflowApiary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Apiary.Interfaces;

    /// <summary>
    /// Потокобезопасная реализация состояния пасеки.
    /// </summary>
    public class ApiaryStateThreadSafe : IApiaryState
    {
        /// <summary>
        /// Количество мёда.
        /// </summary>
        private long honeyCount;

        /// <summary>
        /// Состояния ульев.
        /// </summary>
        private readonly List<BeehiveStateThreadSafe> beehiveStates;

        /// <summary>
        /// Создать потокобезопасное изменяемое состояние пасеки.
        /// </summary>
        /// <param name="baseState">Любой интерфейс состояния пасеки.</param>
        public ApiaryStateThreadSafe(IApiaryState baseState)
        {
            if (baseState == null)
            {
                throw new ArgumentNullException(
                    nameof(baseState),
                    "Необходимо передать состояние пасеки.");
            }

            this.beehiveStates = new List<BeehiveStateThreadSafe>(
                baseState.BeehiveStates.Select(baseBeehiveState =>
                new BeehiveStateThreadSafe(baseBeehiveState)));

            this.honeyCount = baseState.HoneyCount;
        }

        /// <summary>
        /// Состояния ульев.
        /// </summary>
        public IEnumerable<IBeehiveState> BeehiveStates 
            => this.beehiveStates;
        
        /// <summary>
        /// Количество мёда.
        /// </summary>
        public long HoneyCount => this.honeyCount;

        /// <summary>
        /// Увеличить количество мёда на 1.
        /// </summary>
        public void IncrementHoneyCount()
        {
            Interlocked.Increment(ref this.honeyCount);
        }
    }
}
