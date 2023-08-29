using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDLL
{
    public class ChatDatabase
    {
        public HashSet<User> users;
        public HashSet<Chatroom> chatrooms;
        public HashSet<PrivateChatroom> privateChatrooms;

        public ChatDatabase()
        {
            users = new HashSet<User>();
            chatrooms = new HashSet<Chatroom>();
            privateChatrooms = new HashSet<PrivateChatroom>();
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

        public HashSet<PrivateChatroom> PrivateChatrooms
        {
            get { return privateChatrooms; }
            set { privateChatrooms = value; }
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

        public Boolean AddChatroom(string roomName)
        {

            return chatrooms.Add(new Chatroom(roomName));
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

        public Boolean AddPrivateChatroom(string roomName, User userOne, User userTwo)
        {
            return privateChatrooms.Add(new PrivateChatroom(roomName, userOne, userTwo));
        }

        public Boolean RemovePrivateChatroom(string roomName)
        {
            foreach (var room in privateChatrooms)
            {
                if (room.Name == roomName)
                {
                    return privateChatrooms.Remove(room);
                }
            }
            return false;
        }

        public Boolean PrivateChatroomExists(string roomName)
        {
            foreach (var room in privateChatrooms)
            {
                if (room.Name == roomName)
                {
                    return true;
                }
            }
            return false;
        }

        public User SearchUserByName(string name)
        {
            foreach (var user in users)
            {
                if (user.Username == name)
                {
                    return user;
                }
            }
            throw new KeyNotFoundException("User not found.");
        }

        public Chatroom SearchChatroomByName(string name)
        {
            foreach (var room in chatrooms)
            {
                if (room.Name == name)
                {
                    return room;
                }
            }
            throw new KeyNotFoundException("Chatroom not found.");
        }

        public Chatroom SearchPrivateChatroomByName(string name)
        {
            foreach (var room in privateChatrooms)
            {
                if (room.Name == name)
                {
                    return room;
                }
            }
            throw new KeyNotFoundException("Private chatroom not found.");
        }

        public string[] GetUserNames()
        {
            string[] names = new string[users.Count];
            int i = 0;
            foreach (var user in users)
            {
                names[i] = user.Username;
                i++;
            }
            return names;
        }

        public string[] GetChatroomNames()
        {
            string[] names = new string[chatrooms.Count];
            int i = 0;
            foreach (var room in chatrooms)
            {
                names[i] = room.Name;
                i++;
            }
            return names;
        }

        public string[] GetAllowedPrivateChatroomNames(User user)
        {
            List<String> allowedPrivateChatrooms = new List<String>();
            foreach (var room in privateChatrooms)
            {
                if (room.AllowedUsers.Contains(user))
                {
                    allowedPrivateChatrooms.Add(room.Name);
                }
            }
            return allowedPrivateChatrooms.ToArray();
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
