using WebChatApi.data;
using WebChatApi.dto;
using WebChatApi.model;

namespace WebChatApi.services.users
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationData database;

        public UserServices(ApplicationData database)
        {
            this.database = database;
        }
        public List<UserDto> getAllUsers(int page, string userId)
        {
            return database.Users.
                Skip(page-1).Take(page*5).
                Where(u=>u.Id!=userId).Select(u=>new UserDto { Id=u.Id,userName=u.UserName!,
                phone=u.PhoneNumber!}).ToList();
        }

        public string getUserState(string userId) { 

            var connections= database.userConnections.Where(u=>u.UserId == userId).ToList();

            if (connections.Count() == 0) { 
                var user= database.Users.FirstOrDefault(u=>u.Id == userId)!;

                return user.LastSeen.ToString();
            }

            return "Online";
        }

    }
}
