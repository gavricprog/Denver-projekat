using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Serialization;
using System.Windows.Controls;

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

        public Kosarkas() { dateTime = DateTime.Now; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static string SaveRTFContent(string rtfContent)
        {
            string rtfFilePath = ""; 
            try
            {
               
                string directoryPath = @"C:\Users\HP\Desktop\Milan-Denver igraci";
                string fileName = Guid.NewGuid().ToString() + ".rtf";
                rtfFilePath = Path.Combine(directoryPath, fileName);

               
                File.WriteAllText(rtfFilePath, rtfContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom spremanja RTF sadržaja: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return rtfFilePath;
        }



        public static void SerializeKosarkas(Kosarkas[] kosarkasi)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Kosarkas[]));
                using (var stream = new FileStream("kosarkas.xml", FileMode.Create))
                {
                    serializer.Serialize(stream, kosarkasi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom serijalizacije kosarkasa: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static ObservableCollection<Kosarkas> DeserializeKosarkas()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<Kosarkas>));
                using (var stream = new FileStream("kosarkas.xml", FileMode.Open))
                {
                    return (ObservableCollection<Kosarkas>)serializer.Deserialize(stream);
                }
            }
            catch (FileNotFoundException)
            {
                
                return new ObservableCollection<Kosarkas>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom deserijalizacije kosarkasa: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return new ObservableCollection<Kosarkas>();
            }
        }
    }
}
