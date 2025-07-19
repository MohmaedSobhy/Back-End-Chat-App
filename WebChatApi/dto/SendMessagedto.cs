namespace WebChatApi.dto
{
    public class SendMessagedto
    {
        public string Text { get; set; }

        public string receiverId { get; set; }

        public int ChatId { get; set; } = 0;

    }
}
