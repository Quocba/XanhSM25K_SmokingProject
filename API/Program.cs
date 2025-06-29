using CloudinaryDotNet;
using Domain;
using Domain.Config;
using EmailService.Config;
using EmailService.Implement;
using EmailService.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Models;
using OhBau.Service.CloudinaryService;
using PayOSService.Config;
using PayOSService.Services;
using Repository.Implement;
using Repository.Interface;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Discord;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Your API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập 'Bearer {token}' vào đây"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


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

builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new Account(
        configuration["Cloudinary:CloudName"],
        configuration["Cloudinary:ApiKey"],
        configuration["Cloudinary:Secret"]);
});

builder.Services.AddSingleton(sp =>
{
    var account = sp.GetRequiredService<Account>();
    return new Cloudinary(account);
});

builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

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
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<ICenterRepository, CenterRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IPayOSService, PayOSService.Services.PayOSService>();
builder.Services.AddAuthorization();

//PayOs Config
builder.Services.Configure<PayOSConfig>(
    builder.Configuration.GetSection(PayOSConfig.ConfigName));
builder.Services.AddHttpClient<IPayOSService, PayOSService.Services.PayOSService>();
builder.Services.Configure<PayOSConfig>(builder.Configuration.GetSection("PayOS"));

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
