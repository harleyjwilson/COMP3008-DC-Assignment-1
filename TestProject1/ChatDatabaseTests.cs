using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseDLL;

namespace TestsAssignment1 {
    [TestClass]
    public class ChatDatabaseTests {
        ChatDatabase db;

        [TestInitialize]
        public void Setup() {
            db = ChatDatabase.Instance;
            DBGenerator.GenerateFakeDatabase(db); //populate database with fake data - DEBUG ONLY
        }

        [TestMethod]
        public void TestAddUser() {
            Assert.IsTrue(db.AddUser("Alice"));
            Assert.IsTrue(db.UserExists("Alice"));
        }

        [TestMethod]
        public void TestRemoveUser() {
            db.AddUser("Bob");
            Assert.IsTrue(db.RemoveUser("Bob"));
            Assert.IsFalse(db.UserExists("Bob"));
        }

        [TestMethod]
        public void TestAddChatroom() {
            Assert.IsTrue(db.AddChatroom("General"));
            Assert.IsTrue(db.ChatroomExists("General"));
        }

        [TestMethod]
        public void TestRemoveChatroom() {
            db.AddChatroom("General");
            Assert.IsTrue(db.RemoveChatroom("General"));
            Assert.IsFalse(db.ChatroomExists("General"));
        }

        [TestMethod]
        public void TestAddPrivateChatroom() {
            User user1 = new("Alice");
            User user2 = new("Bob");
            Assert.IsTrue(db.AddPrivateChatroom("PrivateRoom", user1, user2));
            Assert.IsNotNull(db.GetPrivateChatroom(user1, user2));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestSearchPrivateChatroomByName_NotFound() {
            db.SearchPrivateChatroomByName("NonExistentRoom");
        }

        [TestMethod]
        public void TestAddSharedFileToChatroom() {
            db.AddChatroom("General");
            SharedFile file = new("file1", "content");
            Assert.IsTrue(db.AddSharedFileToChatroom("General", file));
        }

        [TestMethod]
        public void TestRemoveSharedFileFromChatroom() {
            db.AddChatroom("General");
            SharedFile file = new("file1", "content");
            db.AddSharedFileToChatroom("General", file);
            Assert.IsTrue(db.RemoveSharedFileFromChatroom("General", "file1"));
        }

        [TestMethod]
        public void TestGetSharedFileFromChatroom() {
            db.AddChatroom("General");
            SharedFile file = new("file1", "content");
            db.AddSharedFileToChatroom("General", file);
            Assert.IsNotNull(db.GetSharedFileFromChatroom("General", "file1"));
        }
    }
}
