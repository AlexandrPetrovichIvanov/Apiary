namespace Apiary.Utilities
{
    using System;

    /// <summary>
    /// Класс вспомогательных методов сериализации xml.
    /// </summary>
    public static class XmlSerialization
    {
        /// <summary>
        /// Сериализовать объект в строку xml.
        /// </summary>
        /// <param name="@object">Объект.</param>
        /// <returns>Переданный объект в виде xml-строки.</returns>
        public static string Serialize<T>(this T @object)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(
                    nameof(@object),
                    "Не передан объект для сериализации.");
            }
            
            XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
            StringWriter stringWriter = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, @object);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Десериализовать xml-строку в объект.
        /// </summary>
        /// <param name="xml">Xml-строка.</param>
        /// <returns>Объект, созданный из переданной xml-строки.</returns>
        public static T Deserialize<T>(this string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
            }
            catch (Exception)
            {                
                throw new ArgumentException(
                    nameof(xml),
                    "Ошибка десериализации. Строка не является строкой xml.");
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T result;

            using (TextReader reader = new StringReader(xml))
            {
               return (T)serializer.Deserialize(reader);
            }
        }
    }
}