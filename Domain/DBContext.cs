using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Share.Util;
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

        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bookings>().Property(x => x.Status).HasConversion<string>();
            modelBuilder.Entity<Bookings>().Property(x => x.Type).HasConversion<string>();

            modelBuilder.Entity<Center>().Property(x => x.Type).HasConversion<string>();

            modelBuilder.Entity<Users>().Property(x => x.Role).HasConversion<string>();

            modelBuilder.Entity<Courses>()
            .Property(c => c.Type)
            .HasConversion<string>();

            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = Guid.Parse("d5407a97-9267-4ff4-b01a-fc3de7ff29ac"),
                    Email = "admin@center.vn",
                    Password = PasswordUtil.HashPassword("Admin123@"), 
                    FullName = "Nguyễn Thành Vinh",
                    Phone = "0911222333",
                    Address = "123 Lê Lợi, Quận 1, TP.HCM",
                    Role = Entities.Enum.RoleEnum.Center,
                    CreatedAt = new DateTime(2024, 1, 10),
                    UpdatedAt = new DateTime(2025, 6, 20),
                    IsActive = true,
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<Center>().HasData(
                new Center
                {
                    Id = Guid.Parse("6c187f68-4e52-4e3e-9a2b-b42976f347a2"),
                    Name = "Trung tâm Cai nghiện Thuốc lá Quận 1",
                    Location = "123 Lê Lợi, Quận 1, TP.HCM",
                    HotLine = "18001111",
                    Email = "contact@quitcenter.vn",
                    DirectorName = "Nguyễn Thành Vinh",
                    EstablishedDate = new DateTime(2020, 5, 15),
                    Capacity = 120,
                    CurrentPatientCount = 35,
                    Type = Entities.Enum.CenterType.Public,
                    Notes = "Chuyên hỗ trợ tư vấn và điều trị nghiện thuốc lá bằng các liệu pháp tâm lý và y khoa.",
                    Image = "https://sdmntprwestus.oaiusercontent.com/files/00000000-7d08-6230-9129-91414b5e179d/raw?se=2025-06-29T05%3A01%3A47Z&sp=r&sv=2024-08-04&sr=b&scid=f7e18e3f-756e-5b33-85e5-a5db4150eae2&skoid=61180a4f-34a9-42b7-b76d-9ca47d89946d&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skt=2025-06-28T20%3A11%3A59Z&ske=2025-06-29T20%3A11%3A59Z&sks=b&skv=2024-08-04&sig=UfMiXKvlWY90aTQgza5mAqazs1pjTpapXKj1ZXTpBnI%3D",
                    UserId = Guid.Parse("d5407a97-9267-4ff4-b01a-fc3de7ff29ac"),
                    CreatedAt = new DateTime(2024, 1, 10),
                    UpdatedAt = new DateTime(2025, 6, 20),
                    IsActive = true,
                    IsDeleted = false
                }
            );


        }
    }
}
