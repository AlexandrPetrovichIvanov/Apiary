﻿namespace Apiary.Client.ViewModels.Design
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Apiary.Client.Mvvm;
    using Apiary.Client.XmlStates;

    /// <summary>
    /// Тестовая модель представления пасеки (для разработки UI).
    /// </summary>
    public class ApiaryVmDesignMode : ViewModelBase, IApiaryVM
    {
        /// <summary>
        /// Состояние пасеки.
        /// </summary>
        private readonly ApiaryXmlState state;

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

            this.StartCommand = new SimpleCommand(this.Start, this.CanStart);
            this.StopCommand = new SimpleCommand(this.Stop, this.CanStop);
            this.HarvestHoneyCommand = new SimpleCommand(
                this.HarvestHoney,
                this.CanHarvestHoney);

            this.RaisePropertyChanged(nameof(this.BeehivesCount));
            this.RaisePropertyChanged(nameof(this.BeesCount));

            this.state = ApiaryXmlState.LoadState();
        }

        /// <summary>
        /// Все ульи на пасеке.
        /// </summary>
        public ObservableCollection<IBeehiveVM> Beehives { get; }

        /// <summary>
        /// Общее количество собранного мёда.
        /// </summary>
        public long HoneyCount => this.state.HoneyCount;

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
            throw new NotSupportedException();
        }

        /// <summary>
        /// Призна возможности запуска пасеки.
        /// </summary>
        /// <returns>True - можно запустить.</returns>
        private bool CanStart()
        {
            return false;
        }

        /// <summary>
        /// Остановка пасеки.
        /// </summary>
        private void Stop()
        {
            throw new NotSupportedException();
            // сохранять надо не this.state, а реальное состояние пасеки
            //this.state.BeehiveStates.First().BeehiveNumber++;
            //this.state.SaveInCache();
        }

        /// <summary>
        /// Признак возможности остановки пасеки.
        /// </summary>
        /// <returns>True - пасеку можно остановить.</returns>
        private bool CanStop()
        {
            return true;
        }

        /// <summary>
        /// Собрать мёд.
        /// </summary>
        private void HarvestHoney()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Признак возможности сбора мёда.
        /// </summary>
        /// <returns>True - можно собрать мёд.</returns>
        private bool CanHarvestHoney()
        {
            return false;
        }
    }
}
