using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebChatApi.model;
using WebChatApi.services.users;

namespace WebChatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserServices userServices;

        public UserController(IUserServices userServices)
        {
            this.userServices = userServices;
        }

        [HttpGet("GetAllUsers")]
        public  IActionResult getAllUser(int page)
        {
           GeneralResponse response = new GeneralResponse();
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            response.success = true;
            response.data = userServices.getAllUsers(page,currentUserId);
            response.message = "Users retrieved successfully";

            return Ok(response);
        }


        [HttpGet("GetUserState")]
        
        public IActionResult GetUserState(string UserId)
        {
            GeneralResponse response = new GeneralResponse();
            string userState= userServices.getUserState(UserId);
            response.success = true;
            response.message = "user State Succeffully";
            response.data = new { UserId, state= userState };
            return Ok(response);
        }
    }
}
