namespace Apiary.Client.XmlStates
{
    using System.Xml.Serialization;

    using Apiary.Interfaces;

    /// <summary>
    /// Xml-������������� ����� ��������� ����.
    /// </summary>
    public class BeehiveXmlState : IBeehiveState
    {
        /// <summary>
        /// �������� ��� ������ ����� ����.
        /// </summary>
        /// <returns>����� ����.</returns>
        [XmlElement("Number")]
        public int BeehiveNumber { get; set; }

        /// <summary>
        /// �������� ��� ������ ���������� ��� � ����.
        /// </summary>
        /// <returns>���������� ���.</returns>
        [XmlElement("Honey")]
        public int HoneyCount { get; set; }

        /// <summary>
        /// �������� ��� ������ ����� ���������� ���� � ����.
        /// </summary>
        /// <returns>����� ���������� ����.</returns>
        [XmlElement("TotalBees")]
        public int BeesTotalCount { get; set; }

        /// <summary>
        /// �������� ��� ������ ���������� ���� ������ ����.
        /// </summary>
        /// <returns>���������� ���� ������ ����.</returns>
        [XmlElement("BeesInside")]
        public int BeesInsideCount { get; set; }

        /// <summary>
        /// �������� ��� ������ ���������� ������� ���� � ����.
        /// </summary>
        /// <returns>���������� ������� ����.</returns>
        [XmlElement("Workers")]
        public int WorkerBeesCount { get; set; }

        /// <summary>
        /// �������� ��� ������ ���������� ����� � ����.
        /// </summary>
        /// <returns>���������� �����.</returns>
        [XmlElement("Queens")]
        public int QueensCount { get; set; }

        /// <summary>
        /// �������� ��� ������ ���������� ����-���������� � ����.
        /// </summary>
        /// <returns>���������� ����-����������.</returns>
        [XmlElement("Guards")]
        public int GuardsCount { get; set; }
    }
}