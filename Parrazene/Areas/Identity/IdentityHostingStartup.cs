using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Parrazene.Areas.Identity.Data;
using Parrazene.Data;

[assembly: HostingStartup(typeof(Parrazene.Areas.Identity.IdentityHostingStartup))]
namespace Parrazene.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ParrazeneDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ParrazeneDbContextConnection")));

                services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ParrazeneDbContext>();
            });
        }
    }
}