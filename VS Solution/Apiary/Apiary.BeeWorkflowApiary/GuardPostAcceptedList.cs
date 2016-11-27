namespace Apiary.BeeWorkflowApiary
{
    /// <summary>
    /// Список пчёл, допущенных к проходу в улей.
    /// </summary>
    internal class GuardPostAcceptedList
    {
        /// <summary>
        /// Потокобезопасная коллекция пчёл.
        /// </summary>
        private readonly ConcurrentDictionary<IBee, bool> acceptedList
            = new ConcurrentDictionary<IBee, bool>();

        /// <summary>
        /// Зарегистрировать пчелу, как допущенную к проходу.
        /// </summary>
        /// <param name="bee">Пчела.</param>
        internal void AcceptBeeToEnter(IBee bee)
        {
            this.acceptedList[bee] = true;
        }

        /// <summary>
        /// Выяснить, допущена ли данная пчела к проходу в улей.
        /// </summary>
        /// <param name="bee">Пчела.</param>
        /// <returns>True - пчела допущена к проходу в улей.</returns>
        internal bool IsBeeAcceptedToEnter(IBee bee)
        {
            bool result;
            bool success = this.TryGetValue(bee, out result);
            return success ? result : false;
        }

        /// <summary>
        /// Отменить разрешение на проход в улей (например,
        /// до следующей проверки).
        /// </summary>
        /// <param name="bee">Пчела.</param>
        internal void ResetBeeAcceptedState(IBee bee)
        {
            this.acceptedList[bee] = false;
        }
    }
}