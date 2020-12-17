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
                    options.Conventions.AddPageRoute(RouteConstants.CommunityView, "r/{name}");
                    options.Conventions.AddPageRoute(RouteConstants.CommunityCreate, "new/community");
                    options.Conventions.AddPageRoute(RouteConstants.CommunityDelete, "r/{name}/delete");
                    options.Conventions.AddPageRoute(RouteConstants.CommunityEdit, "r/{name}/edit");
                    options.Conventions.AddPageRoute(RouteConstants.PostView, "p/{id}");
                    options.Conventions.AddPageRoute(RouteConstants.PostCreate, "new/post");
                    options.Conventions.AddPageRoute(RouteConstants.PostDelete, "p/{id}/delete");
                    options.Conventions.AddPageRoute(RouteConstants.PostEdit, "p/{id}/edit");
                    options.Conventions.AddPageRoute(RouteConstants.ReplyDelete, "reply/{id}/delete");
                    options.Conventions.AddPageRoute(RouteConstants.AccountRegister, "register");
                    options.Conventions.AddPageRoute(RouteConstants.AccountLogin, "/login");
                    options.Conventions.AddPageRoute(RouteConstants.AccountLogout, "/logout");
                    options.Conventions.AddPageRoute(RouteConstants.Search, "/search");
                    options.Conventions.AddPageRoute(RouteConstants.Profile, "u/{name}");
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
                app.UseExceptionHandler(RouteConstants.Error);
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapRazorPages());
        }
    }
}
