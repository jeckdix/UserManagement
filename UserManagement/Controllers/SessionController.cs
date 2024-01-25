using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public IActionResult CreateSession([FromBody] SessionDto session)
        {
            return Ok("Session creation successful"); 
        }

    }
}
