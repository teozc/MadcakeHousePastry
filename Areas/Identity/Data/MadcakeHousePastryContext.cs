using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MadcakeHousePastry.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MadcakeHousePastry.Data
{
    public class MadcakeHousePastryContext : IdentityDbContext<MadcakeHousePastryUser>
    {
        public MadcakeHousePastryContext(DbContextOptions<MadcakeHousePastryContext> options)
            : base(options)
        {
        }

        public DbSet<MadcakeHousePastry.Models.Pastry> Pastry { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
