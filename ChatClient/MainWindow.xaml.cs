using ChatServer;
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
            HelloLabel.Content = "Hello, " + channel.GetName() + "!!!";
        }
    }
}
