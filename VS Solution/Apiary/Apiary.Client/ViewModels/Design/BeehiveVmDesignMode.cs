namespace Apiary.Client.ViewModels.Design
{
    using System;
    using System.ComponentModel;

    using Apiary.Interfaces.Events;

    public class BeehiveVmDesignMode : IBeehiveVM
    {
        public int BeehiveNumber
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int BeesInsideCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int GuardsCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int HoneyCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int QueensCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int TotalBeesCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int WorkersCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<BeehiveStateChangedEventArgs> StateChanged;
    }
}
