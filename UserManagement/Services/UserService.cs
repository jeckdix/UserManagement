using System.Text.RegularExpressions;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.DTOs;
using System.Reflection.Metadata.Ecma335;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace UserManagement.Services


{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int userId);
        
        void AddUser(User user);

        bool LoginUser(UserLoginDto user);
        (string, DateTime) GenerateToken (User user);

    }

    public class UserService : IUserService
    {
        private readonly UserContext _context;
        private readonly PasswordService _passwordService;
        private IConfiguration _configuration;



        public UserService(UserContext context, PasswordService passwordService, IConfiguration configuration)
        {
            _context = context;
            _passwordService = passwordService;
            _configuration = configuration; 
       
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }


        public void AddUser(User user)

        {
            if (_context.Users.Any(u => u.Email == user.Email)) 
            {
                throw new InvalidOperationException("Email already exists");
            }
            if (!Regex.IsMatch(user.Password, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"))
            {
                throw new InvalidOperationException("Invalid password format");
            }

            user.Password = _passwordService.HashPassword(user.Password);


            _context.Users.Add(user);
            _context.SaveChanges();
        }


        public string GetHashedPasswordByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            return user?.Password; 
        }

        public (string, DateTime ) GenerateToken (User user)
        {
            var expireAt = DateTime.Now.AddMinutes(5);
            var securityKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
{
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.Phone),
                new Claim (ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                expires: expireAt,
                signingCredentials: credentials 
                );



            return (new JwtSecurityTokenHandler().WriteToken(token), expireAt );
        }

        public bool LoginUser(UserLoginDto user)
        {
           
            string hashedPasswordFromDb =GetHashedPasswordByEmail(user.Email);
            if (hashedPasswordFromDb == null)
            {
                return false;
            }

           bool authStatus =  _passwordService.VerifyPassword(user.Password, hashedPasswordFromDb);

            return authStatus; 
        }



    }
   
}
