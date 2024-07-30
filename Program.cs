using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tascamon.Service;
using TheProjectTascamon.DBContext;
using TheProjectTascamon.IRepos;
using TheProjectTascamon.IService;
using TheProjectTascamon.Models;
using TheProjectTascamon.Repos;
using TheProjectTascamon.Service;



namespace Tascamon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IMoveService, MoveService>();
            builder.Services.AddScoped<IMoveRepository, MoveRepository>();
            builder.Services.AddScoped<IBattleService, BattleService>();
            builder.Services.AddScoped<IBattleRepository, BattleRepository>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<IPokemonService, PokemonService>();
            builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();

            builder.Services.AddScoped<BattleService>();
            builder.Services.AddScoped<MoveService>();
            builder.Services.AddScoped<UserService>();
            //builder.Services.AddIdentity<User, IdentityRole<int>>()
            //.AddEntityFrameworkStores<TascamonContext>()
            //.AddDefaultTokenProviders();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/auth/login";
        options.LogoutPath = "/api/auth/logout";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
            builder.Services.AddAuthorization();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials());
            });
            //Add the DbContext using the connection string from appsettings.json
            builder.Services.AddDbContext<TascamonContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            
            // Configure AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseExceptionHandler("/error");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}