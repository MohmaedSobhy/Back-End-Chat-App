using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebChatApi.model;
using WebChatApi.services.chat;
using WebChatApi.signal;

namespace WebChatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IMessageServices messageServices;

        public ChatController(IMessageServices messageServices)
        {
            this.messageServices = messageServices;
        }

        [HttpGet("{Id}")]
        public IActionResult getChatMessages(string Id)
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            GeneralResponse response = new GeneralResponse();

            response.data = messageServices.GetChatMessages(currentUserId: currentUserId, secondUserId: Id);
            response.message = "All Messages";
            response.success = true;

            return Ok(response);
        }


        [HttpGet("Chats")]
        public IActionResult getAllChats()
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            GeneralResponse response = new GeneralResponse();
            response.data = messageServices.getAllChats(currentUserId);
            response.success =true;
            response.message = "All Chats you Recived";
            return Ok(response);
        }


        [HttpGet("{chatId:int}")]
        public IActionResult ReadChatMessage(int chatId)
        {
            Result result = messageServices.ReadChatMessages(chatId);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        
    }
}
