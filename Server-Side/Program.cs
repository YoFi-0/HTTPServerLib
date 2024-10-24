using Darkom_App.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Server_Side.DataBase;
using Server_Side.Repositorys.Classes;
using Server_Side.Services.Classes;
using Server_Side.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string x = "server=127.0.0.1;uid=root;pwd=;database=darakom";
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseMySql(x, ServerVersion.AutoDetect(x));
    }
 );




builder.Services.AddScoped<IEncreptionService, EncreptionService>();
builder.Services.AddScoped<IFn, Fn>();
builder.Services.AddScoped(typeof(IJwtService<>), typeof(JwtService<>));
builder.Services.AddSingleton<IHandlerService, HandlerService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // إضافة تعريف الأمان (JWT)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "jwt", // يجب أن يكون Authorization وليس jwt
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey, // استخدام ApiKey مع Header
        Scheme = "Bearer"
    });

    // إضافة متطلب الأمان لجميع العمليات
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // يجب أن يكون مطابقًا للاسم في AddSecurityDefinition
                }
            },
            new List<string>()
        }
    });

    // تفعيل الفلتر الذي يضيف متطلبات الأمان للعمليات
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true")); //save token
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<Catcher>();
app.Run();
