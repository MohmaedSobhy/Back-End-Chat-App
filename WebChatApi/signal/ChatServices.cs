using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using WebChatApi.data;
using WebChatApi.dto;
using WebChatApi.model;
using WebChatApi.services.chat;

namespace WebChatApi.signal
{

    [Authorize]
    public class ChatServices:Hub
    {

        private readonly ApplicationData database;
        private readonly IMessageServices messageServices;
        

        public ChatServices(ApplicationData database,IMessageServices services)
        {
            this.database = database;
            this.messageServices = services;
          
        }

        [HubMethodName("sendmessagetouser")]
        public async Task sendMessageToUser(SendMessagedto messageDto)
        {

            var connectionIds = messageServices.ReciverConnectionId(messageDto.receiverId);
            connectionIds.Add(Context.ConnectionId);
            ChatMessagesDto message = await messageServices.SendMessage(messageDto,Context.UserIdentifier!);
            
            await Clients.Clients(connectionIds).SendAsync("ReceiveMessage", message);
        }

        public override Task OnConnectedAsync()
        {
            messageServices.ConnectToServer(userId:Context.UserIdentifier!,connectionId:Context.ConnectionId);
            NotifyAllMyFriendState(true);
            return base.OnConnectedAsync();
        }


        [HubMethodName("typing")]
        public void typing(UserTypingDto userTyping)
        {

            var connections =  messageServices.UserTypingState(userTyping);

            Clients.Clients(connections).SendAsync("typing",userTyping.isTyping);

        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {

            messageServices.DisConnetToServer(userId:Context.UserIdentifier!,connectionId:Context.ConnectionId);
            NotifyAllMyFriendState(false);
            return base.OnDisconnectedAsync(exception);
        }


        [HubMethodName("notifyallmyfriendstate")]
        public void NotifyAllMyFriendState(bool isOnline)
        {
            Clients.All.SendAsync("friendstate",isOnline);
        }

    }
}
