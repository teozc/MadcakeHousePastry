using System;
using MadcakeHousePastry.Areas.Identity.Data;
using MadcakeHousePastry.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(MadcakeHousePastry.Areas.Identity.IdentityHostingStartup))]
namespace MadcakeHousePastry.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<MadcakeHousePastryContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MadcakeHousePastryContextConnection")));

                services.AddDefaultIdentity<MadcakeHousePastryUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<MadcakeHousePastryContext>();
            });
        }
    }
}