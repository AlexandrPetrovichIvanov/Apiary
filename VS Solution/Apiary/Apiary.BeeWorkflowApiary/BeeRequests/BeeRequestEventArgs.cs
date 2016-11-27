namespace Apiary.BeeWorkflowApiary.BeeRequests
{
    using System;

    /// <summary>
    /// Аргументы запроса пчелы к улью.
    /// </summary>
    internal class BeeRequestEventArgs : EventArgs 
    {
        /// <summary>
        /// Запрос выполнен успешно.
        /// </summary>
        /// <returns>True - запрос выполнен успешно, false - нет.</returns>
        public bool Succeed { get; set; }        

        /// <summary>
        /// Результат - т.е. запрошенные пчелой данные.
        /// </summary>
        /// <returns>Запрошенные пчелой данные.</returns>
        public object Response { get; set; }

        /// <summary>
        /// Состояние выполнения запроса.
        /// </summary>
        /// <returns>Состояние запроса.</returns>
        public BeeRequestState State { get; set; }
    }

    /// <summary>
    /// Аргументы запроса пчелы к улью с типизированным результатом.
    /// </summary>
    internal class BeeRequestEventArgs<T> : BeeRequestEventArgs 
    {
        /// <summary>
        /// Результат в виде объекта конкретного типа.
        /// </summary>
        /// <returns>Запрошенные пчелой данные.</returns>
        public T TypedResult { get; set; }

        /// <summary>
        /// Результат в виде объекта неопределенного типа.
        /// </summary>
        /// <returns>Запрошенные пчелой данные.</returns>
        public override object Response
        {
            get
            {
                return this.TypedResult;
            }

            set
            {
                if (!(value is T))
                {
                    throw new ArgumentException(
                        "Результат запроса пчелы имеет неподходящий тип.",
                        nameof(value));
                }

                this.TypedResult = (T)value;
            }
        }
    }
}