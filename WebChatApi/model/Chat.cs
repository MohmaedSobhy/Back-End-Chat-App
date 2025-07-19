using System.ComponentModel.DataAnnotations.Schema;

namespace WebChatApi.model
{
    public class Chat
    {
        public int Id { get; set; }

        public string FirstUserId { get; set; }

        public string SecondUserId { get; set; }

        public List<Message> Messages { get; set; }
    }
}
