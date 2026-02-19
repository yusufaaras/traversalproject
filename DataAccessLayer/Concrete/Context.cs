using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("server=DESKTOP-S1QPNRR;database=TraversalDb;integrated security=true;TrustServerCertificate=True;");
            //  optionsBuilder.UseSqlServer("Server=localhost,1433;Database=TraversalDb;User Id=SA;Password=Yusuf123;Encrypt=False;TrustServerCertificate=True");
          optionsBuilder.UseSqlServer("server=77.245.159.121\\MSSQLSERVER2022;database=vhbtraveldb;user=Vhbtravel;Password=5W&gHpnnXwn7@8py;Encrypt=True;TrustServerCertificate=True;");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. ADIM: Identity tablolarının birincil anahtarlarını (primary keys) tanımlamak için bunu ekleyin
            base.OnModelCreating(modelBuilder);

            // 2. ADIM: Kendi özel yapılandırmalarınız (Features ve Activity listeleri)
            modelBuilder.Entity<Destination_yerler>()
                .Property(x => x.Features)
                .HasConversion(
                    v => v == null ? null : string.Join(';', v),
                    v => string.IsNullOrEmpty(v) ? new List<string>() : v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                );

            modelBuilder.Entity<Destination_yerler>()
                .Property(x => x.Activity)
                .HasConversion(
                    v => v == null ? null : string.Join(';', v),
                    v => string.IsNullOrEmpty(v) ? new List<string>() : v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
                );
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<About2> About2s { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactUs> ContactUses { get; set; }
        public DbSet<Destination_yerler> Destinations { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Feature2> Feature2s { get; set; }
        public DbSet<Guide> Guides { get; set; }
        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<SubAbout> SubAbouts { get; set; }
        public DbSet<TestiMonial> Testimonials { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}