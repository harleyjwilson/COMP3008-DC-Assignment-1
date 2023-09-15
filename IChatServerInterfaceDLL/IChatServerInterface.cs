using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DatabaseDLL;

namespace IChatServerInterfaceDLL
{
    /// <summary>
    /// ChatServer Interface
    /// </summary>
    [ServiceContract]
    public interface IChatServerInterface
    {
        [OperationContract]
        bool AddUser(string username);

        [OperationContract]
        bool RemoveUser(string username);

        [OperationContract]
        bool UserExists(string username);

        [OperationContract]
        bool AddChatroom(string roomName);

        [OperationContract]
        bool RemoveChatroom(string roomName);

        [OperationContract]
        bool ChatroomExists(string roomName);

        [OperationContract]
        bool AddPrivateChatroom(string roomName, string userOne, string userTwo);

        [OperationContract]
        bool RemovePrivateChatroom(string roomName);

        [OperationContract]
        bool PrivateChatroomExists(string roomName);

        [OperationContract]
        [FaultContract(typeof(KeyNotFoundException))]
        User SearchUserByName(string name);

        [OperationContract]
        [FaultContract(typeof(KeyNotFoundException))]
        Chatroom SearchChatroomByName(string name);

        [OperationContract]
        [FaultContract(typeof(KeyNotFoundException))]
        PrivateChatroom SearchPrivateChatroomByName(string name);

        [OperationContract]
        string[] GetUserNames();

        [OperationContract]
        string[] GetChatroomNames();

        [OperationContract]
        string[] GetAllowedPrivateChatroomNames(string username);
    }
}
