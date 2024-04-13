using Microsoft.EntityFrameworkCore;
using WebApplication2;
using WebApplication2.Data;
using Microsoft.AspNetCore.Identity;
using WebApplication2.Services;
using WebApplication2.Areas.ProjectManagement.Models;
using Microsoft.Extensions.Logging;
using static System.Formats.Asn1.AsnWriter;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

//lab9
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//lab9

builder.Services.AddSingleton<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseStatusCodePagesWithRedirects("/Home/NotFound?statusCode={0}"); //Custom ERROR page
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}


//COMPP2139 – Lab10
using var scope = app.Services.CreateScope();
var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

try
{
    AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await ContextSeed.SeedRolesAsync(userManager, roleManager);
    await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);
}
catch (Exception e)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(e, "An error occurred seeding the roles for the system");
}

//lab10


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Projects}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
