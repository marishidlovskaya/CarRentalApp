using Microsoft.EntityFrameworkCore;
using CarRental.Domain.Interfaces.Users;
using CarRental.Infrastructure.Business.Users;
using CarRental.Infrastructure.Data;
using CarRental.Infrastructure.Data.Users;
using CarRental.Services.Interfaces.Users;
using Microsoft.IdentityModel.Tokens;
using CarRental.Domain.Interfaces.Cars;
using CarRental.Services.Interfaces.Cars;
using CarRental.Infrastructure.Business.Cars;
using CarRental.Infrastructure.Data.Cars;
using CarRental.Infrastructure.Business.Bookings;
using CarRental.Services.Interfaces.Bookings;
using CarRental.Domain.Interfaces.Bookings;
using CarRental.Infrastructure.Data.Bookings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers().AddNewtonsoftJson();

// Add services to the container. 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["Authentication:BaseUrl"]; 
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    })
      .AddOpenIdConnect("oidc", options =>
      {
          options.Authority = builder.Configuration["Authentication:BaseUrl"]; options.ClientId = "CarRentalApi";
          options.ResponseType = "code"; options.Scope.Add("openid");
          options.Scope.Add("profile"); options.Scope.Add("fullaccess");
          options.SaveTokens = true;
      });

builder.Services.AddControllers(opt => { opt.Filters.Add(new AuthorizeFilter()); });

builder.Services.AddSwaggerGen(options =>
{
    var scheme = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(builder.Configuration["Authentication:BaseUrl"] + builder.Configuration["Authentication:AuthorizationUrl"]), 
                TokenUrl = new Uri(builder.Configuration["Authentication:BaseUrl"] + builder.Configuration["Authentication:TokenUrl"]), 
            },
        },
        Type = SecuritySchemeType.OAuth2,

    };

    options.AddSecurityDefinition("OAuth", scheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "OAuth", Type = ReferenceType.SecurityScheme }
            },
            new List<string> { }
        }
    });
});

var app = builder.Build();
// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
    .UseSwaggerUI(options =>
    {
        options.OAuthClientId("CarRentalApi");
        options.OAuthScopes("profile", "openid", "fullaccess");
        options.OAuthUsePkce();
    });
}
app.UseHttpsRedirection();
app.UseAuthentication(); app.UseAuthorization();

app.MapControllers();
app.Run();