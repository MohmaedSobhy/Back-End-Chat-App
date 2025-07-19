namespace WebChatApi.model
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
        public string UserId { get; set; }
    }
}
