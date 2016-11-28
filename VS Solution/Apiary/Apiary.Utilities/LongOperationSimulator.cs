namespace Apiary.Utilities
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;

    using Windows.System.Threading;

    /// <summary>
    /// Класс имитации длительных операций.
    /// </summary>
    /// <remarks>
    /// Реализован за счет таймеров (ThreadPoolTimer) - для каждой
    /// конкретной продолжительности - свой таймер и соответствующая
    /// очередь операций.
    /// </remarks>
    public class LongOperationSimulator : ILongOperationSimulator
    {
        /// <summary>
        /// Объект для синхронизации при создании новых таймеров.
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// Таймеры (на каждый интервал - свой таймер), 
        /// и соответствующие им очереди действий.
        /// Элемент очереди включает в себя:
        /// 1. Действие при завершении имитации длительной операции.
        /// 2. Действие после завершения имитации длительной операции. 
        /// </summary>
        private readonly ConcurrentDictionary<ThreadPoolTimer, ConcurrentQueue<Action>> timersAndQueues
            = new ConcurrentDictionary<ThreadPoolTimer, ConcurrentQueue<Action>>();

        /// <summary>
        /// Признак, что в процессе работы произошла одна или несколько ошибок.
        /// </summary>
        public bool HasError { get; private set; }

        /// <summary>
        /// Имитировать длительную операцию.
        /// </summary>
        /// <param name="duration">Продолжительность операции.</param>
        /// <param name="continueWith">Действие после завершения операции.</param>
        public void SimulateAsync(
            TimeSpan duration,
            Action continueWith)
        {
            try
            {
                ThreadPoolTimer timer = this.GetExistingTimer(duration);

                if (timer == null)
                {
                    this.CreateTimerIfNotExists(duration);
                    timer = this.GetExistingTimer(duration);
                }

                var queue = this.timersAndQueues[timer];
                queue.Enqueue(continueWith);
            }
            catch
            {
                this.HasError = true;
            }
        }

        /// <summary>
        /// Найти существующий, или создать новый таймер для заданной
        /// продолжительности операции.
        /// </summary>
        /// <param name="duration"></param>
        private void CreateTimerIfNotExists(TimeSpan duration)
        {
            if (this.GetExistingTimer(duration) == null)
            {
                lock (this.lockObject)
                {
                    if (this.GetExistingTimer(duration) == null)
                    {
                        ThreadPoolTimer newTimer = ThreadPoolTimer.CreatePeriodicTimer(
                            this.TimerElapsed,
                            duration);

                        this.timersAndQueues[newTimer] = 
                            new ConcurrentQueue<Action>();
                    }
                }
            }
        }

        /// <summary>
        /// Получить существующий таймер для заданной продолжительности.
        /// </summary>
        /// <param name="duration">Продолжительность (интервал таймера).</param>
        /// <returns>Найденный таймер или null.</returns>
        private ThreadPoolTimer GetExistingTimer(TimeSpan duration)
        {
            return this.timersAndQueues.Keys.FirstOrDefault
                (t => t.Period == duration);
        }

        /// <summary>
        /// Выполнение действий в одной итерации таймера.
        /// </summary>
        /// <param name="timer">Таймер.</param>
        private void TimerElapsed(ThreadPoolTimer timer)
        {
            try
            {
                var queue = this.timersAndQueues[timer];

                // обрабатываются действия, которые были
                // на момент старта цикла. Остальные действия
                // обработаются в следующей итерации.
                int currentCount = queue.Count;

                for (int i = 0; i < currentCount; i++)
                {
                    Action currentAction;

                    if (!queue.TryDequeue(out currentAction))
                    {
                        return;
                    }

                    if (currentAction == null)
                    {
                        return;
                    }

                    Task.Factory.StartNew(() =>
                    {
                        currentAction();
                    });
                }
            }
            catch
            {
                this.HasError = true;
            }
        }
    }
}