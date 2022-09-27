using Cefalo.TechTalk.Api.Controllers;
using Cefalo.TechTalk.Api.GlobalExceptionHandler;
using Cefalo.TechTalk.Database.Context;
using Cefalo.TechTalk.Repository.Contracts;
using Cefalo.TechTalk.Repository.Repositories;
using Cefalo.TechTalk.Service.Contracts;
using Cefalo.TechTalk.Service.DTOs;
using Cefalo.TechTalk.Service.Services;
using Cefalo.TechTalk.Service.Utils.Contracts;
using Cefalo.TechTalk.Service.Utils.CustomFormatters.OutputFormatters;
using Cefalo.TechTalk.Service.Utils.DtoValidators;
using Cefalo.TechTalk.Service.Utils.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TechTalkContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSetting:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddScoped<BaseValidator<BlogDetailsDto>, BlogDetailsDtoValidator>();
builder.Services.AddScoped<BaseValidator<BlogPostDto>, BlogPostDtoValidator>();
builder.Services.AddScoped<BaseValidator<BlogUpdateDto>, BlogUpdateDtoValidator>();
builder.Services.AddScoped<BaseValidator<UserDetailsDto>, UserDetailsDtoValidator>();
builder.Services.AddScoped<BaseValidator<UserSignInDto>, UserSignInDtoValidator>();
builder.Services.AddScoped<BaseValidator<UserSignUpDto>, UserSignUpDtoValidator>();
builder.Services.AddScoped<BaseValidator<UserUpdateDto>, UserUpdateDtoValidator>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHandler, PasswordHandler>();
builder.Services.AddScoped<IJwtHandler, JwtHandler>();
builder.Services.AddScoped<ILoggerManager, LoggerManager>();



builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
}).AddXmlDataContractSerializerFormatters()
            .AddMvcOptions(option =>
            {
                option.OutputFormatters.Add(new CsvOutputFormatter());
                option.OutputFormatters.Add(new PlainTextOutputFormatter());
                option.OutputFormatters.Add(new HtmlOutputFormatter());
            });


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.ConfigureExceptionHandler();
  


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
