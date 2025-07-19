using System.ComponentModel.DataAnnotations;

namespace WebChatApi.dto
{
    public class LoginDto
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Enter Valid Phone Number")]
        public string phone { get; set; }

        [Required]
        public string password {  get; set; }
    }
}
