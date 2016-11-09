namespace Apiary.Client.ViewModels
{
    using System;

    using Apiary.Client.ViewModels.Design;

    /// <summary>
    /// View Model Locator - создание рабочих View-моделей 
    /// и View-моделей для тестирования интерфейсов.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Получить модель представления пасеки.
        /// </summary>
        public IApiaryVM ApiaryVM
        {
            get
            {
                if (this.IsDesignMode)
                {
                    return new ApiaryVmDesignMode();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Режим проектирования UI.
        /// </summary>
        public bool IsDesignMode { get; set; }
    }
}
