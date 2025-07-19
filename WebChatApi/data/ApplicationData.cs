using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection.Emit;
using WebChatApi.model;

namespace WebChatApi.data
{
    public class ApplicationData : IdentityDbContext<ApplicationUser>
    {
        public ApplicationData(DbContextOptions<ApplicationData> options)
         : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {


            base.OnModelCreating(builder);

        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<UserConnectionId>userConnections { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Chat>Chats { get; set; }
    }
}
