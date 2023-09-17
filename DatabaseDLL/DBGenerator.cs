/* DBGenerator.cs
 * Creates a database with fake data.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDLL {

    public static class DBGenerator {
        private const int NUM_OF_USERS = 12;
        private const int NUM_OF_CHATROOMS = 4;

        /// <summary>
        /// Method to populate ChatDatabase with fake data
        /// </summary>
        /// <param name="db"></param>
        public static void GenerateFakeDatabase(ChatDatabase db) {
            //// Creates users
            for (int i = 1; i <= NUM_OF_USERS; i++) {
                db.AddUser("User " + i);
            }

            //// Creates chatrooms
            for (int i = 1; i <= NUM_OF_CHATROOMS; i++) {
                db.AddChatroom("Chatroom " + i);
            }

            /// Add users to chatrooms
            db.SearchChatroomByName("Chatroom 1").AddUser("User 1");
            db.SearchChatroomByName("Chatroom 1").AddUser("User 2");
            db.SearchChatroomByName("Chatroom 1").AddUser("User 3");

            //// Add private chatrooms
            db.AddPrivateChatroom("Private Chatroom 1", db.SearchUserByName("User 1"), db.SearchUserByName("User 2"));
            db.AddPrivateChatroom("Private Chatroom 2", db.SearchUserByName("User 3"), db.SearchUserByName("User 4"));
        }
    }
}
