namespace Apiary.BeeWorkflowApiary.Bees
{
    /// <summary>
    /// Пчела-матка.
    /// </summary>
    public class QueenBee : BeeBase
    {
        /// <summary>
        /// Баланс пчелы-матки.
        /// </summary>
        private readonly IQueenBeeBalance balance;

        /// <summary>
        /// Получить тип пчелы.
        /// </summary>
        /// <returns>Тип пчелы.</returns>
        public BeeType Type => BeeType.Queen;

        /// <summary>
        /// Создать пчелу-матку.
        /// </summary>
        /// <param name="balance"></param>
        /// <param name="longOperationSimulator"></param>
        public QueenBee()
            : base()
        {
            this.balance = ServiceLocator.Instance.GetService<IQueenBeeBalance>();
        }

        
    }
}