using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reddnet.DataAccess;
using Reddnet.Domain.Entities;
using System;

namespace Reddnet.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.RootDirectory = "/Pages";
                    options.Conventions.AddPageRoute("/Community/View", "r/{name}");
                    options.Conventions.AddPageRoute("/Community/Create", "new/community");
                    options.Conventions.AddPageRoute("/Community/Delete", "r/{name}/delete");
                    options.Conventions.AddPageRoute("/Community/Edit", "r/{name}/edit");
                    options.Conventions.AddPageRoute("/Post/View", "p/{id}");
                    options.Conventions.AddPageRoute("/Post/Create", "new/post");
                    options.Conventions.AddPageRoute("/Post/Delete", "p/{id}/delete");
                    options.Conventions.AddPageRoute("/Post/Edit", "p/{id}/edit");
                    options.Conventions.AddPageRoute("/Reply/Delete", "reply/{id}/delete");
                    options.Conventions.AddPageRoute("/Account/Register", "register");
                    options.Conventions.AddPageRoute("/Account/Login", "/login");
                    options.Conventions.AddPageRoute("/Account/Logout", "/logout");
                    options.Conventions.AddPageRoute("/Search", "/search");
                    options.Conventions.AddPageRoute("/Profile", "u/{name}");
                });

            services.AddIdentity<UserEntity, IdentityRole<Guid>>(x =>
            {
                x.Password.RequireDigit = false;
                x.Password.RequiredLength = 5;
                x.Password.RequireLowercase = true;
                x.Password.RequireUppercase = false;
                x.Password.RequireNonAlphanumeric = false;
                x.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapRazorPages());
        }
    }
}
