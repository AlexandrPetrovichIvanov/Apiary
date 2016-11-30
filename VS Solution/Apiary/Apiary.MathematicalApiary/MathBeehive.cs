namespace Apiary.MathematicalApiary
{
    using System;
    using System.Collections.Generic;

    using Apiary.Implementation.Common;
    using Apiary.Interfaces;
    using Apiary.Interfaces.Balancing;
    using Apiary.Utilities;

    /// <summary>
    /// Улей, работающий по принципу итеративных математических вычислений.
    /// </summary>
    public class MathBeehive : IBeehiveState
    {
        /// <summary>
        /// Признаки состояния работы улья.
        /// </summary>
        private bool isIterationRunning;

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
        private IQueenBeeBalance queenBalance;

        /// <summary>
        /// Минимальная и максимальная продолжительность какой-либо операции.
        /// </summary>
        private int minimalInterval;
        private int maximalInterval;

        /// <summary>
        /// Аккумуляторы нецелых значений, чтобы не терять дробные части.
        /// </summary>
        private double limitAccumulator;
        private double newWorkersAccumulator;
        private double newGuardsAccumulator;
        private double newQueensAccumulator;

        /// <summary>
        /// Создать улей, работающий на основе итерационных
        /// математических вычислений.
        /// </summary>
        /// <param name="state"></param>
        public MathBeehive(
            IBeehiveState state)
        {
            state.Validate();

            IApiaryBalance balance = ServiceLocator.Instance.GetService<IApiaryBalance>();

            this.InitializeBeehiveStateProperties(state);
            
            this.waitingForWork = state.BeesInsideCount - state.QueensCount - state.GuardsCount;
            this.waitingForCheck = state.WorkerBeesCount - waitingForWork;

            this.InitializeOperationTimes(balance);
            this.InitializeIntervals(); 
            this.ValidateIntervals();           
            this.InitializeQueues();
            this.InitializeQueueRatios();            
        }

        /// <summary>
        /// Интервал одной итерации в миллисекундах.
        /// </summary>
        public int IntervalMs => this.minimalInterval;

        #region IBeehiveState implementation

        /// <summary>
        /// Номер улья.
        /// </summary>
        public int BeehiveNumber { get; private set; }

        /// <summary>
        /// Количество мёда.
        /// </summary>
        public long HoneyCount { get; set; }

        /// <summary>
        /// Общее количество пчёл.
        /// </summary>
        public int BeesTotalCount =>
            this.WorkerBeesCount + this.GuardsCount + this.QueensCount;

        /// <summary>
        /// Количество пчёл внутри улья.
        /// </summary>
        public int BeesInsideCount { get; set; }

        /// <summary>
        /// Количество рабочих пчёл.
        /// </summary>
        public int WorkerBeesCount { get; private set; }

        /// <summary>
        /// Количество пчёл-маток.
        /// </summary>
        public int QueensCount { get; private set; }

        /// <summary>
        /// Количество пчёл-охранников.
        /// </summary>
        public int GuardsCount { get; private set; }

        #endregion

        /// <summary>
        /// Запустить одну итерацию улья, имитирующую поведение
        /// улья за интервал времени (свойство IntervalMs).
        /// </summary>
        public void SingleIteration()
        {
            if (this.isIterationRunning)
            {
                throw new InvalidOperationException(
                    "Нельзя запустить две итерации улья сразу.");
            }

            this.isIterationRunning = true;

            this.CalculateBeesInside();
            this.ProduceBees();
            this.DequeueAllQueuesForIteration();
            this.RestingToWorking();
            this.HavingCheckToResting();
            this.WorkingToHavingCheck();

            this.isIterationRunning = false;
        }

        #region Initialization

        /// <summary>
        /// Инициализировать свойства для реализации интерфейса IBeehiveState.
        /// </summary>
        /// <param name="state">Состояние улья.</param>
        private void InitializeBeehiveStateProperties(IBeehiveState state)
        {
            this.BeehiveNumber = state.BeehiveNumber;
            this.GuardsCount = state.GuardsCount;
            this.QueensCount = state.QueensCount;
            this.WorkerBeesCount = state.WorkerBeesCount;
            this.BeesInsideCount = state.BeesInsideCount;
            this.HoneyCount = state.HoneyCount;
        }

        /// <summary>
        /// Инициализировать временные интервалы различных операций.
        /// </summary>
        /// <param name="balance">Баланс пасеки.</param>
        private void InitializeOperationTimes(IApiaryBalance balance)
        {
            this.timeToWork = (int)balance.WorkerBalance.TimeToHarvestHoney.TotalMilliseconds;
            this.timeToRest = (int)balance.WorkerBalance.TimeToRestInBeehive.TotalMilliseconds;
            this.timeToCheck = (int)balance.GuardBalance.TimeToCheckOneBee.TotalMilliseconds;
            this.timeToProduceBee = (int) balance.QueenBalance.TimeToProduceBee.TotalMilliseconds;

            this.queenBalance = balance.QueenBalance;
        }

        /// <summary>
        /// Инициализировать максимальный и минимальный интервалы.
        /// </summary>
        private void InitializeIntervals()
        {
            this.minimalInterval = Math.Min(Math.Min(this.timeToWork, this.timeToCheck), this.timeToRest);
            this.maximalInterval = Math.Max(Math.Max(this.timeToWork, this.timeToCheck), this.timeToRest);
        }

        /// <summary>
        /// Проверить корректность совокупности временных интервалов.
        /// </summary>
        private void ValidateIntervals()
        {
            if (this.timeToWork % this.minimalInterval > 0
                || this.timeToRest % this.minimalInterval > 0
                || this.timeToCheck % this.minimalInterval > 0)
            {
                throw new InvalidOperationException(
                    "Для корректной работы пасеки необходимо, чтобы каждый из интервалов: "
                    + "время сбора мёда, время отдыха в улье, время проверки пчелы охранником - "
                    + "был кратен минимальному из этих интервалов.");
            }
        }

        /// <summary>
        /// Инициализировать специальные очереди.
        /// </summary>
        private void InitializeQueues()
        {
            for (int i = 0; i < this.maximalInterval / this.minimalInterval; i++)
            {
                this.working.Enqueue(0);
                this.resting.Enqueue(0);
                this.havingCheck.Enqueue(0);
            }
        }

        /// <summary>
        /// Инициализировать коэффициенты специальных очередей.
        /// </summary>
        private void InitializeQueueRatios()
        {
            this.workingQueeRatio = this.maximalInterval/this.timeToWork;
            this.restingQueeRatio = this.maximalInterval/this.timeToRest;
            this.havingCheckQueeRatio = this.maximalInterval/this.timeToCheck;
        }

        #endregion

        /// <summary>
        /// Вычислить количество пчёл внутри улья.
        /// </summary>
        private void CalculateBeesInside()
        {
            int result = this.waitingForWork + this.waitingForRest
                + this.GuardsCount + this.QueensCount;

            int maximalRatio = this.maximalInterval / this.minimalInterval;

            for (int i = 0; i < maximalRatio; i++)
            {
                int currentResting = this.resting.Dequeue();
                result += currentResting;
                this.resting.Enqueue(currentResting);
            }

            this.BeesInsideCount = result;
        }

        /// <summary>
        /// Имитировать воспроизводство пчёл в рамках одной итерации.
        /// </summary>
        private void ProduceBees()
        {
            double allBeeProduced = this.QueensCount 
                * ((double)this.IntervalMs / this.timeToProduceBee);

            double producedQueens = allBeeProduced
                * this.queenBalance.ThousandthPartToProduceQueen / 1000;
            this.newQueensAccumulator += producedQueens;
            int currentQueensProduced = (int)this.newQueensAccumulator;
            this.QueensCount += currentQueensProduced;
            this.newQueensAccumulator -= currentQueensProduced;

            double producedGuards = allBeeProduced
                * this.queenBalance.ThousandthPartToProduceGuard / 1000;
            this.newGuardsAccumulator += producedGuards;
            int currentGuardsProduced = (int)this.newGuardsAccumulator;
            this.GuardsCount += currentGuardsProduced;
            this.newGuardsAccumulator -= currentGuardsProduced;

            double producedWorkers = allBeeProduced
                * this.queenBalance.ThousandthPartToProduceWorker / 1000;
            this.newWorkersAccumulator += producedWorkers;
            int currentWorkersProduced = (int)this.newWorkersAccumulator;
            this.WorkerBeesCount += currentWorkersProduced;
            this.waitingForWork += currentWorkersProduced;
            this.newWorkersAccumulator -= currentWorkersProduced;
        }

        /// <summary>
        /// Извлечь необходимое для одной итерации количество 
        /// значений из всех специальных очередей и соответствующим
        /// образом обновить количество пчёл в определенных состояниях
        /// на начало данной итерации.
        /// </summary>
        private void DequeueAllQueuesForIteration()
        {
            for (int i = 0; i < this.restingQueeRatio; i++)
            {
                int finishedResting = this.resting.Dequeue();
                this.waitingForWork += finishedResting;
            }

            for (int i = 0; i < this.havingCheckQueeRatio; i++)
            {
                int goInside = this.havingCheck.Dequeue();
                this.waitingForRest += goInside;
            }

            for (int i = 0; i < this.workingQueeRatio; i++)
            {
                this.waitingForCheck += this.working.Dequeue();
            }
        }

        /// <summary>
        /// В рамках одной итерации: перевести пчёл из состояния
        /// "отдыхает" в состояние "работает".
        /// </summary>
        private void RestingToWorking()
        {
            this.working.Enqueue(this.waitingForWork);
            this.waitingForWork = 0;

            for (int i = 1; i < this.workingQueeRatio; i++)
            {
                this.working.Enqueue(0);
            }
        }

        /// <summary>
        /// В рамках одной итерации: перевести пчёл из состояния
        /// "проходит проверку" в состояние "отдыхает".
        /// </summary>
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

        /// <summary>
        /// В рамках одной итерации: перевести пчёл из состояния
        /// "работает" в состояние "проходит проверку".
        /// </summary>
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
    }
}