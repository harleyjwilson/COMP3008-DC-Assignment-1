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

        /// <summary>
        /// User constructor.
        /// </summary>
        /// <param name="username"></param>
        public User(string username)
        {
            this.username = username;
        }

        /// <summary>
        /// Username property setter and getter. 
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        /// <summary>
        /// Generated Equals method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is User user &&
                   username == user.username &&
                   Username == user.Username;
        }

        /// <summary>
        /// Generated GetHashCode method.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 1270267002;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Username);
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
