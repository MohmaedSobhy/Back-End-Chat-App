using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WaterProducts.services.token;
using WebChatApi.data;
using WebChatApi.dto;
using WebChatApi.model;

namespace WebChatApi.services.authincation
{
    public class AuthincationServices : IAuthincationServices
    {
        private readonly ITokenServices tokenServices;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationData database;

        public AuthincationServices(ITokenServices tokenServices, 
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationData database)
        {
            this.tokenServices = tokenServices;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.database = database;
        }

        public async Task<Result> LoginMethod(LoginDto login)
        {
            ApplicationUser? applicationUser = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == login.phone);
            if (applicationUser == null)
                return Result.Failure("Phone number or password is Wrong He");

            var result = await  signInManager.PasswordSignInAsync(applicationUser, login.password, false, false);
            if (result.Succeeded)
            {
                var roles = await userManager.GetRolesAsync(applicationUser);
                var token = await tokenServices.getGenerateToken(applicationUser, roles);
                var refreshToken = await tokenServices.getNewRefreshToken(applicationUser.Id);
                return Result.Success(new {token,refreshToken,applicationUser.UserName,applicationUser.PhoneNumber,applicationUser.Id});
            }

            return Result.Failure("Phone number or password is Wrong");
        }

        public async Task<Result> RegisterMethod(RegiterDto register)
        {
            if (await userManager.Users.AnyAsync(u => u.PhoneNumber == register.phoneNumber))
                return Result.Failure("Phone number already exists");
            ApplicationUser user = new ApplicationUser();
            user.UserName = register.UserName;
            user.PhoneNumber = register.phoneNumber;
            user.LastSeen= DateTime.UtcNow;
            var result = await userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {

                var roles = await userManager.GetRolesAsync(user);
                var token = await tokenServices.getGenerateToken(user, roles);
                var refreshToken = await tokenServices.getNewRefreshToken(user.Id);
                return Result.Success(new { token, refreshToken, user.UserName, user.PhoneNumber, user.Id });
            }

            return Result.Failure(result.Errors);

            
        }

        public async Task<Result> GetNewToken(string refreshToken)
        {
            var refreshTokenData = await database.RefreshTokens.FirstOrDefaultAsync(token => token.Token == refreshToken);

            if (refreshTokenData == null)
                return Result.Failure("Refresh Token Expired");

            if (refreshTokenData.Expiration < DateTime.UtcNow)
                return Result.Failure("Refresh Token Expired");

            var user = await userManager.FindByIdAsync(refreshTokenData.UserId);
            var roles = await userManager.GetRolesAsync(user!);
            string token = await tokenServices.getGenerateToken(user!, roles);

            return Result.Success(token);
        }

        public async Task<Result> Logout(string userId)
        {

         RefreshToken? refreshToken=  await database.RefreshTokens.FirstOrDefaultAsync(u => u.UserId == userId);
            if (refreshToken==null)
            {
                return Result.Failure("Failed To Logout");
            }
            database.Remove(refreshToken);
            database.SaveChanges();
            return Result.Success("You Logout Success");

        }
    }
}
