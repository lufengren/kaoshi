using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace kaoshi.Models
{
    public class users{
        [Key]
        public int UserId{get;set;}
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string firstname { get; set; }
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string lastname { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string password { get; set; }
        
        public List<events> createdevents {get;set;}
        public List<joins> joins {get;set;}

        public users(){
            createdevents=new List<events>();
            joins=new List<joins>();
        }
        

        
    }
}