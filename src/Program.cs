using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Reddnet.Data.AccountData;
using Reddnet.Data.ApplicationData;
using Reddnet.Models.DataModels;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Reddnet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        public static async Task MainAsync(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            try
            {
                using(var scope = host.Services.CreateScope())
                {
                    using (var applicationContext = scope.ServiceProvider.GetService<ApplicationDbContext>())
                    {
                        await applicationContext.Database.EnsureCreatedAsync();

                        BlogDataModel blogDataModel = null;
                        ApplicationUser applicationUser = null;

                        using (var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>())
                        {
                            if (!await roleManager.RoleExistsAsync("Administrator"))
                            {
                                await roleManager.CreateAsync(new IdentityRole("Administrator"));
                            }

                            if (!await roleManager.RoleExistsAsync("Writer"))
                            {
                                await roleManager.CreateAsync(new IdentityRole("Writer"));
                            }
                        }

                        using (var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>())
                        {
                            if (!applicationContext.Users.Any())
                            {
                                applicationUser = new ApplicationUser();
                                applicationUser.UserName = "Admin";
                                applicationUser.Email = "default@default.com";
                                applicationUser.Image = System.IO.File.ReadAllBytes(".//wwwroot//images//ProfilPicture.png");

                                await userManager.CreateAsync(applicationUser, "adminPassword");
                                await userManager.AddToRoleAsync(applicationUser, "Administrator");
                            }
                        }

                        if (!applicationContext.Blogs.Any())
                        {
                            blogDataModel = new BlogDataModel
                            {
                                Cover = System.IO.File.ReadAllBytes(".//wwwroot//images//Logo.png"),
                                Created = DateTime.Now,
                                Description = "Welcome to Reddnet a Asp.Net Core mini Reddit",
                                Name = "Welcome"
                            };

                            applicationContext.Blogs.Add(blogDataModel);
                        }

                        if (!applicationContext.Posts.Any())
                        {
                            PostDataModel postDataModel = new PostDataModel
                            {
                                Pinned = true,
                                Archieved = true,
                                Blog = blogDataModel,
                                Author = applicationUser,
                                Title = "Welcome to r/Welcome and Reddnet a Asp.Net Core mini Reddit",
                                Preview = "Hello visitor this is not an serious Project that goes online",
                                Content = "Hello visitor this is not an serious Project that goes online! <br> i only want to make an mini Reddit Clone to learn Asp.Net Core <br> And i wan´t to share it with your all!",
                                Link = "https://github.com/cetoxx/Reddnet",
                                Cover = System.IO.File.ReadAllBytes(".//wwwroot//images//Logo.png")
                            };

                            applicationContext.Posts.Add(postDataModel);
                        }

                        if (!applicationContext.Settings.Any())
                        {
                            SettingDataModel settingDataModel = new SettingDataModel
                            {
                                Title = "Reddnet",
                                Logo = System.IO.File.ReadAllBytes(".//wwwroot//images//Logo.png")
                            };

                            applicationContext.Settings.Add(settingDataModel);
                        }

                        await applicationContext.SaveChangesAsync();
                    }
                }
            } catch { }
             await host.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}
