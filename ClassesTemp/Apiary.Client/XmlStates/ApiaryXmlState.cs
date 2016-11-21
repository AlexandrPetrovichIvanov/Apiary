namespace Apiary.Client.XmlStates
{
    using Apiary.Interfaces;

    [XmlRoot("Apiary")]
    public class ApiaryState : IApiaryState
    {
        /// <summary>
        /// Получить или задать состояния всех ульев на пасеке.
        /// </summary>
        /// <returns>Состояния всех ульев.</returns>
        [XmlArray("Beehives")]
        [XmlArrayItem("Beehive")]
        public List<IBeehiveState> BeehiveStates { get; }

        /// <summary>
        /// Получить или задать количество мёда на пасеке.
        /// </summary>
        /// <returns>Количество мёда.</returns>
        [XmlElement("Honey")]
        public int HoneyCount { get; }

    }
}