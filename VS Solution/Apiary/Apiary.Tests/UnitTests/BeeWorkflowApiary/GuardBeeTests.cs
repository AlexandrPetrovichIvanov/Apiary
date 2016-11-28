namespace Apiary.Tests.UnitTests.BeeWorkflowApiary
{
    using System;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    
    /// <summary>
    /// Класс тестирования пчёл-охранников.
    /// </summary>
    [TestClass]
    public class GuardBeeTests
    {
        /// <summary>
        /// Охранник не работает без очереди ожидания поста охраны.
        /// </summary>
        [TestMethod]
        public void GuardBee_DontWorkWithoutGuardPostQueue()
        {
            throw new NotImplementedException();
        }
    }
}