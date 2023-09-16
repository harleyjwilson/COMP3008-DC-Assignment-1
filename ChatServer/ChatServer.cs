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
            db = new ChatDatabase();
            // Set up fake database for testing purposes
            db.GenerateFakeDatabase();
        }

        public bool AddUser(string username)
        {
            return db.AddUser(username);
        }

        public bool RemoveUser(string username)
        {
            return db.RemoveUser(username);
        }

        public bool UserExists(string username)
        {
            return db.UserExists(username);
        }

        public bool AddChatroom(string roomName)
        {
            return db.AddChatroom(roomName);
        }

        public bool RemoveChatroom(string roomName)
        {
            return db.RemoveChatroom(roomName);
        }

        public bool ChatroomExists(string roomName)
        {
            return db.ChatroomExists(roomName);
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
                return db.AddPrivateChatroom(roomName, userOne, userTwo);
            }
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
                return db.GetPrivateChatroom(userOne, userTwo);
            }
            catch (KeyNotFoundException e)
            {
                throw new FaultException<KeyNotFoundException>(e, new FaultReason(e.Message));
            }
        }

        public bool RemovePrivateChatroom(string roomName)
        {
            return db.RemovePrivateChatroom(roomName);
        }

        public bool PrivateChatroomExists(string roomName)
        {
            return db.PrivateChatroomExists(roomName);
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
                return db.SearchUserByName(username);
            }
            catch (KeyNotFoundException e)
            {
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
            try
            {
                return db.SearchChatroomByName(name);
            }
            catch (KeyNotFoundException e)
            {
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
                return db.SearchPrivateChatroomByName(name);
            }
            catch (KeyNotFoundException e)
            {
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
    }
}
