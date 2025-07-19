using WebChatApi.dto;
using WebChatApi.model;

namespace WebChatApi.services.users
{
    public interface IUserServices
    {
        public List<UserDto> getAllUsers(int page,string userId);
        public string getUserState(string userId);
    }
}
