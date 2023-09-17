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
        /* Singleton using the .NET "lazy" convention to avoid threading issues */
        private static readonly Lazy<ChatDatabase> lazy = new Lazy<ChatDatabase>(() => new ChatDatabase());
        public static ChatDatabase Instance => lazy.Value;

        private const int NUMBER_OF_USERS = 12; //number of user in database
        private const int NUMBER_OF_CHATROOMS = 4; //number of chatrooms
        /// <summary>
        /// SortedSet<User> for users. Allows unique usernames as per
        /// assignment spec. 
        /// </summary>
        public SortedSet<User> users;
        /// <summary>
        /// SortedSet<Chatroom> for chatrooms. Allows unique chatroom names
        /// as per assignment spec. 
        /// </summary>
        public SortedSet<Chatroom> chatrooms;
        public List<PrivateChatroom> privateChatrooms;

        /// <summary>
        /// ChatDatabase Constructor
        /// </summary>
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

        /// <summary>
        /// SortedSet(User) users getter.
        /// </summary>
        public SortedSet<User> Users
        {
            get { return users; }
            set { users = value; }
        }

        /// <summary>
        /// SortedSet(Chatroom) chatrooms getter.
        /// </summary>
        public SortedSet<Chatroom> Chatrooms
        {
            get { return chatrooms; }
            set { chatrooms = value; }
        }

        /// <summary>
        /// List(PrivateChatroom) privateChatrooms getter.
        /// </summary>
        public List<PrivateChatroom> PrivateChatrooms
        {
            get { return privateChatrooms; }
            set { privateChatrooms = value; }
        }

        /// <summary>
        /// Add user to users SortedSet given username string.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Boolean of whether successfully added. Returns false
        /// if user already exists.</returns>
        public bool AddUser(string username)
        {
            return users.Add(new User(username));
        }

        /// <summary>
        /// Remove user from users SortedSet given username string.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Boolean of whether successfully removed. Returns false
        /// if user does not exist.</returns>
        public bool RemoveUser(string username)
        {
            return users.Remove(new User(username));
        }

        /// <summary>
        /// Check whether user exists in users SortedSet given username string.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Boolean of whether user exists.</returns>
        public bool UserExists(string username)
        {
            return users.Contains(new User(username));
        }

        /// <summary>
        /// Add chatroom to chatrooms SortedSet given roomName string.
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns>Boolean of whether successfully added. Returns false
        /// if chatroom already exists.</returns>
        public bool AddChatroom(string roomName)
        {

            return chatrooms.Add(new Chatroom(roomName));
        }

        /// <summary>
        /// Remove chatroom from chatrooms SortedSet given roomName string.
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns>Boolean of whether successfully removed. Returns false
        /// if chatroom does not exist.</returns>
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

        /// <summary>
        /// Check whether chatroom exists in chatrooms SortedSet given roomName string.
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns>Boolean of whether chatroom exists.</returns>
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

        /// <summary>
        /// Add private chatroom based on whether private chatroom already exists
        /// with the two users. If it does, no chatroom is created.
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="userOne"></param>
        /// <param name="userTwo"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get private chatroom based on two users. If no private chatroom
        /// exists between two users, throw KeyNotFoundException.
        /// </summary>
        /// <param name="userOne"></param>
        /// <param name="userTwo"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public PrivateChatroom GetPrivateChatroom(User userOne, User userTwo)
        {
            foreach (var room in privateChatrooms)
            {
                if (room.AllowedUsers.Contains(userOne) && room.AllowedUsers.Contains(userTwo))
                {
                    return room;
                }
            }
            throw new KeyNotFoundException("Private chatroom not found.");
        }

        /// <summary>
        /// Remove private chatroom from privateChatrooms SortedSet given roomName string.
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns>Boolean of whether successfully removed. Returns false
        /// if private chatroom does not exist.</returns>
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

        /// <summary>
        /// Check whether private chatroom exists in privateChatrooms
        /// SortedSet given roomName string.
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns>Boolean of whether privateChatroom exists.</returns>
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

        /// <summary>
        /// Search user by name string. Throws KeyNotFoundException if not found. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
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

        /// <summary>
        /// Search chatroom by name string. Throws KeyNotFoundException if not found. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
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

        /// <summary>
        /// Search private chatroom by name string. Throws KeyNotFoundException if not found. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
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

        /// <summary>
        /// Get string array of usernames.
        /// </summary>
        /// <returns>names (string[])</returns>
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

        /// <summary>
        /// Get string array of chatroom names.
        /// </summary>
        /// <returns>names (string[])</returns>
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

        /// <summary>
        /// Get private chatrooms a given user is allowed to access. Defined by
        /// whether that user is in the AllowedUser property of that private
        /// chatroom.
        /// </summary>
        /// <param name="user">User user to search against for allowed
        /// private chatrooms</param>
        /// <returns>string array of private chatrooms</returns>
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

        /// <summary>
        /// Generated Equals method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>boolean result</returns>
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

        /// <summary>
        /// Generated GetHashCode method.
        /// </summary>
        /// <returns>int</returns>
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

        /// <summary>
        /// Generated ToString method.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
