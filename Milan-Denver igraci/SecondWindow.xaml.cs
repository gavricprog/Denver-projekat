using Milan_Denver_igraci.Helper;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Milan_Denver_igraci.Model;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Milan_Denver_igraci
{
    /// <summary>
    /// Interaction logic for SecondWindow.xaml
    /// </summary>
   public partial class SecondWindow : Window

{

        private NotificationManager notificationManager3;
        public IEnumerable<Kosarkas> GetDataGridItems()
        {
            return dataGrid.ItemsSource as IEnumerable<Kosarkas>;
        }
        public User CurrentUser { get; set; }

    public SecondWindow(User currentUser)
    {
        InitializeComponent();
        DataContext = new MyViewModel();
        CurrentUser = currentUser;
        notificationManager3 = new NotificationManager();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void Remove_click(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.Forms.MessageBox.Show(
                "Da li ste sigurni da želite obrisati označene igrače?",
                "Potvrda brisanja",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                var viewModel = (MyViewModel)DataContext;

                // 🔥 UZMI SAMO čekirane (te brišemo)
                var zaBrisanje = viewModel.Kosarkasi
                                         .Where(k => k.IsSelected)
                                         .ToList();

                // 🔥 OBRIŠI ih iz kolekcije
                foreach (var igrac in zaBrisanje)
                {
                    viewModel.Kosarkasi.Remove(igrac);
                }

                ShowToastNotification(new Toast("Uspeh!", "Brisanje se izvršilo", NotificationType.Success));

                // 🔥 sačuvaj stanje
                Kosarkas.SerializeKosarkas(viewModel.Kosarkasi.ToArray());

                RefreshDataGrid();
            }
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



        private void Add_click(object sender, RoutedEventArgs e)
        {
            MyViewModel viewModel = (MyViewModel)DataContext;

            
            AddPlayer addPlayer = new AddPlayer();
            addPlayer.DataContext = viewModel; 
            addPlayer.Show();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            
            if (CurrentUser.Role == UserRole.Admin)
            {
                
                Kosarkas selectedPlayer = (Kosarkas)dataGrid.SelectedItem;
                if (selectedPlayer != null)
                {
                    AddPlayer addPlayer = new AddPlayer(selectedPlayer);
                    addPlayer.DataContext = DataContext;
                 
                    string rtfContent = selectedPlayer.Details;

                    addPlayer.SetRtfContent(rtfContent);
                    addPlayer.ShowDialog();
                }
                else
                {
                    ShowToastNotification(new Toast("Molimo izaberite igrača iz liste za izmenu.", "Upozorenje", NotificationType.Warning));
                }
            }
            else
            {
                
                Info infoWindow = new Info();
                Kosarkas selectedPlayer = (Kosarkas)dataGrid.SelectedItem;

                if (selectedPlayer != null)
                {
                    infoWindow.PrikaziInformacijeOKosarkasu(selectedPlayer);
                }
                else
                {
                    ShowToastNotification(new Toast("Molimo izaberite igrača iz liste za izmenu.", "Upozorenje", NotificationType.Warning));
                    return;
                }

                infoWindow.ShowDialog();
            }
        }





        public void ShowToastNotification(Toast toastNotification)
        {
            notificationManager3.Show(toastNotification.Title, toastNotification.Message, toastNotification.Type, "SecondNotificationArea");
        }


    }
}
