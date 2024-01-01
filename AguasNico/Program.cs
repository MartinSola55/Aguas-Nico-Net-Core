using AguasNico.Data;
using AguasNico.Data.Repository;
using AguasNico.Data.Repository.IRepository;
using AguasNico.Data.Seeding;
using AguasNico.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddControllersWithViews();

// Agregar work container
builder.Services.AddScoped<IWorkContainer, WorkContainer>();

// Agregar seeding
builder.Services.AddScoped<ISeeder, Seeder>();
var users = builder.Configuration["User"];

// Negar nuevos registros
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DisallowRegistration", policy =>
    {
        policy.RequireAssertion(context => false); // Requiere que la expresión siempre sea falsa
    });
});

var app = builder.Build();

// Configure the localization middleware
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(new CultureInfo("es-ES")),
    SupportedCultures = new List<CultureInfo>
    {
        new("es-ES"),
    },
    SupportedUICultures = new List<CultureInfo>
    {
        new("es-ES"),
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

// Metodo para hacer seeding
Seed();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// Metodo Seed()
void Seed()
{
    using var scope = app.Services.CreateScope();
    var dbSeeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
    dbSeeder.Seed();
}