using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDLL
{
    public class ChatDatabase
    {
        public HashSet<User> users;
        public HashSet<Chatroom> chatrooms;

        public ChatDatabase()
        {
            users = new HashSet<User>();
            chatrooms = new HashSet<Chatroom>();
        }

        public HashSet<User> Users
        {
            get { return users; }
            set { users = value; }
        }

        public HashSet<Chatroom> Chatrooms
        {
            get { return chatrooms; }
            set { chatrooms = value; }
        }

        public Boolean AddUser(string username)
        {
            return users.Add(new User(username));
        }

        public Boolean RemoveUser(string username)
        {
            return users.Remove(new User(username));
        }

        public Boolean UserExists(string username)
        {
            return users.Contains(new User(username));
        }

        public Boolean AddChatroom(string username)
        {

            return chatrooms.Add(new Chatroom(username));
        }

        public Boolean RemoveChatroom(string roomName)
        {
            foreach (var room in chatrooms)
            {
                if (room.Name == roomName)
                {
                    return chatrooms.Remove(room);
                }
            }
            return false;
        }

        public Boolean ChatroomExists(string roomName)
        {
            foreach (var room in chatrooms)
            {
                if (room.Name == roomName)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is ChatDatabase database &&
                   EqualityComparer<HashSet<User>>.Default.Equals(users, database.users) &&
                   EqualityComparer<HashSet<Chatroom>>.Default.Equals(chatrooms, database.chatrooms);
        }

        public override int GetHashCode()
        {
            int hashCode = -1206041640;
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<User>>.Default.GetHashCode(users);
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<Chatroom>>.Default.GetHashCode(chatrooms);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
