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
            RegisteredService instance = new RegisteredService();

            ServiceLocator.Instance.RegisterService<RegisteredService>(instance);

            Assert.AreEqual(
                instance,
                ServiceLocator.Instance.GetService<RegisteredService>());
        }

        /// <summary>
        /// Попытка получения незарегистрированного сервиса.
        /// </summary>
        [TestMethod]
        public void ServiceLocatorGetUnregisteredService()
        {
            try
            {
                ServiceLocator.Instance.GetService<UnregisteredService>());
                Assert.WrongExecutionPath();
            }
            catch (InvalidOperationException)
            {                
                // так и должно быть
            }               
            catch (Exception)
            {
                Assert.WrongExecutionPath();
            } 
        }

        /// <summary>
        /// Простой класс (для теста).
        /// </summary>
        private class RegisteredService
        {}

        /// <summary>
        /// Простой класс (для теста).
        /// </summary>
        private class UnregisteredService
        {}
    }
}
