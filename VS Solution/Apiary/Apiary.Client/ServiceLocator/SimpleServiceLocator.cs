namespace Apiary.Client.ServiceLocator
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Простая реализация сервис-локатора.
    /// </summary>
    internal class SimpleServiceLocator
    {
        /// <summary>
        /// Зарегистрированные сервисы.
        /// </summary>
        private readonly Dictionary<Type, object> instances
            = new Dictionary<Type, object>();

        /// <summary>
        /// Зарегистрировать сервис.
        /// </summary>
        /// <typeparam name="T">Тип сервиса.</typeparam>
        /// <param name="instance">Экземпляр сервиса.</param>
        internal void RegisterService<T>(T instance)
        {
            this.instances[typeof (T)] = instance;
        }

        /// <summary>
        /// Получить зарегистрированный сервис.
        /// </summary>
        /// <typeparam name="T">Тип сервиса.</typeparam>
        /// <returns>Зарегистрированный сервис.</returns>
        internal T GetService<T>()
        {
            Type type = typeof (T);

            if (!this.instances.ContainsKey(type))
            {
                throw new InvalidOperationException(
                    $"Сервис для типа {type} не зарегистрирован.");
            }

            return (T)this.instances[type];
        }
    }
}
