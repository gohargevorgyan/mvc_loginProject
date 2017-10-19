using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace LoginProject.Models
{
    public class User
    {
        public int Id {get ; set;}
        public string UserName {get ; set;}
        public string Password {get ; set;}
        public string Address {get ; set;}
        public string Name { get; set; }
    }
    public class UserContext : DbContext
    {
        public UserContext() :
            base("DefaultConnection")
        { }

        public DbSet<User> Users { get; set; }
    }
}