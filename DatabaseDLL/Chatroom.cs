using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDLL
{
    [DataContractAttribute]
    public class Chatroom
    {
        [DataMemberAttribute()]
        public string name;
        [DataMemberAttribute()]
        public HashSet<User> users;
        [DataMemberAttribute()]
        public List<Message> messages;

        public Chatroom(string name)
        {
            this.name = name;
            users = new HashSet<User>();
            messages = new List<Message>();
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public HashSet<User> Users
        {
            get { return users; }
            set { users = value; }
        }

        public List<Message> Messages
        {
            get { return messages; }
            set { messages = value; }
        }

        public Boolean AddUser(string username)
        {
            return users.Add(new User(username));
        }

        public Boolean RemoveUser(string username)
        {
            return users.Remove(new User(username));
        }

        public void AddMessage(Message message)
        {
            messages.Add(message);
        }

        public override bool Equals(object obj)
        {
            return obj is Chatroom chatroom &&
                   name == chatroom.name &&
                   EqualityComparer<HashSet<User>>.Default.Equals(users, chatroom.users) &&
                   EqualityComparer<List<Message>>.Default.Equals(messages, chatroom.messages) &&
                   Name == chatroom.Name &&
                   EqualityComparer<HashSet<User>>.Default.Equals(Users, chatroom.Users) &&
                   EqualityComparer<List<Message>>.Default.Equals(Messages, chatroom.Messages);
        }

        public override int GetHashCode()
        {
            int hashCode = -1686499012;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<User>>.Default.GetHashCode(users);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Message>>.Default.GetHashCode(messages);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<User>>.Default.GetHashCode(Users);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Message>>.Default.GetHashCode(Messages);
            return hashCode;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
