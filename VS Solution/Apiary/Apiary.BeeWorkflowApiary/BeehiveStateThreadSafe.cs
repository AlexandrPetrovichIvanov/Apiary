namespace Apiary.BeeWorkflowApiary
{
    using System.Threading;

    using Apiary.Interfaces;

    /// <summary>
    /// Потокобезопасное изменяемое состояние улья.
    /// </summary>
    public class BeehiveStateThreadSafe : IBeehiveState
    {
        /// <summary>
        /// Количество мёда.
        /// </summary>
        private long honeyCount;

        /// <summary>
        /// Общее количество пчёл внутри улья.
        /// </summary>
        private int beesInsideCount;

        /// <summary>
        /// Количество рабочих пчёл (внутри и снаружи).
        /// </summary>
        private int workerBeesCount;

        /// <summary>
        /// Количество пчёл-маток.
        /// </summary>
        private int queensCount;

        /// <summary>
        /// Количество пчёл-охранников.
        /// </summary>
        private int guardsCount; 

        /// <summary>
        /// Создать потокобезопасное изменяемое состояние улья.
        /// </summary>
        /// <param name="baseBeehiveState">Исходные данные состояния улья.</param>
        public BeehiveStateThreadSafe(IBeehiveState baseBeehiveState)
        {
            this.BeehiveNumber = baseBeehiveState.BeehiveNumber;
            this.honeyCount = baseBeehiveState.HoneyCount;
            this.beesInsideCount = baseBeehiveState.BeesInsideCount;
            this.workerBeesCount = baseBeehiveState.WorkerBeesCount;
            this.queensCount = baseBeehiveState.QueensCount;
            this.guardsCount = baseBeehiveState.GuardsCount;
        }

        /// <summary>
        /// Получить номер улья.
        /// </summary>
        /// <returns>Номер улья.</returns>
        public int BeehiveNumber { get; }

        /// <summary>
        /// Получить количество мёда в улье.
        /// </summary>
        /// <returns>Количество мёда.</returns>
        public long HoneyCount => this.honeyCount;

        /// <summary>
        /// Получить общее количество пчёл в улье.
        /// </summary>
        /// <returns>Общее количество пчёл.</returns>
        public int BeesTotalCount => this.workerBeesCount 
            + this.queensCount 
            + this.guardsCount;

        /// <summary>
        /// Получить количество пчёл внутри улья.
        /// </summary>
        /// <returns>Количество пчёл внутри улья.</returns>
        public int BeesInsideCount => this.beesInsideCount;

        /// <summary>
        /// Получить количество рабочих пчёл в улье.
        /// </summary>
        /// <returns>Количество рабочих пчёл.</returns>
        public int WorkerBeesCount => this.workerBeesCount;

        /// <summary>
        /// Получить количество маток в улье.
        /// </summary>
        /// <returns>Количество маток.</returns>
        public int QueensCount => this.queensCount;

        /// <summary>
        /// Получить количество пчёл-охранников в улье.
        /// </summary>
        /// <returns>Количество пчёл-охранников.</returns>
        public int GuardsCount => this.guardsCount;

        /// <summary>
        /// Увеличить количество мёда на 1.
        /// </summary>
        public void IncrementHoneyCount()
        {
            Interlocked.Increment(ref this.honeyCount);
        }

        /// <summary>
        /// Увеличить количество пчёл внутри улья на 1.
        /// </summary>
        public void IncrementBeesInsideCount()
        {
            Interlocked.Increment(ref this.beesInsideCount);
        }

        /// <summary>
        /// Увеличить количество рабочих пчёл на 1.
        /// </summary>
        public void IncrementWorkerBeesCount()
        {
            Interlocked.Increment(ref this.workerBeesCount);
        }

        /// <summary>
        /// Увеличить количество пчёл-маток на 1.
        /// </summary>
        public void IncrementQueensCount()
        {
            Interlocked.Increment(ref this.queensCount);
        }

        /// <summary>
        /// Увеличить количество пчёл-охранников на 1.
        /// </summary>
        public void IncrementGuardsCount()
        {
            Interlocked.Increment(ref this.guardsCount);
        }
    }
}
