using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDLL
{
    [DataContractAttribute]
    public class PrivateChatroom : Chatroom
    {
        [DataMemberAttribute()]
        public HashSet<User> allowedUsers;

        public PrivateChatroom(string name) : base(name)
        {
            allowedUsers = new HashSet<User>();
        }

        public PrivateChatroom(string roomname, User userOne, User userTwo) : base(roomname)
        {
            allowedUsers = new HashSet<User>();
            AddAllowedUser(userOne);
            AddAllowedUser(userTwo);
        }

        public HashSet<User> AllowedUsers
        {
            get { return allowedUsers; }
            set { allowedUsers = value; }
        }

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

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
