using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneToManyMVC.Models;

namespace OneToManyMVC.Models
{
    public class OneToManyMVCContext : DbContext
    {
        public OneToManyMVCContext (DbContextOptions<OneToManyMVCContext> options)
            : base(options)
        {
        }

        public DbSet<OneToManyMVC.Models.Student> Student { get; set; }

        public DbSet<OneToManyMVC.Models.Grade> Grade { get; set; }

        public DbSet<OneToManyMVC.Models.Ambion> Ambion { get; set; }
    }
}
