using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDLL
{
    [DataContractAttribute]
    public class User
    {
        [DataMemberAttribute()]
        public string username;

        public User(string username)
        {
            this.username = username;
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   username == user.username &&
                   Username == user.Username;
        }

        public override int GetHashCode()
        {
            int hashCode = 1270267002;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Username);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
