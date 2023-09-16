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
    /// Interaction logic for ChatRoom.xaml
    /// </summary>
    public partial class ChatRoom : Window
    {
        public string ChatroomName { get; set; }
        public string UserName { get; set; }
        public ChatRoom(string chatroomName, string userName)
        {
            InitializeComponent();

            ChatroomName = chatroomName;
            UserName = userName;
            ChatRoomName.Content = chatroomName;

            var database = DatabaseDLL.ChatDatabase.Instance;
            var chatroom = database.SearchChatroomByName(ChatroomName);
            if (chatroom != null)
            {
                chatroom.AddUser(UserName);
            }
            else
            {
                // Consider handling the scenario where the chatroom is not found.
                MessageBox.Show($"Chatroom '{ChatroomName}' not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

    }
}
