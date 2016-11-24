namespace Apiary.Utilities
{
    /// <summary>
    /// Класс вспомогательных методов для чтения 
    /// embedded-ресурсов сборки.
    /// </summary>
    public static class AssemblyResourceReader
    {
        /// <summary>
        /// Прочитать ресурс сборки.
        /// </summary>
        /// <param name="assembly">Сборка.</param>
        /// <param name="resourceFullName">Имя ресурса.</param>
        /// <returns>Ресурс в виде строки.</returns>
        public static string ReadResourceAsText(
            this Assembly assembly,
            string resourceFullName)
        {
            string result = string.Empty;

            using (Stream stream = assembly.
                GetManifestResourceStream(resourceFullName))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}