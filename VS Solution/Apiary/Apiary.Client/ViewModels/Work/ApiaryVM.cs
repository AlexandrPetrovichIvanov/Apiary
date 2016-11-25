namespace Apiary.Client.ViewModels.Work
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    using Apiary.Client.XmlStates;
    using Apiary.Client.Commands;
    using Apiary.Interfaces;
    using Apiary.Utilities;

    public class ApiaryVM : IApiaryVM
    {
        /// <summary>
        /// Признак того, что пасека работает.
        /// </summary>
        private bool isWorking;

        /// <summary>
        /// Сама пасека.
        /// </summary>
        private IApiary apiary;

        /// <summary>
        /// Состояние пасеки.
        /// </summary>
        private readonly ApiaryXmlState state;

        /// <summary>
        /// Событие изменения значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Создать тестовую модель представления пасеки.
        /// </summary>
        public ApiaryVM()
        {
            this.state = ApiaryXmlState.LoadState();

            this.apiary = ServiceLocator.Instance.GetService<IApiary>();

            this.Beehives = new ObservableCollection<IBeehiveVM>(
                this.state.BeehiveStates.Select(xmlState =>
                    new BeehiveVM(xmlState)));

            foreach (IBeehiveVM beehiveVm in this.Beehives)
            {
                beehiveVm.PropertyChanged += this.BeehiveVmOnPropertyChanged;
            }

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

        private void BeehiveVmOnPropertyChanged(
            object sender, 
            PropertyChangedEventArgs propertyChangedEventArgs)
        {
            IBeehiveVM beehive = (IBeehiveVM) sender;

            if (propertyChangedEventArgs.PropertyName == nameof(beehive.BeesTotalCount))
            {
                this.PropertyChanged?.Invoke(
                    this,
                    new PropertyChangedEventArgs(nameof(this.BeesCount)));
            }
        }

        /// <summary>
        /// Все ульи на пасеке.
        /// </summary>
        public ObservableCollection<IBeehiveVM> Beehives { get; }

        /// <summary>
        /// Общее количество собранного мёда.
        /// </summary>
        public long HoneyCount => this.apiary.HoneyCount;

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
            if (this.CanStart())
            {
                this.apiary.Start(this.state);
                this.Beehives.Clear();

                foreach (IBeehiveState beehiveState in this.apiary.BeehiveStates)
                {
                    this.Beehives.Add(new BeehiveVM(beehiveState));
                }
            }
        }

        /// <summary>
        /// Призна возможности запуска пасеки.
        /// </summary>
        /// <returns>True - можно запустить.</returns>
        private bool CanStart()
        {
            return !this.isWorking;
        }

        /// <summary>
        /// Остановка пасеки.
        /// </summary>
        private void Stop()
        {
            // сохранять надо не this.state, а реальное состояние пасеки
            this.state.BeehiveStates.First().BeehiveNumber++;
            this.state.SaveInCache();
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Признак возможности остановки пасеки.
        /// </summary>
        /// <returns>True - пасеку можно остановить.</returns>
        private bool CanStop()
        {
            return true;
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
