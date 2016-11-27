namespace Apiary.BeeWorkflowApiary.Interfaces
{
    /// <summary>
    /// Фабрика пчёл.
    /// </summary>
    public interface IBeeFactory
    {
        /// <summary>
        /// Создать рабочую пчелу.
        /// </summary>
        /// <param name="initialState">Изначальное состояние пчелы.</param>
        /// <returns>Рабочая пчела.</returns>
        IBee CreateWorkerBee(BeeWorkingState initialState);

        /// <summary>
        /// Создать пчелу-охранника.
        /// </summary>
        /// <returns>Пчела-охранник.</returns>
        IBee CreateGuardBee();

        /// <summary>
        /// Создать пчелу-матку.
        /// </summary>
        /// <returns>Пчела-матка.</returns>
        IBee CreateQueenBee();
    }
}