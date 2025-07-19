using WebChatApi.data;
using WebChatApi.extensions;
using WebChatApi.signal;


namespace WebChatApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.IdentiyConfigure();
            builder.Services.DataBaseConfigure(builder.Configuration);
            builder.Services.JWTConfigureAuthincation(builder.Configuration);
            builder.Services.ScopeServices();
            builder.Services.AddSignalR();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.SetCors();

            builder.Services.Configure<JwtOptions>(
             builder.Configuration.GetSection("Jwt"));
            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapHub<ChatServices>("/chatHub");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAllPolicy");

            app.MapControllers();

            app.Run();
        }
    }
}
