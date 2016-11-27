namespace Apiary.Tests.UnitTests.Utilities
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    /// <summary>
    /// Тестирование генератора случайных чисел.
    /// </summary>
    [TestClass]
    public class RandomizerTests
    {
        /// <summary>
        /// Генератор случайных чисел работает нормально в 
        /// многопоточной среде.
        /// </summary>
        [TestMethod]
        public void Randomizer_StableWorkInManyThreads()
        {
            Randomizer randomizer = new Randomizer();

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 10000; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    int random = randomizer.GetRandom(1, 1000);
                }));
            }

            tasks.ForEach(t => t.GetAwaiter().Await());
        }

        /// <summary>
        /// Метод выдает действительно случайные числа 
        /// (очень приблизительная проверка).
        /// </summary>
        [TestMethod]
        public void Randomizer_GetPrettyRealRandoms()
        {
            Randomizer randomizer = new Randomizer();

            List<int> randoms = new List<int>();

            for (int i = 0; i < 10000; i++)
            {
                randoms.Add(randomizer.GetRandom(1, 1000));
            }

            Assert.IsTrue(randoms.Count(r => r == 1) < 500);
            Assert.IsTrue(randoms.Count(r => r == 1000) < 500);

            randoms.ForEach(r =>
            {
                Assert.IsTrue(random >= 1);
                Assert.IsTrue(random <= 1000);
            });                                
        }
    }
}