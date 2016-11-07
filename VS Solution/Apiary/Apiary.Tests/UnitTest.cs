using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Apiary.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(2*2, 4);
        }
    }
}
