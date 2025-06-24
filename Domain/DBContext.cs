using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options ): base(options) { 
        
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Center> Centers { get; set; }
        public DbSet<CenterImages> CenterImages { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bookings>().Property(x => x.Status).HasConversion<string>();
            modelBuilder.Entity<Bookings>().Property(x => x.Type).HasConversion<string>();

            modelBuilder.Entity<Center>().Property(x => x.Type).HasConversion<string>();

            modelBuilder.Entity<Users>().Property(x => x.Role).HasConversion<string>();
        }
    }
}
