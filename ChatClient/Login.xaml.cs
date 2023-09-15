using ChatServer;
using DatabaseDLL;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IChatServerInterface channel;

        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<IChatServerInterface> channelFact;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/ChatService";
            channelFact = new ChannelFactory<IChatServerInterface>(tcp, URL);
            channel = channelFact.CreateChannel();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //HelloLabel.Content = "Getting User...";

            string inputUserName = UserName.Text;


            User user = await Task.Run(() => GetUserByUsername(inputUserName));
            UpdateGui(user);

            


        }

        private User GetUserByUsername(string username)
        {
            User user;
            try
            {
                //string username = "User 1";
                user = channel.SearchUserByName(username);
            }
            catch (Exception ex) when (ex is FaultException<KeyNotFoundException> || ex is KeyNotFoundException || ex is FaultException)
            {
                user = new User("");
            }
            catch (Exception ex) when (ex is CommunicationException)
            {
                user = new User("");
            }
            return user;
        }

        private void UpdateGui(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                
                MainPage mainPage = new MainPage();
                mainPage.Show();
            }
            else
            {
                MessageBox.Show("There is a same username!");
            }
        }
    }
}