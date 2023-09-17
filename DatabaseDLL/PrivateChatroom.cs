using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDLL
{
    /// <summary>
    /// PrivateChatroom extends Chatroom, and operates as a direct message functionality.
    /// This is a chatroom where only two users can access. The names of the private
    /// chatroom do not have to be unique as they are not accessible to all users.
    /// This allows users with the private chatroom to name it whatever they want. 
    /// </summary>
    [DataContractAttribute]
    public class PrivateChatroom : Chatroom
    {
        /// <summary>
        /// HashSet chosed to allow for unique usernames
        /// </summary>
        [DataMemberAttribute()]
        public HashSet<User> allowedUsers;

        /// <summary>
        /// PrivateChatroom constructor given private chatroom name.
        /// </summary>
        /// <param name="name"></param>
        public PrivateChatroom(string name) : base(name)
        {
            allowedUsers = new HashSet<User>();
        }

        /// <summary>
        /// PrivateChatroom constructor given private chatroom name and two users.
        /// </summary>
        /// <param name="roomname">string roomname</param>
        /// <param name="userOne">User userOne</param>
        /// <param name="userTwo">User useTwo</param>
        public PrivateChatroom(string roomname, User userOne, User userTwo) : base(roomname)
        {
            allowedUsers = new HashSet<User>();
            // Add users via the below AddAllowedUsers methods.
            AddAllowedUser(userOne);
            AddAllowedUser(userTwo);
        }

        /// <summary>
        /// AllowedUsers property setter and getters.
        /// </summary>
        public HashSet<User> AllowedUsers
        {
            get { return allowedUsers; }
            set { allowedUsers = value; }
        }

        /// <summary>
        /// Adds User to AllowedUser property. Only allowed to add if a unique
        /// username, as well as there are no more than two users already added.
        /// If invalid, throw ArgumentException.
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddAllowedUser(User user)
        {
            if (allowedUsers.Count >= 2)
            {
                throw new ArgumentException("Private chatroom already at capacity.");
            }
            else
            {
                allowedUsers.Add(user);
            }
        }

        /// <summary>
        /// Generated Equals method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is PrivateChatroom chatroom &&
                   base.Equals(obj) &&
                   name == chatroom.name &&
                   EqualityComparer<HashSet<User>>.Default.Equals(users, chatroom.users) &&
                   EqualityComparer<List<Message>>.Default.Equals(messages, chatroom.messages) &&
                   Name == chatroom.Name &&
                   EqualityComparer<HashSet<User>>.Default.Equals(Users, chatroom.Users) &&
                   EqualityComparer<List<Message>>.Default.Equals(Messages, chatroom.Messages) &&
                   EqualityComparer<HashSet<User>>.Default.Equals(allowedUsers, chatroom.allowedUsers) &&
                   EqualityComparer<HashSet<User>>.Default.Equals(AllowedUsers, chatroom.AllowedUsers);
        }

        /// <summary>
        /// Generated GetHashCode method.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = -2079731012;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<User>>.Default.GetHashCode(users);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Message>>.Default.GetHashCode(messages);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<User>>.Default.GetHashCode(Users);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Message>>.Default.GetHashCode(Messages);
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<User>>.Default.GetHashCode(allowedUsers);
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<User>>.Default.GetHashCode(AllowedUsers);
            return hashCode;
        }

        /// <summary>
        /// Genereated ToString method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
