namespace Apiary.Utilities
{
    /// <summary>
    /// Механизм получения случайных чисел.
    /// </summary>
    public interface IRandomizer
    {
        /// <summary>
        /// Получить случайное число.
        /// </summary>
        /// <param name="min">Минимальное число.</param>
        /// <param name="max">Максимальное число.</param>
        /// <returns>Случайное число (от минимального до максимального).</returns>
        int GetRandom(int min, int max);
    }
}