namespace Apiary.Implementation.BeeRequests
{
    using System;

    /// <summary>
    /// Аргументы запроса пчелы к улью.
    /// </summary>
    public abstract class BeeRequestEventArgs : EventArgs 
    {
        /// <summary>
        /// Результат - т.е. запрошенные пчелой данные.
        /// </summary>
        /// <returns>Запрошенные пчелой данные.</returns>
        public abstract object Result { get; set; }

        /// <summary>
        /// Состояние выполнения запроса.
        /// </summary>
        /// <returns>Состояние запроса.</returns>
        public BeeRequestState State { get; set; }
    }

    /// <summary>
    /// Аргументы запроса пчелы к улью с типизированным результатом.
    /// </summary>
    public class BeeRequestEventArgs<T> : BeeRequestEventArgs 
    {
        /// <summary>
        /// Результат запроса (запрошенные пчелой данные).
        /// </summary>
        private T result;

        /// <summary>
        /// Результат - т.е. запрошенные пчелой данные.
        /// </summary>
        /// <returns>Запрошенные пчелой данные.</returns>
        public new T Result 
        { 
            get
            {
                return this.result;
            }

            set
            {
                this.result = value;
            }
        }

        /// <summary>
        /// Реализация базового свойства.
        /// </summary>
        /// <returns>Запрошенные пчелой данные.</returns>
        public object BeeRequestEventArgs.Result
        {
            get
            {
                return this.result;
            }

            set
            {
                if (!(value is T))
                {
                    throw new ArgumentException(
                        "Результат запроса пчелы имеет неподходящий тип.",
                        "value")
                    )
                }

                this.result = (T)value;
            }
        }
    }
}