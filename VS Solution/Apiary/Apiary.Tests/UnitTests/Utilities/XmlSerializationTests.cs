namespace Apiary.Tests.UnitTests.Utilities
{
    using System;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Apiary.Utilities;

    /// <summary>
    /// Тестирование класса вспомогательных методов xml-сериализации.
    /// </summary>
    [TestClass]
    public class XmlSerializationTests
    {
        /// <summary>
        /// Нормальная сериализация.
        /// </summary>
        [TestMethod]
        public void XmlSerialization_Serialize()
        {
            Serializable testObj = new Serializable
            {
                A = 2,
                B = 3
            };

            string xml = testObj.Serialize();

            XDocument doc = XDocument.Parse(xml);
            XElement bElement = doc.Root.Element("Second");

            Assert.AreEqual(3, int.Parse(bElement.Value)); 
        }

        /// <summary>
        /// Сериализация несериализуемого объекта.
        /// </summary>
        [TestMethod]
        public void XmlSerialization_WrongSerialize()
        {
            try
            {
                NotSerializable wrongObj = new NotSerializable
                {
                    A = 2,
                    B = 3
                };

                wrongObj.Serialize();
                throw new AssertFailedException(
                    "Объект не должен был сериализоваться в xml.");
            }
            catch
            {
                // так и должно быть         
            }
        }

        /// <summary>
        /// Сериализация пустого объекта.
        /// </summary>
        [TestMethod]
        public void XmlSerialization_NullObjectSerialize()
        {
            try
            {
                ((object) null).Serialize();
                throw new AssertFailedException(
                    "Пустой объект не должен был сериализоваться в xml.");
            }
            catch (ArgumentException)
            {
                // так и должно быть         
            }
        }

        /// <summary>
        /// Нормальная десериализация.
        /// </summary>
        [TestMethod]
        public void XmlSerialization_Deserialize()
        {
            string xmlString = "<XmlTest><First>3</First><Second>25</Second></XmlTest>";

            Serializable result = xmlString.Deserialize<Serializable>();

            Assert.AreEqual(25, result.B);
        }

        /// <summary>
        /// Десериализация строки, не являющейся xml.
        /// </summary>
        [TestMethod]
        public void XmlSerialization_WrongDeserialize()
        {
            try
            {
                string xmlString = "wrongXml";
                xmlString.Deserialize<Serializable>();
                throw new AssertFailedException(
                    "Некорректная xml-строка не должна была десериализоваться.");
            }
            catch (ArgumentException)
            {
                // так и должно быть
            }
        }

        /// <summary>
        /// Сериализуемый объект.
        /// </summary>
        [XmlRoot("XmlTest")]
        public class Serializable
        {
            /// <summary>
            /// Тестовое свойство.
            /// </summary>
            [XmlElement("First")]
            public int A { get; set; }

            /// <summary>
            /// Тестовое свойство.
            /// </summary>
            [XmlElement("Second")]
            public int B { get; set; }
        }

        /// <summary>
        /// Несериализуемый объект.
        /// </summary>
        public class NotSerializable
        {
            /// <summary>
            /// Тестовое свойство.
            /// </summary>
            public int A { get; set; }

            /// <summary>
            /// Тестовое свойство.
            /// </summary>
            public int B { get; set; }
        }
    }
}