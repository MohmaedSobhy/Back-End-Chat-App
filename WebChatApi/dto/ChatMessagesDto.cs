namespace WebChatApi.dto
{
    public class ChatMessagesDto
    {
        
        public string Text { get; set; }
        public bool isRead { get; set; }
        public DateTime date {  get; set; }

        public string senderId {get;set;}
        public bool sendByYou { get; set; }

        public int ChatId { get; set; }
    }
}
