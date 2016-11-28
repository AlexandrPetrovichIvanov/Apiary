namespace Apiary.Client.ViewModels.Work
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    using Apiary.Client.XmlStates;
    using Apiary.Client.Commands;
    using Apiary.Client.Mvvm;
    using Apiary.Interfaces;
    using Apiary.Utilities;

    /// <summary>
    /// Модель представления пасеки.
    /// </summary>
    public class ApiaryVM : ViewModelBase, IApiaryVM
    {
        /// <summary>
        /// Признак того, что пасека работает.
        /// </summary>
        private bool isWorking;

        /// <summary>
        /// Сама пасека.
        /// </summary>
        private readonly IApiary apiary;

        /// <summary>
        /// Количество собранного мёда, взятое из сохраненных
        /// в кэше данных, а не из реальной пасеки.
        /// </summary>
        private long tempHoneyCount;

        /// <summary>
        /// Создать модель представления пасеки.
        /// </summary>
        public ApiaryVM()
        {            
            this.Refresh(RefreshApiaryVmOptions.ShowTempSavedState);
            this.apiary = ServiceLocator.Instance.GetService<IApiary>();
            this.InitializeCommands();
        }

        /// <summary>
        /// Модели представления всех ульев на пасеке.
        /// </summary>
        public ObservableCollection<IBeehiveVM> Beehives { get; }
            = new ObservableCollection<IBeehiveVM>();

        /// <summary>
        /// Общее количество собранного мёда.
        /// </summary>
        public long HoneyCount =>
            this.isWorking ? this.apiary.HoneyCount : this.tempHoneyCount;

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
        public SimpleCommand StartCommand { get; private set; }

        /// <summary>
        /// Команда остановки работы пасеки.
        /// </summary>
        public SimpleCommand StopCommand { get; private set; }

        /// <summary>
        /// Команда сбора мёда.
        /// </summary>
        public SimpleCommand HarvestHoneyCommand { get; private set; }

        /// <summary>
        /// Команда запуска работы пасеки.
        /// </summary>
        ICommand IApiaryVM.StartCommand => this.StartCommand;

        /// <summary>
        /// Команда остановки работы пасеки.
        /// </summary>
        ICommand IApiaryVM.StopCommand => this.StopCommand;

        /// <summary>
        /// Команда сбора мёда.
        /// </summary>
        ICommand IApiaryVM.HarvestHoneyCommand => this.HarvestHoneyCommand;

        /// <summary>
        /// Установить команды view-модели пасеки.
        /// </summary>
        private void InitializeCommands()
        {
            this.StartCommand = new StartCommand(this.Start, this.CanStart);
            this.StopCommand = new StopCommand(this.Stop, this.CanStop);
            this.HarvestHoneyCommand = new HarvestHoneyCommand(
                this.HarvestHoney,
                this.CanHarvestHoney);
        }

        /// <summary>
        /// Запуск пасеки.
        /// </summary>
        private void Start()
        {
            this.apiary.Start(ApiaryXmlState.LoadState());
            this.Refresh(RefreshApiaryVmOptions.ShowActualApiary); 
            this.isWorking = true;   
            this.RefreshCanExecuteCommands();
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
            IApiaryState lastState = this.apiary.Stop();
            ApiaryXmlState xmlLastState = ApiaryXmlState.CopyFrom(lastState);
            xmlLastState.SaveInCache();
            this.isWorking = false;
            this.RefreshCanExecuteCommands();
        }

        /// <summary>
        /// Признак возможности остановки пасеки.
        /// </summary>
        /// <returns>True - пасеку можно остановить.</returns>
        private bool CanStop()
        {
            return this.isWorking;
        }

        /// <summary>
        /// Собрать мёд.
        /// </summary>
        private void HarvestHoney()
        {
            this.apiary.CollectHoney();
            this.RaisePropertyChanged(nameof(this.HoneyCount));
            this.RefreshCanExecuteCommands();
        }

        /// <summary>
        /// Признак возможности сбора мёда.
        /// </summary>
        /// <returns>True - можно собрать мёд.</returns>
        private bool CanHarvestHoney()
        {
            return this.isWorking
                && this.Beehives.Any(bh => bh.HoneyCount > 0);
        }

        /// <summary>
        /// Обновить визуальное представление пасеки.
        /// </summary>
        /// <param name="options">Опции обновления view-модели пасеки.</param>
        private void Refresh(RefreshApiaryVmOptions options)
        {
            IApiaryState state = this.GetStateForRefresh(options);

            if (options == RefreshApiaryVmOptions.ShowTempSavedState)
            {
                this.tempHoneyCount = state.HoneyCount;
            }

            this.RefreshBeehives(state.BeehiveStates);
            this.RaiseMainPropertiesChanged();
        }

        /// <summary>
        /// Получить/выбрать состояние пасеки, в соответствии с которым
        /// будет обновляться view-модель пасеки.
        /// </summary>
        /// <param name="options">Опции обновления view-модели пасеки.</param>
        /// <returns>Состояние пасеки.</returns>
        private IApiaryState GetStateForRefresh(RefreshApiaryVmOptions options)
        {
            IApiaryState result;

            switch (options)
            {
                case RefreshApiaryVmOptions.ShowActualApiary:
                    result = this.apiary;
                    break;
                case RefreshApiaryVmOptions.ShowTempSavedState:
                    result = ApiaryXmlState.LoadState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(options),
                        "Передано непредусмотренное значение опций обновления view-модели пасеки");
            }

            return result;
        }

        /// <summary>
        /// Обновить коллекцию view-моделей ульев.
        /// </summary>
        private void RefreshBeehives(IEnumerable<IBeehiveState> beehiveStates)
        {
            this.ClearBeehives();
            this.FillBeehives(beehiveStates);                      
        }

        /// <summary>
        /// Очистить коллекцию view-моделей ульев.
        /// </summary>
        private void ClearBeehives()
        {
            foreach (IBeehiveVM beehiveVm in this.Beehives)
            {
                beehiveVm.PropertyChanged -= this.BeehiveVmOnPropertyChanged;
                beehiveVm.StopRaisingEvents();
            }

            this.Beehives.Clear();
        }

        /// <summary>
        /// Заполнить коллекцию view-моделей ульев.
        /// </summary>
        /// <param name="states">Состояния ульев.</param>
        private void FillBeehives(IEnumerable<IBeehiveState> states)
        {
            foreach (IBeehiveState beehiveState in states)
            {
                this.Beehives.Add(new BeehiveVM(beehiveState));
            }

            foreach (IBeehiveVM beehiveVm in this.Beehives)
            {
                beehiveVm.PropertyChanged += this.BeehiveVmOnPropertyChanged;
            }  
        }

        /// <summary>
        /// Обработчик события изменения свойств ульев.
        /// </summary>
        /// <param name="sender">Источник события (улей).</param>
        /// <param name="args">Аргументы события.</param>
        private void BeehiveVmOnPropertyChanged(
            object sender, 
            PropertyChangedEventArgs args)
        {
            IBeehiveVM beehive;

            if (args.PropertyName == nameof(beehive.BeesTotalCount))
            {
                this.RaisePropertyChanged(nameof(this.BeesCount));
            }
        }

        /// <summary>
        /// Оповестить подписчиков об изменении основных свойств пасеки.
        /// </summary>
        private void RaiseMainPropertiesChanged()
        {
            this.RaisePropertyChanged(nameof(this.HoneyCount));
            this.RaisePropertyChanged(nameof(this.BeehivesCount));
            this.RaisePropertyChanged(nameof(this.BeesCount));
        }

        /// <summary>
        /// Обновить доступность всех команд.
        /// </summary>
        private void RefreshCanExecuteCommands()
        {
            this.StartCommand.RefreshCanExecute();
            this.StopCommand.RefreshCanExecute();
            this.HarvestHoneyCommand.RefreshCanExecute();
        }
    }
}