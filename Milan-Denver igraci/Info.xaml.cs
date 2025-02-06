using Milan_Denver_igraci.Model;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Milan_Denver_igraci
{
    /// <summary>
    /// Interaction logic for Info.xaml
    /// </summary>
    public partial class Info : Window
    {
        public Info()
        {
            InitializeComponent();
        }

       
        public void PrikaziInformacijeOKosarkasu(Kosarkas kosarkas)
        {
            
            txtIme.Text = kosarkas.name;
            txtPrezime.Text = kosarkas.last_name;

            
            rtbDetalji.Document.Blocks.Clear();
            string rtfContent = kosarkas.Details;

            if (!string.IsNullOrEmpty(rtfContent))
            {
                TextRange textRange = new TextRange(rtbDetalji.Document.ContentStart, rtbDetalji.Document.ContentEnd);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(rtfContent)))
                {
                    textRange.Load(ms, DataFormats.Rtf);
                }
            }

           
            if (!string.IsNullOrEmpty(kosarkas.ImagePath))
            {
                SelectedImage.Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(kosarkas.ImagePath));
            }
        }


        public void SetRtfContent(string rtfContent)
        {
            if (string.IsNullOrEmpty(rtfContent))
            {
                
                Console.WriteLine("RTF sadržaj je null ili prazan string.");
                return;
            }

            TextRange range = new TextRange(rtbDetalji.Document.ContentStart, rtbDetalji.Document.ContentEnd);
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(rtfContent)))
            {
                range.Load(stream, System.Windows.DataFormats.Rtf); 
            }
        }


 
        private void Zatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
