using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;  // <-- Importante para UseSqlServer
using SETENA.GestionVacaciones.DAL;   // <-- Namespace donde está tu ApplicationDbContext


var builder = WebApplication.CreateBuilder(args);

// Agregar el DbContext y la cadena de conexión (configúralo en appsettings.json)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios MVC
builder.Services.AddControllersWithViews();

// Autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuenta/Login";
        options.LogoutPath = "/Cuenta/Logout";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // <-- Autenticación
app.UseAuthorization();
app.UseStaticFiles(); // En Startup.cs o Program.cs, según .NET Core 6/7/8

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
