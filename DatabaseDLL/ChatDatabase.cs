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
        private const int NUMBER_OF_USERS = 12; //number of user in database
        private const int NUMBER_OF_CHATROOMS = 4; //number of chatrooms
        public SortedSet<User> users;
        public SortedSet<Chatroom> chatrooms;
        public List<PrivateChatroom> privateChatrooms;

        public ChatDatabase()
        {
            // Comparer implementation for below SortedSets
            // obtained from Sergey Kalinichenko,
            // https://stackoverflow.com/a/42356143
            // (Accessed 1 September 2023).

            users = new SortedSet<User>(
                Comparer<User>.Create((a, b) =>
                {
                    var res = a.Username.CompareTo(b.Username);
                    return res != 0 ? res : a.Username.CompareTo(b.Username);
                })
               );
            chatrooms = new SortedSet<Chatroom>(
                Comparer<Chatroom>.Create((a, b) =>
                {
                    var res = a.Name.CompareTo(b.Name);
                    return res != 0 ? res : a.Name.CompareTo(b.Name);
                })
               );
            privateChatrooms = new List<PrivateChatroom>();
        }

        /// <summary>
        /// Generate test entries for database implementation.
        /// TODO delete before submission.
        /// </summary>

        public void GenerateFakeDatabase()
        {
            /* Creates users */
            for (int i = 1; i <= NUMBER_OF_USERS; i++)
            {
                String temp = "User ";
                AddUser(temp + i);
            }
            /* Creates chatrooms */
            for (int i = 1; i <= NUMBER_OF_CHATROOMS; i++)
            {
                String temp = "Chatroom ";
                AddChatroom(temp + i);
            }

            SearchChatroomByName("Chatroom 1").AddUser("User 1");
            SearchChatroomByName("Chatroom 1").AddUser("User 2");
            SearchChatroomByName("Chatroom 1").AddUser("User 3");
            SearchChatroomByName("Chatroom 2").AddUser("User 4");
            SearchChatroomByName("Chatroom 2").AddUser("User 5");
            SearchChatroomByName("Chatroom 3").AddUser("User 6");

            AddPrivateChatroom("Private Chatroom 1", SearchUserByName("User 1"), SearchUserByName("User 2"));
            AddPrivateChatroom("Private Chatroom 2", SearchUserByName("User 3"), SearchUserByName("User 4"));
            AddPrivateChatroom("Private Chatroom 3", SearchUserByName("User 4"), SearchUserByName("User 5"));
        }

        public SortedSet<User> Users
        {
            get { return users; }
            set { users = value; }
        }

        public SortedSet<Chatroom> Chatrooms
        {
            get { return chatrooms; }
            set { chatrooms = value; }
        }

        public List<PrivateChatroom> PrivateChatrooms
        {
            get { return privateChatrooms; }
            set { privateChatrooms = value; }
        }

        public bool AddUser(string username)
        {
            return users.Add(new User(username));
        }

        public bool RemoveUser(string username)
        {
            return users.Remove(new User(username));
        }

        public bool UserExists(string username)
        {
            return users.Contains(new User(username));
        }

        public bool AddChatroom(string roomName)
        {

            return chatrooms.Add(new Chatroom(roomName));
        }

        public bool RemoveChatroom(string roomName)
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

        public bool ChatroomExists(string roomName)
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

        public bool AddPrivateChatroom(string roomName, User userOne, User userTwo)
        {
            bool validNewRoom = true;
            foreach(var room in privateChatrooms)
            {
                if (room.AllowedUsers.Contains(userOne) && room.AllowedUsers.Contains(userTwo))
                {
                    validNewRoom = false;
                }
            }
            if (validNewRoom)
            {
                privateChatrooms.Add(new PrivateChatroom(roomName, userOne, userTwo));
            }
            return validNewRoom;
        }

        public bool RemovePrivateChatroom(string roomName)
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

        public bool PrivateChatroomExists(string roomName)
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

        public PrivateChatroom SearchPrivateChatroomByName(string name)
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

            allowedPrivateChatrooms.Sort(0, allowedPrivateChatrooms.Count, 
                Comparer<string>.Create((a, b) =>
            {
                var res = a.CompareTo(b);
                return res != 0 ? res : a.CompareTo(b);
            }));

            return allowedPrivateChatrooms.ToArray();
        }

        public override bool Equals(object obj)
        {
            return obj is ChatDatabase database &&
                   EqualityComparer<SortedSet<User>>.Default.Equals(users, database.users) &&
                   EqualityComparer<SortedSet<Chatroom>>.Default.Equals(chatrooms, database.chatrooms) &&
                   EqualityComparer<List<PrivateChatroom>>.Default.Equals(privateChatrooms, database.privateChatrooms) &&
                   EqualityComparer<SortedSet<User>>.Default.Equals(Users, database.Users) &&
                   EqualityComparer<SortedSet<Chatroom>>.Default.Equals(Chatrooms, database.Chatrooms) &&
                   EqualityComparer<List<PrivateChatroom>>.Default.Equals(PrivateChatrooms, database.PrivateChatrooms);
        }

        public override int GetHashCode()
        {
            int hashCode = -890036836;
            hashCode = hashCode * -1521134295 + EqualityComparer<SortedSet<User>>.Default.GetHashCode(users);
            hashCode = hashCode * -1521134295 + EqualityComparer<SortedSet<Chatroom>>.Default.GetHashCode(chatrooms);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<PrivateChatroom>>.Default.GetHashCode(privateChatrooms);
            hashCode = hashCode * -1521134295 + EqualityComparer<SortedSet<User>>.Default.GetHashCode(Users);
            hashCode = hashCode * -1521134295 + EqualityComparer<SortedSet<Chatroom>>.Default.GetHashCode(Chatrooms);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<PrivateChatroom>>.Default.GetHashCode(PrivateChatrooms);
            return hashCode;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
