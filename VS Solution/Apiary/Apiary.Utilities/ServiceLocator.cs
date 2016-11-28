using System.Collections.Concurrent;

namespace Apiary.Utilities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Простая реализация паттерна service locator.
    /// </summary>
    public class ServiceLocator
    {
        /// <summary>
        /// Единственный экземпляр service locator'а.
        /// </summary>
        private static ServiceLocator instance;

        /// <summary>
        /// Объект для синхронизации потоков.
        /// </summary>
        private static readonly object lockObject = new object();

        /// <summary>
        /// Зарегистрированные сервисы.
        /// </summary>
        private readonly ConcurrentDictionary<Type, object> registeredServices
            = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Получить единственный экземпляр service locator'а.
        /// </summary>
        /// <returns>Экземпляр service locator'а.</returns>
        public static ServiceLocator Instance
        {
            get
            {
                if (ServiceLocator.instance == null)
                {
                    lock (ServiceLocator.lockObject)
                    {
                        if (ServiceLocator.instance == null)
                        {
                            ServiceLocator.instance = new ServiceLocator();
                        }
                    }
                }

                return ServiceLocator.instance;           
            }
        }

        /// <summary>
        /// Конструктор для предотвращения прямого создания извне.
        /// </summary>
        private ServiceLocator() {}

        /// <summary>
        /// Зарегистрировать сервис.
        /// </summary>
        /// <param name="instance">Экземпляр сервиса.</param>
        public void RegisterService<T>(T instance)
        {
            this.registeredServices[typeof(T)] = instance;
        }

        /// <summary>
        /// Получить зарегистрированный сервис.
        /// </summary>
        /// <returns>Зарегистрированный сервис.</returns>
        public T GetService<T>()
        {
            try
            {
                return (T)this.registeredServices[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException(
                    $"Попытка получить незарегистрированный сервис {typeof(T).Name}.");
            }
        }
    }
}