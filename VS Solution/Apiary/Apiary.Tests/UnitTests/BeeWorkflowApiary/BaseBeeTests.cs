namespace Apiary.Tests.UnitTests.BeeWorkflowApiary
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    
    /// <summary>
    /// Класс тестирования базового класса пчёл.
    /// </summary>
    [TestClass]
    public class BaseBeeTests
    {
        /// <summary>
        /// Пчела не может начать работу, если не связана с ульем.
        /// </summary>
        [TestMethod]
        public void BaseBee_CantStartWithoutBeehive()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Пчела больше не посылает сообщений, если работа пчелы остановлена.
        /// </summary>
        [TestMethod]
        public void BaseBee_DontSendMessagesWhenStopped()
        {
            throw new NotImplementedException();
        }
    }
}