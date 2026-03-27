using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Milan_Denver_igraci.Model;

namespace Milan_Denver_igraci.Helper
{
    public class MyViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Kosarkas> _kosarkasi;
        public ObservableCollection<Kosarkas> Kosarkasi
        {
            get { return _kosarkasi; }
            set
            {
                _kosarkasi = value;
                OnPropertyChanged(nameof(Kosarkasi));
            }
        }

        public MyViewModel()
        {
            Kosarkasi = Kosarkas.DeserializeKosarkas();

           
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
