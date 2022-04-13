using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProFIT
{
    public class FitnesData : INotifyPropertyChanged
    {
        private float weight;
        private float og;
        private float ot;
        private float ob;
        private float oBed;
        private float oPl;

        public int Id { get; set; }

        public float Weight 
        { 
            get { return weight; } 
            set { weight = value; OnPropertyChanged("Weight"); } 
        }

        public float Og
        {
            get { return og; }
            set { og = value; OnPropertyChanged("Og"); }
        }

        public float Ot
        {
            get { return ot; }
            set { ot = value; OnPropertyChanged("Ot"); }
        }

        public float Ob
        {
            get { return ob; }
            set { ob = value; OnPropertyChanged("Ob"); }
        }

        public float Obed
        {
            get { return oBed; }    
            set { oBed = value; OnPropertyChanged("Ob"); }
        }

        public float Opl
        {
            get { return oPl; } 
            set { oPl = value; OnPropertyChanged("Opl"); }
        }

        public string Date { get; set; } = DateTime.Today.ToString("d");

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
