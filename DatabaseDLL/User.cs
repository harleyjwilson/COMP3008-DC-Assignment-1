using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDLL
{
    public class User
    {
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
                   username == user.username;
        }

        public override int GetHashCode()
        {
            return 799926177 + EqualityComparer<string>.Default.GetHashCode(username);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
