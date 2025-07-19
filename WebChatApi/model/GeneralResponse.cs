namespace WebChatApi.model
{
    public class GeneralResponse
    {
        public bool success { get; set; }

        public string message { get; set; }

        public dynamic data { get; set; }
    }
}
