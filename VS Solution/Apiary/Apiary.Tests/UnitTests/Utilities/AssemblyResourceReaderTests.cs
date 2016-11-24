namespace Apiary.Tests.UnitTests.Utilities
{
    using System.Reflection;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.Utilities;

    /// <summary>
    /// Тестирование класса чтения ресурсов сборки.
    /// </summary>
    [TestClass]
    public class AssemblyResourceReaderTests
    {
        /// <summary>
        /// Чтение файла ресурса сборки.
        /// </summary>
        [TestMethod]
        public void AssemblyResourceReader_GetEmbeddedResource()
        {
            string result = this.GetType().GetTypeInfo().Assembly
                .ReadResourceAsText("Apiary.Tests.UnitTests.Utilities.Resources.ResourceFileExample.txt");

            Assert.AreEqual("Example", result);
        }
    }
}