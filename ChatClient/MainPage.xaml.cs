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
using DatabaseDLL;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public string UserName { get; set; }
        public MainPage(string username)
        {
            InitializeComponent();
            DataContext = new ViewModel.MainPageViewModel();
            UserName = username;
            // viewModel = DataContext as ViewModel.MainPageViewModel;
            //viewModel.Users.Add(new User(UserName));

            // Update the Label with the username
            UpdateUsernameLabel();
        }

        private void UpdateUsernameLabel()
        {
            // Assuming the Label has a name, for example, "UsernameLabel"
            UsernameLabel.Content = UserName;
           
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

        private void ChatroomList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If nothing is selected, just return
            if (e.AddedItems.Count == 0) return;

            // Get the selected chatroom
            var selectedChatroom = e.AddedItems[0] as Chatroom;

            if (selectedChatroom == null) return; // Return if the cast failed

            // Open the ChatRoom window. Pass the chatroom's name as a parameter
            var chatRoomWindow = new ChatRoom(selectedChatroom.Name, UserName);
            chatRoomWindow.Show();



            // Optionally deselect the chatroom in the list to prevent accidental re-openings
            var listView = sender as ListView;
            listView.SelectedItem = null;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshChatRooms();
        }

        private void RefreshChatRooms()
        {
            var viewModel = DataContext as ViewModel.MainPageViewModel;
            if (viewModel != null)
            {
                // Assuming you have a method to update the chat rooms list in your ViewModel
                viewModel.UpdateChatRoomsList();
            }
        }

    }
}
