﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseDLL;

namespace ChatClient.ViewModel
{
    public class MainPageViewModel
    {
        public ChatDatabase _database;

        public ObservableCollection<Chatroom> Chatrooms { get; set; }


        public MainPageViewModel()
        {
            
            _database = new ChatDatabase();
           // _database.GenerateFakeDatabase();

     

            Chatrooms = new ObservableCollection<Chatroom>(_database.Chatrooms.ToList());
        }
    }
}