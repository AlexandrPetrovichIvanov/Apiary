namespace Apiary.Client.Mvvm
{
    using Apiary.Client.ViewModels;
    using Apiary.Client.ViewModels.Design;
    using Apiary.Utilities;

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
                    return ServiceLocator.Instance.GetService<IApiaryVM>();
                }
            }
        }

        /// <summary>
        /// Режим проектирования UI.
        /// </summary>
        public bool IsDesignMode { get; set; }
    }
}
