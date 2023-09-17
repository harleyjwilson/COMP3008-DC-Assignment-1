/* Chatroom.cs
 * Holds information for a chatroom.
 * */
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
        /// <summary>
        /// HashSet chosen to allow for unique usernames within chatroom.
        /// </summary>
        [DataMemberAttribute()]
        private HashSet<User> users;
        [DataMemberAttribute()]
        private List<Message> messages;
        private List<byte[]> sharedImages; //List of images shared between users
        private List<byte[]> sharedTextFiles; //List of text files shared between users

        /// <summary>
        /// Chatroom Constructor
        /// </summary>
        /// <param name="name">string</param>
        public Chatroom(string name)
        {
            this.name = name;
            users = new HashSet<User>();
            messages = new List<Message>();
            sharedImages = new List<byte[]>();
            sharedTextFiles = new List<byte[]>();
        }

        /// <summary>
        /// Name properties setter and getter.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Users properties setter and getter.
        /// </summary>
        public HashSet<User> Users
        {
            get { return users; }
            set { users = value; }
        }

        /// <summary>
        /// Messages properties setter and getter.
        /// </summary>
        public List<Message> Messages
        {
            get { return messages; }
            set { messages = value; }
        }

        /// <summary>
        /// Returns entire list of shared images.
        public List<byte[]> SharedImages {
            get { return sharedImages; }
            set { sharedImages = value; }
        }

        /// <summary>
        /// Returns image by index.
        public byte[] GetSharedImage(int index) {
            return sharedImages[index];
        }

        /// <summary>
        /// Returns entire list of shared text files.
        public List<byte[]> SharedTextFiles {
            get { return sharedTextFiles; }
            set { sharedTextFiles = value; }
        }

        /// <summary>
        /// Returns text file by index.
        public byte[] GetSharedTextFile(int index) {
            return sharedTextFiles[index];
        }



        /// <summary>
        /// Add user to chatroom given username string.
        /// </summary>
        /// <param name="username">string username</param>
        /// <returns>Return true if successful, false if not.</returns>
        public Boolean AddUser(string username)
        {
            return users.Add(new User(username));
        }

        /// <summary>
        /// Remove user from chatroom via username string.
        /// </summary>
        /// <param name="username">string username</param>
        /// <returns>Return true if successful, false if not.</returns>
        public Boolean RemoveUser(string username)
        {
            return users.Remove(new User(username));
        }

        /// <summary>
        /// Add message to end of Message List.
        /// </summary>
        /// <param name="message">Message message</param>
        public void AddMessage(Message message)
        {
            messages.Add(message);
        }

        /// <summary>
        /// Generated Equals method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Generated GetHashCode method.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Generated ToString method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
