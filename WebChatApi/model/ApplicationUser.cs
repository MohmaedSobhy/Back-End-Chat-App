using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebChatApi.model
{
    public class ApplicationUser : IdentityUser
    {

        public DateTime? LastSeen { get; set; }

    }
}
