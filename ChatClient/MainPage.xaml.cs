using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using IChatServerInterfaceDLL;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public string Username { get; private set; }
        private IChatServerInterface chatServer;
        public static readonly string chatUrl = "net.tcp://localhost:8100/ChatService";
        public MainPage(string username)
        {
            InitializeComponent();

    

            // Create connection to chatroom server
            NetTcpBinding tcp = new NetTcpBinding();
            ChannelFactory<IChatServerInterface> chatroomChannelFactory = new ChannelFactory<IChatServerInterface>(tcp, chatUrl);
            chatServer = chatroomChannelFactory.CreateChannel();


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
        private async void CreateChatroomButton_Click(object sender, RoutedEventArgs e)
        {
            //var await Task.Run(() => ChatServer = (DataContext as ViewModel.MainPageViewModel)._await Task.Run(() => ChatServer; // Access the await Task.Run(() => ChatServer from ViewModel

            var dialog = new ChatroomNameDialog();
            if (dialog.ShowDialog() == true)
            {
                var newChatroomName = dialog.ChatroomName;
                if (string.IsNullOrWhiteSpace(newChatroomName))
                {
                    MessageBox.Show("Please enter a valid chatroom name.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (await Task.Run(() => chatServer.ChatroomExists(newChatroomName)))
                {
                    MessageBox.Show($"Chatroom '{newChatroomName}' already exists. Please choose a different name.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //Add to db
                await Task.Run(() => chatServer.AddChatroom(newChatroomName));
                
                // Update GUI
                var viewModel = DataContext as ViewModel.MainPageViewModel;
                viewModel.Chatrooms.Add(await Task.Run(() => chatServer.SearchChatroomByName(newChatroomName)));
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Delete active User
            chatServer.RemoveUser(Username);
            
            // Create an instance of the Login window
            MainWindow loginWindow = new MainWindow();

            // Show the Login window
            loginWindow.Show();

            // Close the current window
            this.Close();
        }

        private async void CreateRefreshButton_Click(object sender, RoutedEventArgs e)
        {
           // var await Task.Run(() => ChatServer = (DataContext as ViewModel.MainPageViewModel)._await Task.Run(() => ChatServer;


            var viewModel = DataContext as ViewModel.MainPageViewModel;
            var chatRoomList = await Task.Run(() => chatServer.ListChatRooms());
            viewModel.Chatrooms.Clear();

            foreach (var chat in chatRoomList)
            {
                viewModel.Chatrooms.Add(chat);
            }

        }
        private void ChatroomListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var selectedChatroom = e.AddedItems[0] as Chatroom;
                if (selectedChatroom != null)
                {
                    // Add the user to the chatroom
                    chatServer.AddUserToChatroom(Username, selectedChatroom.Name);

                    // Navigate to ChatRoom.xaml page and pass the chatroom name as argument
                    ChatRoom chatRoomWindow = new ChatRoom(selectedChatroom.Name, Username);
                    chatRoomWindow.Show();
                    this.Close();
                }
            }
        }
    }
}
