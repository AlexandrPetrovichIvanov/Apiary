namespace Apiary.Tests.UnitTests.Client
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.Client.XmlStates;

    /// <summary>
    /// Тесты кэшируемого xml-сериализуемого состояния пасеки.
    /// </summary>
    [TestClass]
    public class ApiaryXmlStateTests
    {
        /// <summary>
        /// Получение стандартного (не кэшированного) состояния пасеки.
        /// </summary>
        [TestMethod]
        public void ApiaryXmlState_GetStandardConfig()
        {
            ApiaryXmlState.ClearCache();
            ApiaryXmlState standardState 
                = ApiaryXmlState.LoadState();

            int workersInFirstBeehive = standardState
                .BeehiveStates.First(st => st.BeehiveNumber == 1)
                .WorkerBeesCount;

            Assert.AreEqual(30, workersInFirstBeehive);
        }

        /// <summary>
        /// Кэширование состояния пасеки.
        /// </summary>
        [TestMethod]
        public void ApiaryXmlState_SaveState()
        {
            ApiaryXmlState beforeUpdating
                = ApiaryXmlState.LoadState();

            int randomValue = new Random().Next(1, 1000);

            beforeUpdating.BeehiveStates.First(st => 
                st.BeehiveNumber == 1)
                .WorkerBeesCount = randomValue;

            beforeUpdating.SaveInCache();

            ApiaryXmlState afterUpdating = ApiaryXmlState.LoadState();

            int workersInFirstBeehive = afterUpdating
                .BeehiveStates.First(st => st.BeehiveNumber == 1)
                .WorkerBeesCount;

            Assert.AreEqual(randomValue, workersInFirstBeehive);

            ApiaryXmlState.ClearCache();
        }

        /// <summary>
        /// Очистка кэша.
        /// </summary>
        [TestMethod]
        public void ApiaryXmlState_ClearCache()
        {
            ApiaryXmlState state 
                = ApiaryXmlState.LoadState();

            int randomValue = new Random().Next(1, 1000);

            state.BeehiveStates.First(st => st.BeehiveNumber == 1)
                .WorkerBeesCount = randomValue;

            state.SaveInCache();

            ApiaryXmlState.ClearCache();
            
            ApiaryXmlState standardState
                = ApiaryXmlState.LoadState();

            int workersInFirstBeehive = standardState
                .BeehiveStates.First(st => st.BeehiveNumber == 1)
                .WorkerBeesCount;

            Assert.AreNotEqual(randomValue, workersInFirstBeehive);            
        }
    }
}