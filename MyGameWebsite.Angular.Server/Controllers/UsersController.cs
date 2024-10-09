using MyGameWebsite.Server.MockData;
using MyGameWebsite.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyGameWebsite.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public ActionResult<List<User>> GetAllUsers()
        {
            // Return the static list of users.
            return UserStore.Users;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<User> GetUser(int id)
        {
            // Searches the list for a user with the specified ID.
            var user = UserStore.Users.FirstOrDefault(u => u.Id == id);
            // If no user is found, return a 404 Not Found response.
            if (user == null)
                return NotFound();
            // If found, return the user.
            return user;
        }
    }
}
