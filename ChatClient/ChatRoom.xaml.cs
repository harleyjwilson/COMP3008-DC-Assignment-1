﻿using DatabaseDLL;
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
    /// Interaction logic for ChatRoom.xaml
    public partial class ChatRoom : Window
    {
        private IChatServerInterface chatServer;
        public static readonly string chatUrl = "net.tcp://localhost:8100/ChatService";
        public string ChatroomName { get; private set; }
        public string Username { get; private set; }
        public ChatRoom(string chatroomName, string username)
        {
            InitializeComponent();

            // Initialize connection to the chat server
            ChannelFactory<IChatServerInterface> channelFact;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/ChatService";
            channelFact = new ChannelFactory<IChatServerInterface>(tcp, URL);
            chatServer = channelFact.CreateChannel();
            DataContext = new ViewModel.ChatRoomViewModel();

            // Store the chatroom name and username
            ChatroomName = chatroomName;
            Username = username;

            GetChatRoomName.Content = ChatroomName;
            UsernameLabel.Content = Username;
    
        }
        /// Handle the mouse down event to enable dragging of the window.
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        /// Minimize the window.
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        /// Toggle between window's maximized and normal states.
        private void WindowStateButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
                this.WindowState = WindowState.Maximized;
        }

        /// Close the ChatRoom window.
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            chatServer.RemoveUserFromChatroom(Username, ChatroomName);
            this.Close();

        }
        /// Send a message to the chatroom.
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {

            var messageText = MessageTyping.Text;

            if (string.IsNullOrWhiteSpace(messageText))
            {
                MessageBox.Show("Please enter a valid text.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Add the message to the database
            User user = chatServer.SearchUserByName(Username);
            Message message = new Message(user, messageText);
            await Task.Run(() => chatServer.AddMessageToChatroom(ChatroomName, message));

            // Update the ViewModel's Messages collection
            var viewModel = DataContext as ViewModel.ChatRoomViewModel;
            viewModel?.Messages.Add(message);


        }
        /// Refresh the list of active users and messages in the chatroom.
        private async void RefreshUsersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Fetch the list of active users in the current chat room
                var usersList = await Task.Run(() => chatServer.ListUsersInChatroom(ChatroomName));
                var messageList = await Task.Run(() => chatServer.ListMessagesInChatroom(ChatroomName));
                var viewModel = DataContext as ViewModel.ChatRoomViewModel;
                if (viewModel != null)
                {
                    viewModel.Users.Clear();
                    foreach (var user in usersList)
                    {
                        viewModel.Users.Add(user);
                    }
                    viewModel.Messages.Clear();
                    foreach (var message in messageList)
                    {
                        viewModel.Messages.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or display it to the user
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        /// Handle the file sharing button click to upload a file.

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            //This is for file sharing button.
        }

    }
}
