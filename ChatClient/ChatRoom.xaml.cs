using IChatServerInterfaceDLL;
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

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for ChatRoom.xaml
    /// </summary>
    public partial class ChatRoom : Window
    {
        private IChatServerInterface chatServer;
        public static readonly string chatUrl = "net.tcp://localhost:8100/ChatService";
        public string ChatroomName { get; private set; }
        public ChatRoom(string chatroomName)
        {
            InitializeComponent();
            ChannelFactory<IChatServerInterface> channelFact;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/ChatService";
            channelFact = new ChannelFactory<IChatServerInterface>(tcp, URL);
            chatServer = channelFact.CreateChannel();
            DataContext = new ViewModel.ChatRoomViewModel();

            // Store the chatroom name
            ChatroomName = chatroomName;

            GetChatRoomName.Content = ChatroomName;

    
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

        private async void RefreshUsersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Fetch the list of active users in the current chat room
                var usersList = await Task.Run(() => chatServer.ListUsersInChatroom(ChatroomName));

                var viewModel = DataContext as ViewModel.ChatRoomViewModel;
                if (viewModel != null)
                {
                    viewModel.Users.Clear();
                    foreach (var user in usersList)
                    {
                        viewModel.Users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or display it to the user
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
