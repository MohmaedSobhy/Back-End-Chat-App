using WebChatApi.dto;
using WebChatApi.model;

namespace WebChatApi.services.authincation
{
    public interface IAuthincationServices
    {
        public Task<Result> LoginMethod(LoginDto login);

        public Task<Result> RegisterMethod(RegiterDto register);

        public Task<Result> GetNewToken(string refreshToken);

        public Task<Result> Logout(string userId);
    }
}
