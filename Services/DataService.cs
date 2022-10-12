using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheBlogProject.Data;
using TheBlogProject.Enums;
using TheBlogProject.Models;

//#nullable disable

namespace TheBlogProject.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext dbContext, 
                           RoleManager<IdentityRole> roleManager, 
                           UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManageDataAsync()
        {
            //Task: Create the DB from the Migrations
            await _dbContext.Database.MigrateAsync();

            //Task 1: Seed a few Roles into the system
            await SeedRolesAsync();

            //Task 2: Seed a few users into the system
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            //If there are already Roles in the system, do nothing.
            if (_dbContext.Roles.Any())
            {
                return;
            }

            //Otherwise we want to create a few Roles
            foreach(var role in Enum.GetNames(typeof(BlogRole)))
            {
                //I need to use the Role Manager to create roles
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task SeedUsersAsync()
        {
            //If there are already User in the system, do nothing.
            if(_dbContext.Users.Any())
            {
                return;
            }

            //Step 1: Creates a new instance of BlogUser
            var adminUser = new BlogUser()
            {
                Email = "rerive@gmail.com",
                UserName = "rerive@gmail.com",
                FirstName = "Renato",
                LastName = "Erive",
                DisplayName = "The Professor",
                PhoneNumber = "(800) 123-4567",
                EmailConfirmed = true
            };


            //Step 2: Use the UserManager to create a new user that is defined by adminUser
            await _userManager.CreateAsync(adminUser, "Abc&123!");


            //Step 3: Add this new user to the Administrator role
            await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());


            //Step 1 repeat: Create the moderator user
            var modUser = new BlogUser()
            {
                Email = "rerive@outlook.com",
                UserName = "rerive@outlook.com",
                FirstName = "Ren",
                LastName = "Erive",
                DisplayName = "The Other Professor",
                PhoneNumber = "(800) 123-1221",
                EmailConfirmed = true
            };

            //Step 2 repeat: Use the UserManager to create a new user that is defined by modUser
            await _userManager.CreateAsync(modUser, "Abc&123!");

            //Step 3 repeat: Add this new user to the Moderator role
            await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());
        }
    }
}
