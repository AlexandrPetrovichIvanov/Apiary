using System.Linq;

namespace Apiary.Client.ViewModels.Design
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    using Apiary.Client.Commands;

    /// <summary>
    /// Тестовая модель представления пасеки (для разработки UI).
    /// </summary>
    public class ApiaryVmDesignMode : IApiaryVM
    {
        /// <summary>
        /// Общее количество собранного мёда.
        /// </summary>
        private int honeyCount;

        /// <summary>
        /// Событие изменения значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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

            this.StartCommand = new StartCommand(this.Start, this.CanStart);
            this.StopCommand = new StopCommand(this.Stop, this.CanStop);
            this.HarvestHoneyCommand = new HarvestHoneyCommand(
                this.HarvestHoney,
                this.CanHarvestHoney);

            this.PropertyChanged?.Invoke(
                this, 
                new PropertyChangedEventArgs(nameof(this.BeehivesCount)));
            this.PropertyChanged?.Invoke(
                this, 
                new PropertyChangedEventArgs(nameof(this.BeesCount)));
        }

        /// <summary>
        /// Все ульи на пасеке.
        /// </summary>
        public ObservableCollection<IBeehiveVM> Beehives { get; }

        /// <summary>
        /// Общее количество собранного мёда.
        /// </summary>
        public int HoneyCount
        {
            get { return this.honeyCount; }
            private set
            {
                this.honeyCount = value;
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Количество ульев.
        /// </summary>
        public int BeehivesCount => this.Beehives.Count;

        /// <summary>
        /// Общее количество пчёл.
        /// </summary>
        public int BeesCount => this.Beehives.Sum(beehive => beehive.BeesTotalCount);

        /// <summary>
        /// Команда запуска работы пасеки.
        /// </summary>
        public ICommand StartCommand { get; }

        /// <summary>
        /// Команда остановки работы пасеки.
        /// </summary>
        public ICommand StopCommand { get; }

        /// <summary>
        /// Команда сбора мёда.
        /// </summary>
        public ICommand HarvestHoneyCommand { get; }

        /// <summary>
        /// Запуск пасеки.
        /// </summary>
        private void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Призна возможности запуска пасеки.
        /// </summary>
        /// <returns>True - можно запустить.</returns>
        private bool CanStart()
        {
            return false;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Остановка пасеки.
        /// </summary>
        private void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Признак возможности остановки пасеки.
        /// </summary>
        /// <returns>True - пасеку можно остановить.</returns>
        private bool CanStop()
        {
            return false;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Собрать мёд.
        /// </summary>
        private void HarvestHoney()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Признак возможности сбора мёда.
        /// </summary>
        /// <returns>True - можно собрать мёд.</returns>
        private bool CanHarvestHoney()
        {
            return false;
            throw new NotImplementedException();
        }
    }
}
