 using BCrypt.Net;

namespace UserManagement.Services
{
    public class PasswordService
    {
        public string HashPassword(string password)
        {
            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());

            return hashedPassword;
        }

        public bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
          
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }
    }
}
