namespace Apiary.Utilities
{
    using System;

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
        private readonly Dictionary<ThreadPoolTimer, ConcurrentQueue<KeyValuePair<Action, Action>>> timersAndQueues
            = new Dictionary<ThreadPoolTimer, ConcurrentQueue<KeyValuePair<Action, Action>>>();

        /// <summary>
        /// Имитировать длительную операцию.
        /// </summary>
        /// <param name="duration">Продолжительность операции.</param>
        /// <param name="endWith">Действие при завершении операции.</param>
        /// <param name="continueWith">Действие после завершения операции.</param>
        internal void SimulateAsync(
            TimeSpan duration,
            Action endWith,
            Action continueWith)
        {
            this.CreateTimerIfNotExists(duration);
            ThreadPoolTimer timer = this.GetExistingTimer();
            var queue = this.timersAndQueues[timer];
            queue.Enqueue(new KeyValuePair<Action, Action>(continueWith, endWith));
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

                        this.timersAndQueues[timer] = 
                            new ConcurrentQueue<KeyValuePair<Action, Action>>();
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
                (t => t.Interval == duration);
        }

        /// <summary>
        /// Выполнение действий в одной итерации таймера.
        /// </summary>
        /// <param name="timer">Таймер.</param>
        private void TimerElapsed(ThreadPoolTimer timer)
        {
            var queue = this.timersAndQueues[timer];

            // обрабатываются действия, которые были
            // на момент старта цикла. Остальные действия
            // обработаются в следующей итерации.
            int currentCount = queue.Count;
            
            for (int i = 0; i < currentCount; i++)
            {
                KeyValuePair<Action, Action> currentAction;

                bool success = queue.TryDequeue(out currentAction);

                if (!success || currentAction.Key == null || currentAction.Value == null)
                {
                    throw new InvalidOperationException(
                        "Ошибка имитации длительной операции. "
                        + "Элемент очереди действий не найден или некорректен.")
                }

                Task.Factory.StartNew(() =>
                {
                    currentAction.Key();
                    currentAction.Value();
                });
            }
        }
    }
}