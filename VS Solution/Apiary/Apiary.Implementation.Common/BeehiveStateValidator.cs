namespace Apiary.Implementation.Common
{
    using System.ComponentModel.DataAnnotations;

    using Apiary.Interfaces;

    /// <summary>
    /// Вспомогательный класс валидации состояния улья.
    /// </summary>
    public static class BeehiveStateValidator
    {
        /// <summary>
        /// Проверка валидности состояния улья.
        /// </summary>
        /// <param name="state">Состояние улья.</param>
        public static void Validate(this IBeehiveState state)
        {
            if (state.BeesTotalCount != (state.WorkerBeesCount + state.QueensCount + state.GuardsCount))
            {
                throw new ValidationException("Общее количество пчёл несовпадает с суммой количеств пчёл разных видов.");
            }

            if (state.BeesInsideCount < (state.QueensCount + state.GuardsCount))
            {
                throw new ValidationException("Количество пчёл внутри улья подразумевает, что снаружи находятся не только рабочие пчёлы.");
            }

            if (state.BeesInsideCount > state.BeesTotalCount)
            {
                throw new ValidationException("Количество пчёл внутри улья больше, чем количество пчёл вообще.");
            }

            if (state.BeesInsideCount < 0
                || state.BeesTotalCount < 0
                || state.GuardsCount < 0
                || state.QueensCount < 0
                || state.WorkerBeesCount < 0
                || state.BeehiveNumber < 0
                || state.HoneyCount < 0)
            {
                throw new ValidationException("Ни одно из значений не может быть < 0.");
            }
        }
    }
}
