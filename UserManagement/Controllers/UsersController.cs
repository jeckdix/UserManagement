using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserContext _context;


        public UsersController(IUserService userService, UserContext context)
        {
            _userService = userService;
            _context = context;

        }


        [HttpGet("users")]
        [Authorize]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }


        [HttpGet("user{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("user")]
        public ActionResult CreateUser([FromBody] User user)
        {
            try
            {
                _userService.AddUser(user);

                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict($"Couldn't create account!: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]

        public ActionResult Login([FromBody] UserLoginDto userLogin)
        {
            bool authStatus = _userService.LoginUser(userLogin);
            if (authStatus)

            {
                User user = _context.Users.FirstOrDefault(u => u.Email == userLogin.Email);
                 
                var token = _userService.GenerateToken(user);
                return Ok(new { token = token.Item1, expireAt = token.Item2 });
            }
            else
            {
                return Unauthorized("Invalid email or password");
            }




        }







    }
}
