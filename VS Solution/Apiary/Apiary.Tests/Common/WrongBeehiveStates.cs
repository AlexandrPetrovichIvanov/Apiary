namespace Apiary.Tests.Common
{
    using System.Collections.Generic;

    using Apiary.Client.XmlStates;
    using Apiary.Interfaces;

    /// <summary>
    /// Класс предоставления некорректных состояний улья.
    /// </summary>
    internal static class WrongBeehiveStates
    {
        /// <summary>
        /// Получить совокупность разных некорректных состояний улья.
        /// </summary>
        /// <returns>Набор некорректных состояний улья.</returns>
        internal static IEnumerable<IBeehiveState> GetWrongStates()
        {
            return new List<IBeehiveState>(new[]
            {
                // неправильное общее количество пчёл
                new BeehiveXmlState
                {
                    HoneyCount = 0,
                    BeesTotalCount = 10,
                    BeesInsideCount = 100,
                    WorkerBeesCount = 90,
                    QueensCount = 9,
                    GuardsCount = 1
                },
                // неправильное количество пчёл внутри улья
                new BeehiveXmlState
                {
                    HoneyCount = 0,
                    BeesTotalCount = 100,
                    BeesInsideCount = 2000,
                    WorkerBeesCount = 90,
                    QueensCount = 9,
                    GuardsCount = 1
                },
                // охранники или матки снаружи улья
                new BeehiveXmlState
                {
                    HoneyCount = 0,
                    BeesTotalCount = 100,
                    BeesInsideCount = 0,
                    WorkerBeesCount = 0,
                    QueensCount = 50,
                    GuardsCount = 50
                },
                // отрицательные значения
                new BeehiveXmlState
                {
                    HoneyCount = -5,
                    BeesTotalCount = -100,
                    BeesInsideCount = 0,
                    WorkerBeesCount = -90,
                    QueensCount = -9,
                    GuardsCount = -1
                }
            });
        }
    }
}