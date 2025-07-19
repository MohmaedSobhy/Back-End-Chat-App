using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebChatApi.model
{
    [Table("UserConnections")]
    public class UserConnectionId
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = default!;

        [Required]
        public string connectionId { get; set; } = default!;
    }
}
