using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Apiary.Tests.subfolder
{
    [TestClass]
    public class Class1
    {
        [TestMethod]
        public void fdfdfd()
        {
            string str = new Apiary.Class1().GetString();
            Assert.AreEqual("String from model", str);
        }
    }
}
