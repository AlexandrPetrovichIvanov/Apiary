namespace Apiary.BeeWorkflowApiary.BeeRequests
{
    using System;

    /// <summary>
    /// Запрос пчелы к улью.
    /// </summary>
    public class BeeRequest : EventArgs 
    {
        /// <summary>
        /// Получить или задать тип запроса.
        /// </summary>
        /// <returns>Тип запроса пчелы к улью.</returns>
        public BeeRequestType RequestType { get; set; }
        
        /// <summary>
        /// Запрос выполнен успешно.
        /// </summary>
        /// <returns>True - запрос выполнен успешно, false - нет.</returns>
        public bool Succeed { get; set; }        

        /// <summary>
        /// Результат - т.е. запрошенные пчелой данные.
        /// </summary>
        /// <returns>Запрошенные пчелой данные.</returns>
        public virtual object Response { get; set; }
    }

    /// <summary>
    /// Запрос пчелы к улью с типизированным результатом.
    /// </summary>
    internal class BeeRequest<T> : BeeRequest 
    {
        /// <summary>
        /// Результат в виде объекта конкретного типа.
        /// </summary>
        /// <returns>Запрошенные пчелой данные.</returns>
        public T TypedResponse { get; set; }

        /// <summary>
        /// Результат в виде объекта неопределенного типа.
        /// </summary>
        /// <returns>Запрошенные пчелой данные.</returns>
        public override object Response
        {
            get
            {
                return this.TypedResponse;
            }

            set
            {
                if (!(value is T))
                {
                    throw new ArgumentException(
                        "Результат запроса пчелы имеет неподходящий тип.",
                        nameof(value));
                }

                this.TypedResponse = (T)value;
            }
        }
    }
}