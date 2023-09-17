/* Message.cs
 * 
 * */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDLL
{
    [DataContractAttribute]
    public class Message
    {
        [DataMemberAttribute()]
        public User user;
        [DataMemberAttribute()]
        public string message;

        /// <summary>
        /// Message Constructor
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public Message(User user, string message)
        {
            this.user = user;
            this.message = message;
        }

        /// <summary>
        /// User property setter and getter.
        /// </summary>
        public User User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// Text property setter and getter.
        /// </summary>
        public string Text
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// Generated Equals method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is Message message &&
                   EqualityComparer<User>.Default.Equals(user, message.user) &&
                   this.message == message.message &&
                   EqualityComparer<User>.Default.Equals(User, message.User) &&
                   Text == message.Text;
        }

        /// <summary>
        /// Generated GetHashCode method.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 902200646;
            hashCode = hashCode * -1521134295 + EqualityComparer<User>.Default.GetHashCode(user);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(message);
            hashCode = hashCode * -1521134295 + EqualityComparer<User>.Default.GetHashCode(User);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
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
