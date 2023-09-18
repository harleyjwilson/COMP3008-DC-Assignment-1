/* Chatroom.cs
 * Holds information for a chatroom.
 * Including shared files
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DatabaseDLL {
    /// <summary>
    /// Represents a chatroom with users, messages, and shared files.
    /// </summary>
    [DataContract]
    public class Chatroom {

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public HashSet<User> Users { get; set; } = new HashSet<User>();

        [DataMember]
        public List<Message> Messages { get; set; } = new List<Message>();

        [DataMember]
        public List<SharedFile> SharedFiles { get; set; } = new List<SharedFile>();

        public Chatroom(string name) {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Adds a shared file to the chatroom.
        /// </summary>
        /// <param name="file">The file to add.</param>
        /// <returns>True if the file was added successfully, otherwise false.</returns>
        public bool AddSharedFile(SharedFile file) {
            if (file == null) return false;
            SharedFiles.Add(file);
            return true;
        }

        /// <summary>
        /// Removes a shared file from the chatroom by its name.
        /// </summary>
        /// <param name="fileName">The name of the file to remove.</param>
        /// <returns>True if the file was removed successfully, otherwise false.</returns>
        public bool RemoveSharedFile(string fileName) {
            var file = SharedFiles.FirstOrDefault(f => f.FileName == fileName);
            return file != null && SharedFiles.Remove(file);
        }

        /// <summary>
        /// Retrieves a shared file from the chatroom by its name.
        /// </summary>
        /// <param name="fileName">The name of the file to retrieve.</param>
        /// <returns>The shared file if found, otherwise null.</returns>
        public SharedFile GetSharedFile(string fileName) => SharedFiles.FirstOrDefault(f => f.FileName == fileName);

        /// <summary>
        /// Retrieves all shared files in the chatroom.
        /// </summary>
        /// <returns>A list of all shared files.</returns>
        public List<SharedFile> GetAllSharedFiles() => SharedFiles;

        /// <summary>
        /// Adds a user to the chatroom.
        /// </summary>
        /// <param name="username">The username of the user to add.</param>
        /// <returns>True if the user was added successfully, otherwise false.</returns>
        public bool AddUser(string username) => Users.Add(new User(username));

        /// <summary>
        /// Removes a user from the chatroom.
        /// </summary>
        /// <param name="username">The username of the user to remove.</param>
        /// <returns>True if the user was removed successfully, otherwise false.</returns>
        public bool RemoveUser(string username) => Users.Remove(new User(username));

        /// <summary>
        /// Adds a message to the chatroom.
        /// </summary>
        /// <param name="message">The message to add.</param>
        public void AddMessage(Message message) {
            if (message != null) {
                Messages.Add(message);
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the objects are equal, otherwise false.</returns>
        public override bool Equals(object obj) {
            return obj is Chatroom chatroom && Name == chatroom.Name;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() {
            return Name != null ? Name.GetHashCode() : 0;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string representation of the object.</returns>
        public override string ToString() {
            return $"Chatroom: {Name}, Users: {Users.Count}, Messages: {Messages.Count}, SharedFiles: {SharedFiles.Count}";
        }



    }
}
