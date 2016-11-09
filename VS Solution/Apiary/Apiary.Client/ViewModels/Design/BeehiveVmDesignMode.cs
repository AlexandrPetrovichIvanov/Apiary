namespace Apiary.Client.ViewModels.Design
{
    using System;
    using System.ComponentModel;

    using Apiary.Interfaces;

    public class BeehiveVmDesignMode : IBeehiveVM
    {
        internal BeehiveVmDesignMode(
            IBeehiveState state)
        {
            this.BeehiveNumber = state.BeehiveNumber;
            this.BeesInsideCount = state.BeesInsideCount;
            this.GuardsCount = state.GuardsCount;
            this.HoneyCount = state.HoneyCount;
            this.QueensCount = state.QueensCount;
            this.TotalBeesCount = state.TotalBeesCount;
            this.WorkersCount = state.WorkersCount;
        }

        public int BeehiveNumber { get; private set; }

        public int BeesInsideCount { get; private set; }

        public int GuardsCount { get; private set; }

        public int HoneyCount { get; private set; }

        public int QueensCount { get; private set; }

        public int TotalBeesCount { get; private set; }

        public int WorkersCount { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
