namespace Apiary.Tests.FunctionalTests.BeeWorkflowApiary
{
    using System;

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверить воспроизводство пчёл при нормальных условиях.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_CorrectBeeProducing()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверить ограничение сбора мёда при недостаточном количестве охранников.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_CorrectGuardsLimitation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверить, не могут ли несколько охранников проверять одну и ту же пчелу,
        /// тем самым делая проверку пчелы быстрее, чем нужно согласно требованиям.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_OneGuardToOneBee()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверить, не изменяется ли количество пчёл при работе улья без маток.
        /// </summary>
        [TestMethod]
        public void BeeWorkflowBeehive_BeesCountStable()
        {
            throw new NotImplementedException();
        }
    }
}