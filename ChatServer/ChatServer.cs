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

        public ChatServer()
        {
            db = new ChatDatabase();
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

        public bool RemovePrivateChatroom(string roomName)
        {
            return db.RemovePrivateChatroom(roomName);
        }

        public bool PrivateChatroomExists(string roomName)
        {
            return db.PrivateChatroomExists(roomName);
        }

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
