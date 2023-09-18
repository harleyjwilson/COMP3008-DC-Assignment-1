using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DatabaseDLL {
    /// <summary>
    /// Represents a private chatroom that extends the Chatroom class.
    /// This chatroom is designed for direct messaging between two users.
    /// </summary>
    [DataContract]
    public class PrivateChatroom : Chatroom {
        /// <summary>
        /// Gets or sets the allowed users in the private chatroom.
        /// </summary>
        [DataMember]
        public HashSet<User> AllowedUsers { get; set; } = new HashSet<User>();

        /// <summary>
        /// Initializes a new instance of the PrivateChatroom class with a given name.
        /// </summary>
        /// <param name="name">The name of the private chatroom.</param>
        public PrivateChatroom(string name) : base(name) {
        }

        /// <summary>
        /// Initializes a new instance of the PrivateChatroom class with a given name and two users.
        /// </summary>
        /// <param name="roomname">The name of the private chatroom.</param>
        /// <param name="userOne">The first user.</param>
        /// <param name="userTwo">The second user.</param>
        public PrivateChatroom(string roomname, User userOne, User userTwo) : base(roomname) {
            AddAllowedUser(userOne);
            AddAllowedUser(userTwo);
        }

        /// <summary>
        /// Adds a user to the allowed users list.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <exception cref="ArgumentException">Thrown when the chatroom is already at capacity.</exception>
        public void AddAllowedUser(User user) {
            if (AllowedUsers.Count >= 2) {
                throw new ArgumentException("Private chatroom already at capacity.");
            }
            AllowedUsers.Add(user);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the objects are equal, otherwise false.</returns>
        public override bool Equals(object obj) {
            return obj is PrivateChatroom chatroom &&
                   base.Equals(obj) &&
                   AllowedUsers.SetEquals(chatroom.AllowedUsers);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() {
            return base.GetHashCode() ^ AllowedUsers.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string representation of the object.</returns>
        public override string ToString() {
            return $"PrivateChatroom: {Name}, Allowed Users: {string.Join(", ", AllowedUsers.Select(u => u.Username))}";
        }
    }
}
