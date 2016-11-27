namespace Apiary.Utilities
{
    /// <summary>
    /// Механизм получения случайных чисел.
    /// </summary>
    /// <remarks>
    /// Использование множества стандартных Random'ов иногда приводит к созданию
    /// в них одинаковых случайных чисел, а использование одного общего - непотокобезопасно
    /// (https://msdn.microsoft.com/ru-ru/library/system.random(v=vs.110).aspx#ThreadSafety)
    /// </remarks>
    public class Randomizer : IRandomizer
    {
        /// <summary>
        /// Стандартный механизм получения случайных чисел.
        /// </summary>
        private readonly Random random = new Random();

        /// <summary>
        /// Объект синхронизации.
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// Получить случайное число.
        /// </summary>
        /// <param name="min">Минимальное число.</param>
        /// <param name="max">Максимальное число.</param>
        /// <returns>Случайное число (от минимального до максимального).</returns>
        public int GetRandom(int min, int max)
        {
            lock (lockObject)
            {
                return this.random.GetNext(min, max);
            }
        }
    }
}