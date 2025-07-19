using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebChatApi.dto;
using WebChatApi.model;
using WebChatApi.services.authincation;

namespace WebChatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthincationServices authincation;

        public AccountController(IAuthincationServices authincation)
        {
            this.authincation = authincation;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var result= await authincation.LoginMethod(login);
            if (result.IsSuccess)
            {
                GeneralResponse response = new GeneralResponse();
                response.data = result.data;
                response.success = true;
                response.message = "Login successful";
                return Ok(response);
            }
            return BadRequest(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegiterDto regiter)
        {
            var result = await authincation.RegisterMethod(regiter);
            if (result.IsSuccess)
            {
                GeneralResponse response = new GeneralResponse();
                response.data = result.data;
                response.success = true;
                response.message = "Register successful";
                return Ok(response);
            }
            return BadRequest(result);
        }


     


        [HttpGet("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            Result result = await authincation.Logout(currentUserId);
            if (result.IsSuccess) 
                return Ok(result);
            return BadRequest(result);
        }

        

    }
}
