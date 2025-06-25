using Domain;
using Domain.Config;
using EmailService.Config;
using EmailService.Implement;
using EmailService.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Repository.Implement;
using Repository.Interface;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Discord;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var webHookId = builder.Configuration["Discord:WebHookId"]!;
var webHookToken = builder.Configuration["Discord:WebHookToken"];
builder.Services.AddSerilog();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.Discord(webhookId: ulong.Parse(webHookId), webhookToken: webHookToken,
        restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

builder.Services.AddDbContext<DBContext>(options =>
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning))
);

builder.Services.Configure<SendMailConfig>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);
var jwtSettings = jwtSection.Get<JwtSettings>();
JWTUtil.Configure(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,

        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,

        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.NoResult();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\":\"Token không hợp lệ hoặc đã hết hạn.\"}");
        },

        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\":\"Bạn chưa đăng nhập hoặc token không hợp lệ.\"}");
        },

        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\":\"Bạn không có quyền truy cập vào tài nguyên này.\"}");
        }

    };
});

// Repository Scope
builder.Services.AddScoped<IAuthenicationRepository, AuthenicationRepository>();   
builder.Services.AddAuthorization();

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

app.MapGet("api/discord", () =>
{
    Log.Error("Đi bằng 2 chân chớ sống kiểu 4 chân. Sói Cô Độc Thân Ái <3");
    return "Discord logging test completed.";
});


app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
