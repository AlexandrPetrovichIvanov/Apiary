namespace Apiary.Client.XmlStates
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;
    using Windows.Storage;

    using Apiary.Interfaces;
    using Apiary.Utilities;

    /// <summary>
    /// Xml-сериализуемый класс состояния пасеки.
    /// </summary>
    [XmlRoot("Apiary")]
    public class ApiaryXmlState : IApiaryState
    {
        /// <summary>
        /// Имя файла, в котором хранится состояние пасеки.
        /// </summary>
        private const string CachedStateFileName = "ApiaryState.xml";

        /// <summary>
        /// Имя embedded-ресурса сборки, хранящего стандартное состояние пасеки.
        /// </summary>
        private const string StandardXmlStateResourceName =
            "Apiary.Client.XmlStates.ApiaryState.xml";

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
        public long HoneyCount { get; set; }

        /// <summary>
        /// Явная реализация получения перечисления состояний ульев.
        /// </summary>
        IEnumerable<IBeehiveState> IApiaryState.BeehiveStates
            => this.BeehiveStates;

        /// <summary>
        /// Загрузить состояние из кэша или получить стандартное.
        /// </summary>
        /// <returns></returns>
        public static ApiaryXmlState LoadState()
        {
            StorageFile file = ApiaryXmlState.GetFileFromLocalCache();
            string xml = file != null
                ? FileIO.ReadTextAsync(file).GetAwaiter().GetResult()
                : typeof(ApiaryXmlState).GetTypeInfo().Assembly.ReadResourceAsText(
                    ApiaryXmlState.StandardXmlStateResourceName);

            return xml.Deserialize<ApiaryXmlState>();
        }

        /// <summary>
        /// Создать xml-сериализуемое состояние на основе 
        /// переданного состояния любого вида.
        /// </summary>
        /// <param name="state">Состояние пасеки.</param>
        /// <returns>Новое xml-сериализуемое состояние пасеки.</returns>
        public static ApiaryXmlState CopyFrom(IApiaryState state)
        {
            return new ApiaryXmlState
            {
                HoneyCount = state.HoneyCount,
                BeehiveStates = new List<BeehiveXmlState>(
                    state.BeehiveStates.Select(bhState => new BeehiveXmlState
                    {
                        BeehiveNumber = bhState.BeehiveNumber,
                        HoneyCount = bhState.HoneyCount,
                        BeesTotalCount = bhState.BeesTotalCount,
                        BeesInsideCount = bhState.BeesInsideCount,
                        WorkerBeesCount = bhState.WorkerBeesCount,
                        QueensCount = bhState.QueensCount,
                        GuardsCount = bhState.GuardsCount
                    }))
            };
        }

        /// <summary>
        /// Очистить кэш - удалить сохраненный файл состояния
        /// пасеки, если такой файл есть.
        /// </summary>
        public static void ClearCache()
        {
            ApiaryXmlState.GetFileFromLocalCache()?.DeleteAsync()
                .GetAwaiter().GetResult();
        }

        /// <summary>
        /// Сохранить это состояние в кэше приложения.
        /// </summary>
        public void SaveInCache()
        {
            StorageFile file = ApiaryXmlState.GetFileFromLocalCache()
                ?? ApiaryXmlState.CreateNewFileInCache();

            if (file != null)
            {
                FileIO.WriteTextAsync(file, this.Serialize())
                    .GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// Получить файл состояния, сохраненный в кэше.
        /// </summary>
        /// <returns>Файл состояния пасеки, или null.</returns>
        private static StorageFile GetFileFromLocalCache()
        {
            try
            {
                return ApplicationData.Current.LocalFolder
                    .GetFileAsync(ApiaryXmlState.CachedStateFileName)
                    .GetAwaiter().GetResult();
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Создать новый файл в кэше для хранения состояния.
        /// </summary>
        /// <returns>Файл в кэше для хранения состояния.</returns>
        private static StorageFile CreateNewFileInCache()
        {
            return ApplicationData.Current.LocalFolder
                .CreateFileAsync(
                    ApiaryXmlState.CachedStateFileName,
                    CreationCollisionOption.FailIfExists)
                .GetAwaiter().GetResult();
        }
    }
}
