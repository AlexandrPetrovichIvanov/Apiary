namespace Apiary.BeeWorkflowApiary
{
    using System.Collections.Generic;

    using Apiary.BeeWorkflowApiary.Interfaces;

    /// <summary>
    /// Список пчёл, допущенных к проходу в улей.
    /// </summary>
    internal class GuardPostAcceptedList
    {
        /// <summary>
        /// Объект синхронизации.
        /// </summary>
        private object lockObject = new object();

        /// <summary>
        /// Проверенные/непроверенные пчёлы.
        /// </summary>
        private readonly Dictionary<IBee, bool> list = new Dictionary<IBee, bool>(); 

        /// <summary>
        /// Зарегистрировать пчелу, как допущенную к проходу.
        /// </summary>
        /// <param name="bee">Пчела.</param>
        internal void AcceptBeeToEnter(IBee bee)
        {
            lock (lockObject)
            {
                list[bee] = true;
            }
        }

        /// <summary>
        /// Выяснить, допущена ли данная пчела к проходу в улей.
        /// </summary>
        /// <param name="bee">Пчела.</param>
        /// <returns>True - пчела допущена к проходу в улей.</returns>
        internal bool IsBeeAcceptedToEnter(IBee bee)
        {
            lock (lockObject)
            {
                return this.list.ContainsKey(bee) && this.list[bee];
            }
        }

        /// <summary>
        /// Отменить разрешение на проход в улей (например,
        /// до следующей проверки).
        /// </summary>
        /// <param name="bee">Пчела.</param>
        internal void ResetBeeAcceptedState(IBee bee)
        {
            lock (lockObject)
            {
                list[bee] = false;
            }
        }
    }
}