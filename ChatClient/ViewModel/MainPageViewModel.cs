using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseDLL;

namespace ChatClient.ViewModel
{
    public class MainPageViewModel
    {
        public ChatDatabase _database;

        public ObservableCollection<Chatroom> Chatrooms { get; set; }
        public ObservableCollection<User> Users { get; set; }

        public MainPageViewModel()
        {
            _database = ChatDatabase.Instance;
            Chatrooms = new ObservableCollection<Chatroom>(_database.Chatrooms.ToList());
            Users = new ObservableCollection<User>(_database.Users.ToList());
        }

        // Method to refresh chat rooms list from the database
        public void UpdateChatRoomsList()
        {
            // Fetch the latest chat rooms from the database
            var updatedChatRooms = _database.Chatrooms.ToList();

            // Clear the existing ObservableCollection and fill it with the updated list
            Chatrooms.Clear();
            foreach (var room in updatedChatRooms)
            {
                Chatrooms.Add(room);
            }
        }
    }
}
