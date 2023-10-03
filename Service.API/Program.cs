using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.API.Configurations;
using Service.Common.Options;
using Service.Domain;
using Service.Repository.Extentions;
using Service.Services.Extentions;
using System.Text;

namespace Service.API
{
    public class Program
    {

        private static DbContextOptionsBuilder ApplicationDbContextBuilderAction(IConfiguration configuration ,DbContextOptionsBuilder? builder = null)
        {
            builder ??= new DbContextOptionsBuilder();
            return builder.UseSqlServer(configuration.GetConnectionString("AplicationDb"));
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adds services to the container.
            // Adds the config source for settings from the database
            builder.Configuration.Add<DatabaseConfigSource>(srt => ApplicationDbContextBuilderAction(builder.Configuration, srt.OptionsBuilder));

            // Adds suppport to IOptions pattern
            builder.Services.Configure<JwtInfo>(builder.Configuration.GetSection("JwtInfo"));
            builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

            builder.Services.AddDbContext<MainContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MainContextDb")));


            builder.Services.AddControllers();
            builder.Services.AddRepositories();
            builder.Services.AddServices();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtInfo:TokenKey"] ?? throw new ArgumentNullException("TokenKey"))),
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                    };
                });


            var app = builder.Build();

            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4100"));

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}