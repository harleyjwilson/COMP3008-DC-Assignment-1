using System;
using System.Collections.Generic;
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

        public Message(User user, string message)
        {
            this.user = user;
            this.message = message;
        }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public string Text
        {
            get { return message; }
            set { message = value; }
        }

        public override bool Equals(object obj)
        {
            return obj is Message message &&
                   EqualityComparer<User>.Default.Equals(user, message.user) &&
                   this.message == message.message &&
                   EqualityComparer<User>.Default.Equals(User, message.User) &&
                   Text == message.Text;
        }

        public override int GetHashCode()
        {
            int hashCode = 902200646;
            hashCode = hashCode * -1521134295 + EqualityComparer<User>.Default.GetHashCode(user);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(message);
            hashCode = hashCode * -1521134295 + EqualityComparer<User>.Default.GetHashCode(User);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
