using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheBlogProject.Data;
using TheBlogProject.Models;
using TheBlogProject.Services;

var builder = WebApplication.CreateBuilder(args);

//Need to convert this to Visual Studio 2022
public static async Task Main(string[] args)
    {
    var host = CreateHostBuilder(args).Build();


    //Pull out my registered DataService
    var DataService = host.Services
                          .CreateScope()
                          .ServiceProvider
                          .GetRequiredService<DataService>();

    await DataService.ManageDataAsync();

    host.Run();
}

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddDefaultTokenProviders() 
    .AddEntityFrameworkStores<ApplicationDbContext>();
    
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Register my custom DataService class
builder.Services.AddScoped<DataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
