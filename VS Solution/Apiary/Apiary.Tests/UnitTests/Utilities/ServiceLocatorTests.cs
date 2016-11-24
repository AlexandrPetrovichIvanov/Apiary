namespace Apiary.Tests.UnitTests.Utilities
{
    using System;
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
        public void ServiceLocator_GetRegisteredService()
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
        public void ServiceLocator_GetUnregisteredService()
        {
            try
            {
                ServiceLocator.Instance.GetService<UnregisteredService>();
                throw new Exception();
            }
            catch (InvalidOperationException)
            {                
                // так и должно быть
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