namespace WebChatApi.dto
{
    public class ChatDto
    {
        public int chatId {  get; set; }
        public string senderId { get; set; }

        public string lastMessage { get; set; }

        public int unReaded { get; set; }

        public string title { get; set; }

        public DateTime date { get; set; }
    }
}
