﻿using System;
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
using IChatServerInterfaceDLL;

namespace ChatClient {
    /// <summary>
    /// Interaction logic for PrivateMessage.xaml
    /// </summary>
    public partial class PrivateMessage : Window {
        public string ReceiverUsername { get; private set; }
        public string SenderUsername { get; private set; }

        private IChatServerInterface chatServer; //TODO: delete if not needed
        public PrivateMessage(string receiverUsername, string senderUsername, IChatServerInterface chatServerInterface) {
            InitializeComponent();
            ReceiverUsername = receiverUsername;
            SenderUsername = senderUsername;

            // Initialize the chat server interface
            chatServer = chatServerInterface;

            // TODO: You may need a view model depending on how you choose to do this
            DataContext = new ViewModel.PrivateMessageViewModel();
        }

        /// <summary>
        /// Clears text when in focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageTextBox_GotFocus(object sender, RoutedEventArgs e) {
            if (MessageTextBox.Text == "Type your message here...") {
                MessageTextBox.Text = "";
            }
        }
        /// <summary>
        /// Lets user drag window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                this.DragMove();
            }
        }
        /// Close the ChatRoom window.
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // TODO: Implement your logic to send the private message here
        private void SendMessageButton_Click(object sender, RoutedEventArgs e) {
            string messageText = MessageTextBox.Text;

            if (string.IsNullOrWhiteSpace(messageText)) {
                MessageBox.Show("Please enter a valid message.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Add the message to the database
            User user = chatServer.SearchUserByName(SenderUsername);
            Message message = new Message(user, messageText);
            await Task.Run(() => chatServer.AddMessageToPrivateChatroom(SenderUsername, ReceiverUsername, message));

            // Update the ViewModel's Messages collection
            var viewModel = DataContext as ViewModel.PrivateChatRoomViewModel;
            viewModel?.Messages.Add(message);

            // Clears the text box
            MessageTextBox.Text = "";
        }

        // TODO: Test private message refresh
        /// <summary>
        /// Refresh the list of active users and messages in the chatroom.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RefreshUsersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Fetch the list of active users in the current chat room
                //var usersList = await Task.Run(() => chatServer.ListUsersInChatroom(ChatroomName));
                var messageList = await Task.Run(() => chatServer.ListMessagesInPrivateChatroom(ChatroomName));
                var viewModel = DataContext as ViewModel.PrivateChatRoomViewModel;
                if (viewModel != null)
                {
                    //viewModel.Users.Clear();
                    //foreach (var user in usersList)
                    //{
                    //    viewModel.Users.Add(user);
                    //}
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
    }
}
