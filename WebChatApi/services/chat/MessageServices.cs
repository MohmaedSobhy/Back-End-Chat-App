using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebChatApi.data;
using WebChatApi.dto;
using WebChatApi.model;

namespace WebChatApi.services.chat
{
    public class MessageServices:IMessageServices
    {
        private readonly ApplicationData dataBase;

        public MessageServices(ApplicationData dataBase) {
            this.dataBase = dataBase;
        }

        public void ConnectToServer(string userId, string connectionId)
        {
            dataBase.userConnections.Add(new UserConnectionId { connectionId = connectionId, UserId = userId });
            dataBase.SaveChanges();
        }

        public void DisConnetToServer(string userId, string connectionId)
        {
            var connection = dataBase.userConnections.FirstOrDefault(c => c.connectionId == connectionId);
            var user = dataBase.Users.FirstOrDefault(u => u.Id == userId)!;
            user.LastSeen = DateTime.UtcNow;
            dataBase.userConnections.Remove(connection!);
            dataBase.SaveChanges();
        }
      
        public async Task<int> CreateChat(string userId, string receiverId)
        {


            var chat = await dataBase.Chats
                .FirstOrDefaultAsync(c => 
                (c.FirstUserId == userId && c.SecondUserId == receiverId)||
                (c.SecondUserId==userId&& c.FirstUserId==receiverId));

            if (chat == null) {

                chat = new Chat { FirstUserId=userId,SecondUserId=receiverId};
                dataBase.Chats.Add(chat);
                await dataBase.SaveChangesAsync();
               
            }

            return chat.Id;

        }
       

        public async Task<ChatMessagesDto> SendMessage(SendMessagedto messageDto,string userId)
        {
            int chatId = messageDto.ChatId;
            if (chatId==0)
            {
              chatId= await CreateChat( userId,messageDto.receiverId);
            }
            Message message = new Message();
            message.Time = DateTime.Now;
            message.IsRead = false;
            message.senderId =userId;
            message.Text= messageDto.Text;
            message.ChatId = chatId;
            dataBase.Messages.Add(message);
            dataBase.SaveChanges();

            return new ChatMessagesDto{
                date=message.Time,
                senderId=userId,
                ChatId = message.ChatId,
                Text =message.Text,sendByYou=true,
                isRead=false};
        }


        public List<string> UserTypingState(UserTypingDto userTyping)
        {
            var connections = dataBase.userConnections.Where(u => u.UserId == userTyping.userId).Select(c => c.connectionId).ToList();
            return connections;
        }

        public List<string> ReciverConnectionId(string reciverId)
        {
           return  dataBase.userConnections
                .Where(c => c.UserId == reciverId)
                .Select(c => c.connectionId)
                .ToList();
        }

        public List<ChatMessagesDto> GetChatMessages(string currentUserId, string secondUserId)
        {
            Chat? chat = dataBase.Chats.
                FirstOrDefault(c => 
                ( c.FirstUserId==currentUserId&& c.SecondUserId==secondUserId) || 
                (c.FirstUserId == secondUserId && c.SecondUserId == currentUserId));
            if (chat == null)
            {
                return [];
            }
            dataBase.Entry(chat).Collection(c => c.Messages).Load();

            List<ChatMessagesDto> message = chat.Messages.OrderBy(m=>m.Time).Select(m=>new ChatMessagesDto {
                ChatId=m.ChatId,Text=m.Text,isRead=m.IsRead,
                 senderId=m.senderId,
                date=m.Time,sendByYou=m.senderId==currentUserId}).ToList();
            return message;
        }

        public  List<ChatDto> getAllChats(string userId)
        {
            List<Chat>  chats= dataBase.Chats.AsNoTracking().
                Where(u=>(u.FirstUserId==userId || u.SecondUserId==userId)).Include(m=>m.Messages).ToList();


            List<ChatDto> results = [];
            Message lastMessage;
            int unRead = 0;
            ApplicationUser user;
            string senderId;
            foreach (Chat c in chats)
            {

                lastMessage=c.Messages.OrderByDescending(m=>m.Time).FirstOrDefault()!;
                unRead = c.Messages.Count(m => m.IsRead == false);
                senderId = (c.FirstUserId == userId) ? c.SecondUserId : c.FirstUserId;
                user = dataBase.Users.FirstOrDefault(u => senderId == u.Id)!;
                
                ChatDto chatDto = new ChatDto
                {
                    chatId = c.Id,
                    senderId = user.Id,
                    title = user.UserName!,
                    unReaded = unRead,
                    lastMessage = lastMessage.Text,
                    date = lastMessage.Time,
                };

                results.Add(chatDto);
            }

            return results;
        }

        public Result ReadChatMessages(int chatId)
        {
            
            Chat? chat = dataBase.Chats.FirstOrDefault(c=> c.Id == chatId);

            if (chat == null)
               return Result.Failure("No Chat Founds");


            dataBase.Entry(chat).Collection(c => c.Messages).Load();

            List<Message> unReadMessages = chat.Messages.Where(m => m.IsRead == false).ToList();

            foreach(var message in unReadMessages)
                message.IsRead = true;
            

            dataBase.SaveChanges();
            return Result.Success("All Message Read");
         

        }
    }
}
