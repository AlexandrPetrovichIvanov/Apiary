namespace Apiary.Tests.UnitTests.BeeWorkflowApiary
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    
    /// <summary>
    /// Класс тестирования объектно-ориентированного улья.
    /// </summary>
    [TestClass]
    public class BeeWorkflowBeehiveTests
    {
        /// <summary>
        /// Проверить сбор мёда при нормальных условиях.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_CorrectHoneyHarvesting()
        {
            throw new NotImplementingException();
        }

        /// <summary>
        /// Проверить воспроизводство пчёл при нормальных условиях.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_CorrectBeeProducing()
        {
            throw new NotImplementingException();
        }

        /// <summary>
        /// Проверить ограничение сбора мёда при недостаточном количестве охранников.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_CorrectGuardsLimitation()
        {
            throw new NotImplementingException();
        }

        /// <summary>
        /// Проверить, не могут ли несколько охранников проверять одну и ту же пчелу,
        /// тем самым делая проверку пчелы быстрее, чем нужно согласно требованиям.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_OneGuardToOneBee()
        {
            throw new NotImplementingException();
        }

        /// <summary>
        /// Проверить, не изменяется ли количество пчёл при работе улья без маток.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_BeesCountStable()
        {
            throw new NotImplementingException();
        }
    }
}