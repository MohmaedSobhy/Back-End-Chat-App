using System.ComponentModel.DataAnnotations;

namespace WebChatApi.dto
{
    public class RegiterDto
    {
        public string UserName { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Enter Valid Phone Number")]
        public string phoneNumber { get; set; } = string.Empty;
    }
}
