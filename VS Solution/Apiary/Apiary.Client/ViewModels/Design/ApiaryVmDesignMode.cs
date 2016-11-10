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
        /// <summary>
        /// Создать тестовую модель представления пасеки.
        /// </summary>
        public ApiaryVmDesignMode()
        {
            Beehives = new ObservableCollection<IBeehiveVM>(new []
            {
                new BeehiveVmDesignMode(1), 
                new BeehiveVmDesignMode(2), 
                new BeehiveVmDesignMode(3), 
                new BeehiveVmDesignMode(4)
            });
        }

        /// <summary>
        /// Все ульи на пасеке.
        /// </summary>
        public ObservableCollection<IBeehiveVM> Beehives { get; }

        /// <summary>
        /// Команда запуска работы пасеки.
        /// </summary>
        public ICommand StartCommand
        {
            get
            {
                return null;
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Команда остановки работы пасеки.
        /// </summary>
        public ICommand StopCommand
        {
            get
            {
                return null;
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Команда сбора мёда.
        /// </summary>
        public ICommand HarvestHoneyCommand
        {
            get
            {
                return null;
                throw new NotImplementedException();
            }
        }
    }
}
