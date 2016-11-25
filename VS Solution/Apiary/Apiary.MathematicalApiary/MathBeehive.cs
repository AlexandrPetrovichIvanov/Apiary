namespace Apiary.MathematicalApiary
{
    using System;
    using System.Collections.Generic;

    using Apiary.Interfaces;
    using Apiary.Interfaces.Balancing;

    /// <summary>
    /// Улей, работающий по принципу итеративных математических вычислений.
    /// </summary>
    public class MathBeehive : IBeehiveState
    {
        /// <summary>
        /// Признаки состояния работы улья.
        /// </summary>
        private bool isIterationRunning;
        private bool isStopped;

        /// <summary>
        /// Количество пчёл, которые в каждой конкретной итерации переходят из 
        /// одного состояния в другое (Работа -> Проверка на охране -> Отдых).
        /// </summary>
        private int waitingForWork;
        private int waitingForCheck;
        private int waitingForRest;

        /// <summary>
        /// Специальные очереди пчёл, находящихся в определенном состоянии.
        /// </summary>
        private readonly Queue<int> working = new Queue<int>();
        private readonly Queue<int> resting = new Queue<int>();
        private readonly Queue<int> havingCheck = new Queue<int>();
        
        /// <summary>
        /// Коэффициенты специальных очередей пчёл.
        /// </summary>
        private int workingQueeRatio;
        private int restingQueeRatio;
        private int havingCheckQueeRatio;

        /// <summary>
        /// Время в миллисекундах на совершение пчёлами
        /// определенных операций.
        /// </summary>
        private int timeToWork;
        private int timeToCheck;
        private int timeToRest;
        private int timeToProduceBee;

        /// <summary>
        /// Вероятности воспроизводства пчёл определенного типа.
        /// </summary>
        private QueenBeeBalance queenBalance;

        /// <summary>
        /// Минимальная и максимальная продолжительность какой-либо операции.
        /// </summary>
        private int minimalInterval;
        private int maximalInterval;

        /// <summary>
        /// Аккумуляторы нецелых значений, чтобы не терять дробные части.
        /// </summary>
        private double limitAccumulator = 0;
        private double newWorkersAccumulator = 0;
        private double newGuardsAccumulator = 0;
        private double newQueensAccumulator = 0;

        /// <summary>
        /// Создать улей, работающий на основе итерационных
        /// математических вычислений.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="balance"></param>
        public MathBeehive(
            IBeehiveState state, 
            ApiaryBalance balance)
        {
            this.InitializeBeehiveStateProperties(state);

            // check for normal balance (кратны минимальному)
            
            this.waitingForWork = state.BeesInsideCount - state.QueensCount - state.GuardsCount;

            this.InitializeOperationTimes(balance);
            this.InitializeIntervals();
            this.InitializeQueues(maximalInterval, minimalInterval);
            this.InitializeQueueRatios(maximalInterval);
        }

        /// <summary>
        /// Интервал одной итерации в миллисекундах.
        /// </summary>
        public int IntervalMs => this.minimalInterval;

        #region IBeehiveState implementation

        public int BeehiveNumber { get; private set; }

        public long HoneyCount { get; set; }

        public int BeesTotalCount =>
            this.WorkerBeesCount + this.GuardsCount + this.QueensCount;

        public int BeesInsideCount { get; private set; }

        public int WorkerBeesCount { get; private set; }

        public int QueensCount { get; private set; }

        public int GuardsCount { get; private set; }

        #endregion

        public void SingleIteration()
        {
            this.ThrowIfAlreadyStopped();

            if (this.isIterationRunning)
            {
                throw new InvalidOperationException(
                    "Нельзя запустить две итерации улья сразу.");
            }

            this.isIterationRunning = true;

            this.ProduceBees();
            this.DequeueAllQueues();
            this.RestingToWorking();
            this.HavingCheckToResting();
            this.WorkingToHavingCheck();

            this.isIterationRunning = false;
        }

        public void Stop()
        {
            this.ThrowIfAlreadyStopped();

            if (this.isIterationRunning)
            {
                throw new InvalidOperationException(
                    "Нельзя остановить улей. Текущая итерация еще не завершена.");
            }

            this.DequeueAllQueues();
            this.isStopped = true;
        }

        #region Initialization

        private void InitializeBeehiveStateProperties(IBeehiveState state)
        {
            this.BeehiveNumber = state.BeehiveNumber;
            this.GuardsCount = state.GuardsCount;
            this.QueensCount = state.QueensCount;
            this.WorkerBeesCount = state.WorkerBeesCount;
            this.BeesInsideCount = state.BeesInsideCount;
            this.HoneyCount = state.HoneyCount;
        }

        private void InitializeOperationTimes(ApiaryBalance balance)
        {
            this.timeToWork = (int)balance.WorkerBalance.TimeToHarvestHoney.TotalMilliseconds;
            this.timeToRest = (int)balance.WorkerBalance.TimeToRestInBeehive.TotalMilliseconds;
            this.timeToCheck = (int)balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds;
            this.timeToProduceBee = (int) balance.QueenBalance.TimeToProduceBee.TotalMilliseconds;

            this.queenBalance = balance.QueenBalance;
        }

        private void InitializeIntervals()
        {
            this.minimalInterval = Math.Min(Math.Min(this.timeToWork, this.timeToCheck), this.timeToRest);
            this.maximalInterval = Math.Max(Math.Max(this.timeToWork, this.timeToCheck), this.timeToRest);
        }

        private void InitializeQueues(int maximalInterval, int minimalInterval)
        {
            for (int i = 0; i < maximalInterval/minimalInterval; i++)
            {
                this.working.Enqueue(0);
                this.resting.Enqueue(0);
                this.havingCheck.Enqueue(0);
            }
        }

        private void InitializeQueueRatios(int maximalInterval)
        {
            this.workingQueeRatio = maximalInterval/this.timeToWork;
            this.restingQueeRatio = maximalInterval/this.timeToRest;
            this.havingCheckQueeRatio = maximalInterval/this.timeToCheck;
        }

        #endregion

        private void ProduceBees()
        {
            double allBeeProduced = this.QueensCount 
                * ((double)this.IntervalMs / this.timeToProduceBee);

            double producedQueens = allBeeProduced
                * this.queenBalance.ThousandthPartToProduceQueen / 1000;
            this.newQueensAccumulator += producedQueens;
            int currentQueensProduced = (int)this.newQueensAccumulator;
            this.QueensCount += currentQueensProduced;
            this.BeesInsideCount += currentQueensProduced;
            this.newQueensAccumulator -= currentQueensProduced;

            double producedGuards = allBeeProduced
                * this.queenBalance.ThousandthPartToProduceGuard / 1000;
            this.newGuardsAccumulator += producedGuards;
            int currentGuardsProduced = (int)this.newGuardsAccumulator;
            this.GuardsCount += currentGuardsProduced;
            this.BeesInsideCount += currentGuardsProduced;
            this.newGuardsAccumulator -= currentGuardsProduced;

            double producedWorkers = allBeeProduced
                * this.queenBalance.ThousandthPartToProduceWorker / 1000;
            this.newWorkersAccumulator += producedWorkers;
            int currentWorkersProduced = (int)this.newWorkersAccumulator;
            this.WorkerBeesCount += currentWorkersProduced;
            this.waitingForWork += currentWorkersProduced;
            this.BeesInsideCount += currentWorkersProduced;
            this.newWorkersAccumulator -= currentWorkersProduced;
        }

        private void DequeueAllQueues()
        {
            for (int i = 0; i < this.restingQueeRatio; i++)
            {
                int finishedResting = this.resting.Dequeue();
                this.BeesInsideCount -= finishedResting;
                this.waitingForWork += finishedResting;
            }

            for (int i = 0; i < this.havingCheckQueeRatio; i++)
            {
                int goInside = this.havingCheck.Dequeue();
                this.BeesInsideCount += goInside;
                this.waitingForRest += goInside;
            }

            for (int i = 0; i < this.workingQueeRatio; i++)
            {
                this.waitingForCheck += this.working.Dequeue();
            }
        }

        private void RestingToWorking()
        {
            this.working.Enqueue(this.waitingForWork);
            this.waitingForWork = 0;

            for (int i = 1; i < this.workingQueeRatio; i++)
            {
                this.working.Enqueue(0);
            }
        }

        private void HavingCheckToResting()
        {
            this.resting.Enqueue(this.waitingForRest);
            this.HoneyCount += this.waitingForRest;
            this.waitingForRest = 0;

            for (int i = 1; i < this.restingQueeRatio; i++)
            {
                this.resting.Enqueue(0);
            }
        }

        private void WorkingToHavingCheck()
        {
            double doubleLimit = (double)this.GuardsCount * this.IntervalMs / this.timeToCheck;
            this.limitAccumulator += doubleLimit;

            int currentLimit = (int)this.limitAccumulator;

            this.limitAccumulator -= currentLimit;

            if (currentLimit >= this.waitingForCheck)
            {
                this.havingCheck.Enqueue(this.waitingForCheck);
                this.waitingForCheck = 0;
            }
            else
            {
                this.havingCheck.Enqueue(currentLimit);
                this.waitingForCheck -= currentLimit;
            }

            for (int i = 1; i < this.havingCheckQueeRatio; i++)
            {
                this.havingCheck.Enqueue(0);
            }
        }

        private void ThrowIfAlreadyStopped()
        {
            if (this.isStopped)
            {
                throw new InvalidOperationException(
                    "Работа улья уже остановлена.");
            }
        }
    }
}