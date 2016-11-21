namespace Apiary.Client.XmlStates
{
    using System.Xml.Serialization;

    using Apiary.Interfaces;

    /// <summary>
    /// Xml-сериализуемый класс состояния улья.
    /// </summary>
    public class BeehiveXmlState : IBeehiveState
    {
        /// <summary>
        /// Получить или задать номер улья.
        /// </summary>
        /// <returns>Номер улья.</returns>
        [XmlElement("Number")]
        public int BeehiveNumber { get; set; }

        /// <summary>
        /// Получить или задать количество мёда в улье.
        /// </summary>
        /// <returns>Количество мёда.</returns>
        [XmlElement("Honey")]
        public int HoneyCount { get; set; }

        /// <summary>
        /// Получить или задать общее количество пчёл в улье.
        /// </summary>
        /// <returns>Общее количество пчёл.</returns>
        [XmlElement("TotalBees")]
        public int BeesTotalCount { get; set; }

        /// <summary>
        /// Получить или задать количество пчёл внутри улья.
        /// </summary>
        /// <returns>Количество пчёл внутри улья.</returns>
        [XmlElement("BeesInside")]
        public int BeesInsideCount { get; set; }

        /// <summary>
        /// Получить или задать количество рабочих пчёл в улье.
        /// </summary>
        /// <returns>Количество рабочих пчёл.</returns>
        [XmlElement("Workers")]
        public int WorkerBeesCount { get; set; }

        /// <summary>
        /// Получить или задать количество маток в улье.
        /// </summary>
        /// <returns>Количество маток.</returns>
        [XmlElement("Queens")]
        public int QueensCount { get; set; }

        /// <summary>
        /// Получить или задать количество пчёл-охранников в улье.
        /// </summary>
        /// <returns>Количество пчёл-охранников.</returns>
        [XmlElement("Guards")]
        public int GuardsCount { get; set; }
    }
}