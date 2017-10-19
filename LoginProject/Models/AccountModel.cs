using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginProject.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName {get ; set ; }
        [Required]
        [DataType(DataType.Password)]
         public string  Password {get ; set ; }
         [Required]
         [DataType(DataType.Password)]
         [Compare("Password", ErrorMessage = "Пароли не совпадают")]
         public string ConfirmPassword  {get ; set ; }

    }
        public class LoginModel
    {
            [Required]
          public string UserName {get ; set ; }
            [Required]
            [DataType(DataType.Password)]
            public string Password {get ; set ; }
    }

        public class HomePageModel {
             [Required]
            public string Address { get; set; }
             [Required]
            public string Name { get; set; }
        
        }
}