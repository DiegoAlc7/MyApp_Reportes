using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyApp.Business.Services;
using MyApp.DataAccess.Data;
using MyApp.DataAccess.IRepositories;
using MyApp.DataAccess.Repositories;
using MyApp.Entities;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configurar autenticación basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login"; // Ruta para redirigir cuando el usuario no está autenticado
        options.AccessDeniedPath = "/Login/AccessDenied"; // Ruta para redirigir cuando el usuario no tiene permisos
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tiempo de expiración de la cookie
        options.SlidingExpiration = true; // Renovar la expiración con cada solicitud
    });

QuestPDF.Settings.License = LicenseType.Community;

// Configurar base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IArtículoRepository, ArtículoRepository>();
builder.Services.AddScoped<IPréstamoRepository, PréstamoRepository>();

builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ArtículoService>();
builder.Services.AddScoped<PréstamoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts. 
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Habilitar autenticación
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();