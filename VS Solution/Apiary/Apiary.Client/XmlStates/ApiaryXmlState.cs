namespace Apiary.Client.XmlStates
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Windows.Storage;
    using Windows.Storage.Pickers;

    using Apiary.Interfaces;
    using System;
    using System.IO;
    using Windows.Storage.Provider;

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

            if (file != null)
            {
                string xml = FileIO.ReadTextAsync(file)
                    .GetAwaiter().GetResult();
            }
            else
            {
                string xml = "FromResource";
                // получить из ресурса
            }

            return new ApiaryXmlState();
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
                //CachedFileManager.DeferUpdates(file);
                FileIO.WriteTextAsync(file, "AfterSaving file content" + DateTime.Now)
                    .GetAwaiter().GetResult();
                //FileUpdateStatus status = CachedFileManager.CompleteUpdatesAsync(file).GetResults();
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
                    .GetFileAsync("file.xml").GetAwaiter().GetResult();
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
                    "file.xml",
                    CreationCollisionOption.FailIfExists)
                .GetAwaiter().GetResult();
        }
    }
}