using System.ComponentModel.DataAnnotations.Schema;

namespace WebChatApi.model
{
    public class Message
    {
        public int Id { get; set; }
        
        public string Text { get; set; }
        public string senderId { get; set; }
        
        public DateTime Time { get; set; }

        public bool IsRead { get; set; }

        [ForeignKey("chatId")]
        public int ChatId { get; set; }

        public Chat chat { get; set; }

    }
}
