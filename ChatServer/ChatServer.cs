using DatabaseDLL;
using IChatServerInterfaceDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class ChatServer : IChatServerInterface
    {
        ChatDatabase db;

        /// <summary>
        /// ChatServer Constructor. 
        /// </summary>
        public ChatServer()
        {
            db = ChatDatabase.Instance;
            //DBGenerator.GenerateFakeDatabase(db); //populate database with fake data - DEBUG ONLY
        }

        public bool AddUser(string username)
        {
            Console.WriteLine($"Attempting to add user: {username}");
            bool result = db.AddUser(username);
            Console.WriteLine($"Add user result: {result}");
            return result;
        }

        public bool RemoveUser(string username)
        {
            Console.WriteLine($"Attempting to remove user: {username}");
            bool result = db.RemoveUser(username);
            Console.WriteLine($"Remove user result: {result}");
            return result;
        }

        public bool UserExists(string username)
        {
            Console.WriteLine($"Checking if user exists: {username}");
            bool result = db.UserExists(username);
            Console.WriteLine($"User exists result: {result}");
            return result;
        }

        public bool AddChatroom(string roomName)
        {
            Console.WriteLine($"Attempting to add chatroom: {roomName}");
            bool result = db.AddChatroom(roomName);
            Console.WriteLine($"Add chatroom result: {result}");
            return result;
        }

        public bool RemoveChatroom(string roomName)
        {
            Console.WriteLine($"Attempting to remove chatroom: {roomName}");
            bool result = db.RemoveChatroom(roomName);
            Console.WriteLine($"Remove chatroom result: {result}");
            return result;
        }

        public bool ChatroomExists(string roomName)
        {
            Console.WriteLine($"Checking if chatroom exists: {roomName}");
            bool result = db.ChatroomExists(roomName);
            Console.WriteLine($"Chatroom exists result: {result}");
            return result;
        }

        /// <summary>
        /// Take strings for room name, and two usernames. Searches for users,
        /// throws FaultException<KeyNotFoundException>if not found. If both
        /// users searched successfully, attempt to add private chatroom, return
        /// success as a boolean value.
        /// </summary>
        /// <param name="roomName">string roomName</param>
        /// <param name="usernameOne">string usernameOne</param>
        /// <param name="usernameTwo">string usernameTwo</param>
        /// <returns>boolean of whether successfully added</returns>
        /// <exception cref="FaultException{KeyNotFoundException}"></exception>
        public bool AddPrivateChatroom(string roomName, string usernameOne, string usernameTwo)
        {
            Console.WriteLine($"Attempting to add private chatroom: {roomName} between {usernameOne} and {usernameTwo}");
            User userOne = null;
            User userTwo = null;

            try
            {
                userOne = db.SearchUserByName(usernameOne);
            }
            catch (KeyNotFoundException e)
            {
                throw new FaultException<KeyNotFoundException>(e, new FaultReason("First user not found."));
            }
            try
            {
                userTwo = db.SearchUserByName(usernameTwo);
            }
            catch (KeyNotFoundException e)
            {
                throw new FaultException<KeyNotFoundException>(e, new FaultReason("Second user not found."));
            }

            if (userOne != null && userTwo != null)
            {
                bool result = db.AddPrivateChatroom(roomName, userOne, userTwo);
                Console.WriteLine($"Add private chatroom result: {result}");
                return result;
            }
            Console.WriteLine("Add private chatroom failed.");
            return false;
        }

        /// <summary>
        /// Get private chatroom based on two users. If no private chatroom
        /// exists between two users, throw KeyNotFoundException.
        /// </summary>
        /// <param name="usernameOne">string usernameOne</param>
        /// <param name="usernameTwo">string usernameTwo</param>
        /// <returns>boolean of whether successfully added</returns>
        /// <exception cref="FaultException{KeyNotFoundException}"></exception>
        public PrivateChatroom GetPrivateChatroom(string usernameOne, string usernameTwo)
        {
            Console.WriteLine($"Attempting to get private chatroom between {usernameOne} and {usernameTwo}");
            User userOne = null;
            User userTwo = null;

            try
            {
                userOne = db.SearchUserByName(usernameOne);
            }
            catch (KeyNotFoundException e)
            {
                throw new FaultException<KeyNotFoundException>(e, new FaultReason("First user not found."));
            }
            try
            {
                userTwo = db.SearchUserByName(usernameTwo);
            }
            catch (KeyNotFoundException e)
            {
                throw new FaultException<KeyNotFoundException>(e, new FaultReason("Second user not found."));
            }

            try
            {
                PrivateChatroom chatroom = db.GetPrivateChatroom(userOne, userTwo);
                Console.WriteLine("Successfully retrieved private chatroom.");
                return chatroom;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine($"Failed to get private chatroom. Error: {e.Message}");
                throw new FaultException<KeyNotFoundException>(e, new FaultReason(e.Message));
            }
        }

        public bool RemovePrivateChatroom(string roomName)
        {
            Console.WriteLine($"Attempting to remove private chatroom: {roomName}");
            bool result = db.RemovePrivateChatroom(roomName);
            Console.WriteLine($"Remove private chatroom result: {result}");
            return result;
        }

        public bool PrivateChatroomExists(string roomName)
        {
            Console.WriteLine($"Checking if private chatroom exists: {roomName}");
            bool result = db.PrivateChatroomExists(roomName);
            Console.WriteLine($"Private chatroom exists result: {result}");
            return result;
        }

        /// <summary>
        /// Search for User by string. Either return User or throw
        /// FaultException<KeyNotFoundException> if unsuccessful.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{KeyNotFoundException}"></exception>
        public User SearchUserByName(string username)
        {
            try
            {
                User user = db.SearchUserByName(username);
                Console.WriteLine("User found.");
                return user;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine($"User not found. Error: {e.Message}");
                throw new FaultException<KeyNotFoundException>(e, new FaultReason(e.Message));
            }
        }

        /// <summary>
        /// Search for Chatroom by string. Either return Chatroom or throw
        /// FaultException<KeyNotFoundException> if unsuccessful.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{KeyNotFoundException}"></exception>
        public Chatroom SearchChatroomByName(string name)
        {
            Console.WriteLine($"Searching for chatroom by name: {name}");
            try
            {
                Chatroom chatroom = db.SearchChatroomByName(name);
                Console.WriteLine("Chatroom found.");
                return chatroom;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine($"Chatroom not found. Error: {e.Message}");
                throw new FaultException<KeyNotFoundException>(e, new FaultReason(e.Message));
            }
        }

        /// <summary>
        /// Search for PrivateChatroom by string. Either return PrivateChatroom or throw
        /// FaultException<KeyNotFoundException> if unsuccessful.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{KeyNotFoundException}"></exception>
        public PrivateChatroom SearchPrivateChatroomByName(string name)
        {
            try
            {
                PrivateChatroom chatroom = db.SearchPrivateChatroomByName(name);
                Console.WriteLine("Private chatroom found.");
                return chatroom;
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine($"Private chatroom not found. Error: {e.Message}");
                throw new FaultException<KeyNotFoundException>(e, new FaultReason(e.Message));
            }
        }

        public string[] GetUserNames()
        {
            return db.GetUserNames();
        }

        public string[] GetChatroomNames()
        {
            return db.GetChatroomNames();
        }

        /// <summary>
        /// Search for User by string. If successful, get allowed private
        /// chatrooms. Throw FaultException<KeyNotFoundException>
        /// if unsuccessful.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{KeyNotFoundException}"></exception>
        public string[] GetAllowedPrivateChatroomNames(string username)
        {
            User user;
            try
            {
                user = db.SearchUserByName(username);
                return db.GetAllowedPrivateChatroomNames(user);
            }
            catch (KeyNotFoundException e)
            {
                throw new FaultException<KeyNotFoundException>(e, new FaultReason(e.Message));
            }
        }

        /* FileManagement methods */

        /// <summary>
        /// Add File to Chatroom
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{KeyNotFoundException}"></exception>
        public bool AddSharedFileToChatroom(string roomName, SharedFile file) {
            try {
                return db.AddSharedFileToChatroom(roomName, file);
            } catch (KeyNotFoundException e) {
                throw new FaultException<KeyNotFoundException>(e, new FaultReason("Chatroom not found."));
            }
        }

        /// <summary>
        /// Remove file from chatroom
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{KeyNotFoundException}"></exception>
        public bool RemoveSharedFileFromChatroom(string roomName, string fileName) {
            try {
                return db.RemoveSharedFileFromChatroom(roomName, fileName);
            } catch (KeyNotFoundException e) {
                throw new FaultException<KeyNotFoundException>(e, new FaultReason("Chatroom not found."));
            }
        }

        /// <summary>
        /// Get shared file from chatroom
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{KeyNotFoundException}"></exception>
        public SharedFile GetSharedFileFromChatroom(string roomName, string fileName) {
            try {
                return db.GetSharedFileFromChatroom(roomName, fileName);
            } catch (KeyNotFoundException e) {
                throw new FaultException<KeyNotFoundException>(e, new FaultReason("Chatroom or File not found."));
            }
        }

        /// <summary>
        /// Get all shared files from chatroom
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        /// <exception cref="FaultException{KeyNotFoundException}"></exception>
        public List<SharedFile> GetAllSharedFilesFromChatroom(string roomName) {
            Console.WriteLine($"Attempting to add shared file to chatroom: {roomName}");
            try {
                return db.GetAllSharedFilesFromChatroom(roomName);
            } catch (KeyNotFoundException e) {
                throw new FaultException<KeyNotFoundException>(e, new FaultReason("Chatroom not found."));
            }
        }
    }
}
