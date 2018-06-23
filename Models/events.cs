using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace kaoshi.Models
{
    public class events{
        [Key]
        public int eventid {get;set;}
        [Required]
        [MinLength(2)]
        public string title {get;set;}
        public string time {get;set;}
        public DateTime date {get;set;}
        public string duration  {get;set;}
        [MinLength(10)]
        public string description {get;set;}
        public int UserId {get;set;}
        public users user {get;set;}

        public List<joins> joins {get;set;}
        public events(){
        joins=new List<joins>();
        }
    }
}