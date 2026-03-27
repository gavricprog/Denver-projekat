using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace Milan_Denver_igraci.Model
{
    public class Kosarkas : INotifyPropertyChanged
    {
        public string name { get; set; }
        public string last_name { get; set; }
        public string RtfFilePath { get; set; }

        public string HyperlinkText
        {
            get { return $"{name} {last_name}"; }
        }

        public string ImagePath { get; set; }

        private string _details;
        public string Details
        {
            get { return _details; }
            set
            {
                if (_details != value)
                {
                    _details = value;
                    OnPropertyChanged(nameof(Details));
                }
            }
        }

        private int _brDresa;
        public int brDresa
        {
            get { return _brDresa; }
            set
            {
                if (_brDresa != value)
                {
                    _brDresa = value;
                    OnPropertyChanged(nameof(brDresa));
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public DateTime dateTime { get; set; }

        public Kosarkas()
        {
            dateTime = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 🔥 RTF SAVE (fix putanja)
        public static string SaveRTFContent(string rtfContent)
        {
            try
            {
                string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RTF");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + ".rtf";
                string fullPath = Path.Combine(folder, fileName);

                File.WriteAllText(fullPath, rtfContent);

                return fullPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom spremanja RTF sadržaja: " + ex.Message);
                return "";
            }
        }

        // 🔥 SERIALIZE (FIX PATH + IME FAJLA)
        public static void SerializeKosarkas(Kosarkas[] kosarkasi)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kosarkasi.xml");

                var serializer = new XmlSerializer(typeof(Kosarkas[]));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    serializer.Serialize(stream, kosarkasi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom serijalizacije: " + ex.Message);
            }
        }

        // 🔥 DESERIALIZE (FIX SVE)
        public static ObservableCollection<Kosarkas> DeserializeKosarkas()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kosarkasi.xml");

                if (!File.Exists(path) || new FileInfo(path).Length == 0)
                    return new ObservableCollection<Kosarkas>();

                var serializer = new XmlSerializer(typeof(Kosarkas[]));

                using (var stream = new FileStream(path, FileMode.Open))
                {
                    var data = (Kosarkas[])serializer.Deserialize(stream);
                    return new ObservableCollection<Kosarkas>(data);
                }
            }
            catch (Exception)
            {
                return new ObservableCollection<Kosarkas>();
            }
        }
    }
}