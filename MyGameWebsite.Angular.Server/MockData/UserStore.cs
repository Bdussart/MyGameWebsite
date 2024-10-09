using MyGameWebsite.Server.Models;

namespace MyGameWebsite.Server.MockData
{
    public class UserStore
    {
        public static List<User> Users = new List<User>
        {
            new User { Id=1, Name = "admin", Password = "password", Email="admin@Example.com", Roles = new List<string> { "Admin", "User" } },
            new User { Id=2, Name= "user", Password = "password", Email="user@Example.com", Roles = new List<string> { "User" } },
            new User { Id=3, Name= "test", Password = "password", Email="test@Example.com", Roles = new List<string> { "Admin" } }
        };
    }
}
