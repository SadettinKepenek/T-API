﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using T_API.Core.DTO.EndPoint;
using T_API.Core.DTO.Table;

namespace T_API.UI.Models.Database
{
    public class EditServiceViewModel
    {
 

        public EditServiceViewModel()
        {
            
        }

        public int UserId { get; set; }
        public int DatabaseId { get; set; }
        public string Server { get; set; }
        public string Username { get; set; }
        public string DatabaseName { get; set; }
        public string Port { get; set; }
        public string Provider { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}