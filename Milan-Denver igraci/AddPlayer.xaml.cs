using Microsoft.Win32;
using Milan_Denver_igraci.Helper;
using Milan_Denver_igraci.Model;
using Notification.Wpf;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Milan_Denver_igraci
{
    public partial class AddPlayer : Window
    {
        private string SelectedImagePath;
        private Kosarkas existingKosarkas;
        private NotificationManager notificationManager2;

        public AddPlayer()
        {
            InitializeComponent();
            notificationManager2 = new NotificationManager();
        }

        public AddPlayer(Kosarkas kosarkas)
        {
            InitializeComponent();
            existingKosarkas = kosarkas;

            
            txtIme.Text = kosarkas.name;
            txtPrezime.Text = kosarkas.last_name;
            rtbDetalji.AppendText(kosarkas.Details);
            SelectedImagePath = kosarkas.ImagePath;
            SelectedImage.Source = new BitmapImage(new Uri(SelectedImagePath));
        }



        private void IzaberiSliku_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            openFileDialog.Title = "Select an image file";
            openFileDialog.InitialDirectory = @"C:\Users\HP\Desktop\Milan-Denver igraci\Slike";

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                SelectedImagePath = openFileDialog.FileName;
                SelectedImage.Source = new BitmapImage(new Uri(SelectedImagePath));

                
                SelectedImage.Source = new BitmapImage(new Uri(SelectedImagePath));
            }
        }


        private async void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrEmpty(txtIme.Text) || string.IsNullOrEmpty(txtPrezime.Text))
            {
                ShowToastNotification(new Toast("Upozorenje", "Popunite polja za igraca!", NotificationType.Warning));
                return;
            }

            
            if (string.IsNullOrEmpty(SelectedImagePath))
            {
                ShowToastNotification(new Toast("Upozorenje", "Izaberite sliku!", NotificationType.Warning));
                return;
            }

            
            string rtfContent = GetRtfContent(rtbDetalji);
            string rtfFilePath = Kosarkas.SaveRTFContent(rtfContent);

            
            MyViewModel myViewModelInstance = (MyViewModel)this.DataContext;

            if (existingKosarkas != null)
            {
               
                existingKosarkas.name = txtIme.Text;
                existingKosarkas.last_name = txtPrezime.Text;
                existingKosarkas.Details = rtfContent;
                existingKosarkas.ImagePath = SelectedImagePath;
            }
            else
            {
                
                Kosarkas newKosarkas = new Kosarkas
                {
                    name = txtIme.Text,
                    last_name = txtPrezime.Text,
                    Details = rtfContent,
                    ImagePath = SelectedImagePath
                };

                ShowToastNotification(new Toast("Uspeh", "Kosarkas je dodat u tim!", NotificationType.Success));             
                myViewModelInstance.Kosarkasi.Add(newKosarkas);
            }

            
            Kosarkas.SerializeKosarkas(myViewModelInstance.Kosarkasi.ToArray());
         
            RefreshDataGrid();
            await Task.Delay(800);       
            this.Close();
        }


        private string GetRtfContent(System.Windows.Controls.RichTextBox richTextBox)
        {
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);        
            MemoryStream memoryStream = new MemoryStream();
            textRange.Save(memoryStream, System.Windows.DataFormats.Rtf);

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }







        private void Bold_Click(object sender, RoutedEventArgs e)
{
    if (rtbDetalji.Selection.GetPropertyValue(FontWeightProperty) != DependencyProperty.UnsetValue &&
        rtbDetalji.Selection.GetPropertyValue(FontWeightProperty).Equals(FontWeights.Bold))
    {
        rtbDetalji.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Normal);
    }
    else
    {
        rtbDetalji.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
    }
}

private void Italic_Click(object sender, RoutedEventArgs e)
{
    if (rtbDetalji.Selection.GetPropertyValue(FontStyleProperty) != DependencyProperty.UnsetValue &&
        rtbDetalji.Selection.GetPropertyValue(FontStyleProperty).Equals(FontStyles.Italic))
    {
        rtbDetalji.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Normal);
    }
    else
    {
        rtbDetalji.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
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




        private void Underline_Click(object sender, RoutedEventArgs e)
{
    if (rtbDetalji.Selection.GetPropertyValue(Inline.TextDecorationsProperty) != DependencyProperty.UnsetValue &&
        rtbDetalji.Selection.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextDecorations.Underline))
    {
        rtbDetalji.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
    }
    else
    {
        rtbDetalji.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
    }
}



        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
           
            this.Close();
        }

        private void ChangeTextColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Color color = colorDialog.Color;
                System.Windows.Media.Color wpfColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                SolidColorBrush brush = new SolidColorBrush(wpfColor);

                rtbDetalji.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brush);

                
                ColorRectangle.Fill = brush;
            }
        }



        private void ChangeFont_Click(object sender, RoutedEventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Font selectedFont = fontDialog.Font;
                FontFamily fontFamily = new FontFamily(selectedFont.Name);
                double fontSize = selectedFont.SizeInPoints;

                rtbDetalji.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
                rtbDetalji.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
            }
        }

        private void IncreaseFontSize_Click(object sender, RoutedEventArgs e)
        {
            if (rtbDetalji.Selection.GetPropertyValue(TextElement.FontSizeProperty) is double size)
            {
                rtbDetalji.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, size * 1.1); 
            }
        }

        private void DecreaseFontSize_Click(object sender, RoutedEventArgs e)
        {
            if (rtbDetalji.Selection.GetPropertyValue(TextElement.FontSizeProperty) is double size)
            {
                rtbDetalji.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, size * 0.9); 
            }
        }

        private void UpdateWordCount()
        {
            
            int wordCount = 0;
            TextPointer start = rtbDetalji.Document.ContentStart;
            TextPointer end = rtbDetalji.Document.ContentEnd;
            TextRange textRange = new TextRange(start, end);

            string text = textRange.Text.Trim();

            
            if (text != "")
            {
                wordCount = text.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            }

           
            WordCountTextBlock.Text = $"Broj reči: {wordCount}";
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void rtbDetalji_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateWordCount();
        }


        public void ShowToastNotification(Toast toastNotification)
        {
            notificationManager2.Show(toastNotification.Title, toastNotification.Message, toastNotification.Type, "AddNotificationArea");
        }

        private void RefreshDataGrid()
        {
            
            Milan_Denver_igraci.SecondWindow secondWindow = System.Windows.Application.Current.Windows.OfType<Milan_Denver_igraci.SecondWindow>().FirstOrDefault();
            if (secondWindow != null)
            {
                secondWindow.dataGrid.ItemsSource = null;
                secondWindow.dataGrid.ItemsSource = ((MyViewModel)secondWindow.DataContext).Kosarkasi;
            }
        }




    }
}
