

using WebChatApi.model;

namespace WaterProducts.services.token
{
    public interface ITokenServices
    {
        public  Task<string> getGenerateToken(ApplicationUser user,IList<string> userRoles);

        public Task<string> getNewRefreshToken(string userId);

        
    }
}
