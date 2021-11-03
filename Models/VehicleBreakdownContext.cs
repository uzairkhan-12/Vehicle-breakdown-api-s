using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleBreakdownAssistant.Models
{
    public class VehicleBreakdownContext : DbContext
    {
    public VehicleBreakdownContext(DbContextOptions<VehicleBreakdownContext> options)
            : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<SecretKeys> SecretKeys { get; set; }
    }
}
