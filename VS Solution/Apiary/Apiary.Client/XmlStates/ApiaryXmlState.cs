namespace Apiary.Client.XmlStates
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Apiary.Interfaces;

    /// <summary>
    /// Xml-сериализуемый класс состояния пасеки.
    /// </summary>
    [XmlRoot("Apiary")]
    public class ApiaryState : IApiaryState
    {
        /// <summary>
        /// Получить или задать состояния всех ульев на пасеке.
        /// </summary>
        /// <returns>Состояния всех ульев.</returns>
        [XmlArray("Beehives")]
        [XmlArrayItem("Beehive")]
        public List<BeehiveXmlState> BeehiveStates { get; set; }

        /// <summary>
        /// Получить или задать количество мёда на пасеке.
        /// </summary>
        /// <returns>Количество мёда.</returns>
        [XmlElement("Honey")]
        public int HoneyCount { get; set; }

        /// <summary>
        /// Явная реализация получения перечисления состояний ульев.
        /// </summary>
        IEnumerable<IBeehiveState> IApiaryState.BeehiveStates 
            => this.BeehiveStates;
    }
}