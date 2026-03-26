using Notification.Wpf; 
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using Milan_Denver_igraci.Helper;
using Milan_Denver_igraci.Model;


namespace Milan_Denver_igraci
{
    public partial class MainWindow : Window
    {
        private NotificationManager notificationManager;
        private User[] users = new User[]
        {
            new User { Username = "milan", Password = "123", Role = UserRole.Admin },
            new User { Username = "marko", Password = "123", Role = UserRole.Visitor }
        };


        private void UserTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserTextBox.Text == "Username")
            {
                UserTextBox.Text = "";
            }
        }

        private void UserTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserTextBox.Text))
            {
                UserTextBox.Text = "Username";
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            SerializeUsers(users);
            notificationManager = new NotificationManager();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private async void Submit(object sender, RoutedEventArgs e)
        {
            if (UserTextBox != null && !string.IsNullOrEmpty(UserTextBox.Text) &&
                PasswordPasswordBox != null && !string.IsNullOrEmpty(PasswordPasswordBox.Password))
            {
                string username = UserTextBox.Text;
                string password = PasswordPasswordBox.Password;

                User user = Array.Find(users, u => u.Username == username && u.Password == password);

                if (user != null)
                {
                    if (user.Role == UserRole.Admin || user.Role == UserRole.Visitor)
                    {
                        ShowToastNotification(new Toast("Uspešna prijava", "Dobrodošli nazad!", NotificationType.Success));
                        await Task.Delay(750);
                        SecondWindow secondWindow = new SecondWindow(user);
                        secondWindow.Show();
                        if (user.Role == UserRole.Visitor)
                        {
                            secondWindow.AddButton.Visibility = Visibility.Hidden;
                            secondWindow.RemoveButton.Visibility = Visibility.Hidden;
                        }

                        this.Close();
                    }
                }
                else
                {
                    ShowToastNotification(new Toast("Neuspešna prijava", "Pogrešno korisničko ime ili lozinka.", NotificationType.Error));
                }
            }
            else
            {
                ShowToastNotification(new Toast("Neuspešna prijava", "Molimo unesite korisničko ime i lozinku.", NotificationType.Error));
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SerializeUsers(User[] users)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(User[]));
                using (var stream = new FileStream("users.xml", FileMode.Create))
                {
                    serializer.Serialize(stream, users);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom serijalizacije korisnika: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ShowToastNotification(Toast toastNotification)
        {
            notificationManager.Show(toastNotification.Title, toastNotification.Message, toastNotification.Type, "WindowNotificationArea");
        }

    }
}
