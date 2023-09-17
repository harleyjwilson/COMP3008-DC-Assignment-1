using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public string Username { get; private set; }
        public MainPage(string username)
        {
            InitializeComponent();
            DataContext = new ViewModel.MainPageViewModel();
            Username = username;
            GetUsername.Content = Username;
            MessageBox.Show($"Received username: {Username}");
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void WindowStateButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
                this.WindowState = WindowState.Maximized;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
        // Add the event handler here:
        private void CreateChatroomButton_Click(object sender, RoutedEventArgs e)
        {
            var database = (DataContext as ViewModel.MainPageViewModel)._database; // Access the database from ViewModel

            var dialog = new ChatroomNameDialog();
            if (dialog.ShowDialog() == true)
            {
                var newChatroomName = dialog.ChatroomName;
                if (string.IsNullOrWhiteSpace(newChatroomName))
                {
                    MessageBox.Show("Please enter a valid chatroom name.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (database.ChatroomExists(newChatroomName))
                {
                    MessageBox.Show($"Chatroom '{newChatroomName}' already exists. Please choose a different name.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                database.AddChatroom(newChatroomName);
                var viewModel = DataContext as ViewModel.MainPageViewModel;
                viewModel.Chatrooms.Add(database.SearchChatroomByName(newChatroomName));
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the Login window
            MainWindow loginWindow = new MainWindow();

            // Show the Login window
            loginWindow.Show();

            // Close the current window
            this.Close();
        }
    }
}
