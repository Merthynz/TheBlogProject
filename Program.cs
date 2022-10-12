using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheBlogProject.Data;
using TheBlogProject.Models;
using TheBlogProject.Services;
using TheBlogProject.ViewModels;

var builder = WebApplication.CreateBuilder(args);

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

//Register a preconfigured instance of the MailSettings class
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IBlogEmailSender, EmailService>();

var app = builder.Build();

// Pull out my registered DataService
var serviceProvider = app.Services.CreateScope().ServiceProvider;

ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();
RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
UserManager<BlogUser> userManager = serviceProvider.GetRequiredService<UserManager<BlogUser>>();

var dataService = new DataService(context, roleManager, userManager);
await dataService.ManageDataAsync();


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
