namespace Apiary.Implementation.BeeActions
{
    using System;

    /// <summary>
    /// Аргументы события любого действия пчелы.
    /// </summary>
    public class BeeActionEventArgs : EventArgs
    {
        /// <summary>
        /// Пчела-инициатор действия.
        /// </summary>
        /// <returns>Пчела-инициатор действия.</returns>
        public IBee SenderBee { get; set; }

        /// <summary>
        /// Пчела, над которой совершается действие.
        /// </summary>
        /// <returns></returns>
        public IBee RelatedBee { get; set; }

        /// <summary>
        /// Тип действия пчелы.
        /// </summary>
        /// <returns>Тип действия пчелы.</returns>
        public BeeActionType ActionType { get; set; }
    }
}