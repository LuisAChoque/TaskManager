using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar la base de datos
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer("Server=localhost,1433;Database=Tasks;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=true"));

// Habilitar sesiones
//builder.Services.AddSession();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;  
    options.Cookie.IsEssential = true; // Asegura que la sesión se mantenga activa
    options.Cookie.SameSite = SameSiteMode.None; // Permite compartir cookies entre frontend y backend
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // 
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://127.0.0.1:5500") 
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});


var app = builder.Build();

app.UseRouting(); 
app.UseCors("AllowFrontend");
app.UseSession();
app.UseAuthorization();
app.MapControllers(); 

app.Run();