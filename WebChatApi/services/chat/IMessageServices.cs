using WebChatApi.dto;
using WebChatApi.model;

namespace WebChatApi.services.chat
{
    public interface IMessageServices
    {
       
        public void ConnectToServer(string userId, string connectionId);

        public void DisConnetToServer(string userId, string connectionId);
        public Task<int> CreateChat(string senderId, string receiverId);
        public Task<ChatMessagesDto> SendMessage(SendMessagedto messageDto, string userId);

        public List<string> UserTypingState(UserTypingDto userTyping);

        public List<string> ReciverConnectionId(string reciverId);

        public List<ChatMessagesDto> GetChatMessages(string currentUserId, string secondUserId);

        public List<ChatDto> getAllChats(string userId);

        public Result ReadChatMessages(int chatId);

    }
}
