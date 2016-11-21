namespace Apiary.Tests.UnitTests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.Utilities;

    /// <summary>
    /// Простые тесты кастомной реализации сервис-локатора.
    /// </summary>
    [TestClass]
    public class ServiceLocatorTests
    {
        /// <summary>
        /// Нормальное получение сервиса.
        /// </summary>
        [TestMethod]
        public void ServiceLocatorGetRegisteredService()
        {
            TestType instance = new TestType();

            ServiceLocator.Instance.RegisterService<TestType>(instance);

            Assert.AreEqual(
                instance,
                ServiceLocator.Instance.GetService<TestType>());
        }

        /// <summary>
        /// Простой класс (для теста).
        /// </summary>
        private class TestType
        {}
    }
}
