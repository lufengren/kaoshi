using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace kaoshi.Models
{
    public class joins{
        [Key]
        public int joinid {get;set;}
        public int UserId {get;set;}
        public users user {get;set;}
        public int eventid {get;set;}
        public events events {get;set;}
    }
}