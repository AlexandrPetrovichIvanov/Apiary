namespace Apiary.Client.ViewModels.Design
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    /// <summary>
    /// Тестовая модель представления пасеки (для разработки UI).
    /// </summary>
    public class ApiaryVmDesignMode : IApiaryVM
    {
        public ApiaryVmDesignMode()
        {
            
        }

        /// <summary>
        /// Все ульи на пасеке.
        /// </summary>
        public ObservableCollection<IBeehiveVM> Beehives
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICommand StartCommand
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICommand StopCommand
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICommand HarvestHoneyCommand
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
