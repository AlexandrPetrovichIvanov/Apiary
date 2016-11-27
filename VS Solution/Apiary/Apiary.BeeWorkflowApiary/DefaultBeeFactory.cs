namespace Apiary.BeeWorkflowApiary
{
    using Apiary.BeeWorkflowApiary.Bees;
    using Apiary.BeeWorkflowApiary.Interfaces;

    /// <summary>
    /// Стандартная фабрика пчёл.
    /// </summary>
    public class DefaultBeeFactory : IBeeFactory
    {
        /// <summary>
        /// Создать рабочую пчелу.
        /// </summary>
        /// <param name="initialState">Изначальное состояние пчелы.</param>
        /// <returns>Рабочая пчела.</returns>
        public IBee CreateWorkerBee(BeeWorkingState initialState)
        {
            return new WorkerBee(initialState);
        }

        /// <summary>
        /// Создать пчелу-охранника.
        /// </summary>
        /// <returns>Пчела-охранник.</returns>
        public IBee CreateGuardBee()
        {
            return new GuardBee();
        }

        /// <summary>
        /// Создать пчелу-матку.
        /// </summary>
        /// <returns>Пчела-матка.</returns>
        public IBee CreateQueenBee()
        {
            return new QueenBee();
        }
    }
}