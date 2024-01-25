using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Services
{
    public interface ISessionSession  {
    
     public string AddSession(SessionDto session); 
    }

    public class SessionService: ISessionSession
    {
        private readonly UserContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(UserContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;

            _httpContextAccessor = httpContextAccessor;

        }


        public string AddSession(SessionDto session)
        {
            var user = _httpContextAccessor.HttpContext.User;

            var name = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;    

            var session_ = new Session(session.SessionName)
            {
                CreatedAt = DateTime.UtcNow,
                CreatedBy = name
            };


            var sc = new SessionClass();
            session.SessionClasses.ForEach(x =>
            {
                sc.Name = x.ClassName;
                sc.SessionId = session_.Id;

                session_.SessionClasses.Add(sc);
            });

            _context.Add(session_);
            _context.SaveChanges();
            

            return ("Session created"); 
        }
    }
}
