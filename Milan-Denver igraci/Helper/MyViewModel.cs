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

           
            if (Kosarkasi.Count == 0)
            {
                Kosarkasi.Add(new Kosarkas { name = "Nikola", last_name = "Jokic", ImagePath = "C:\\Users\\HP\\Desktop\\jokic.jpg", brDresa = 15 });
                Kosarkasi.Add(new Kosarkas { name = "Aaron", last_name = "Gordon", ImagePath = "C:\\Users\\HP\\Desktop\\gordon.jpg", brDresa = 00 });
                Kosarkasi.Add(new Kosarkas { name = "Jamal", last_name = "Murray", ImagePath = "C:\\Users\\HP\\Desktop\\murray.jpg", brDresa = 27 });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
