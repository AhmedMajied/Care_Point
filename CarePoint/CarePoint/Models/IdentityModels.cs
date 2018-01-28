using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace CarePoint.Models
{
    public class CPUserRole : IdentityUserRole<long> { }
    public class CPUserClaim : IdentityUserClaim<long> { }
    public class CPUserLogin : IdentityUserLogin<long> { }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<long,CPUserLogin,CPUserRole,CPUserClaim>
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public Nullable<long> BloodTypeID { get; set; }
        public byte[] Photo { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string NationalIDNumber { get; set; }
        public byte[] NationalIDPhoto { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser,long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class Specialist : ApplicationUser
    {
        public byte[] ProfessionLicense { get; set; }
        public long SpecialityID { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,CPRole,long,CPUserLogin,CPUserRole,CPUserClaim>
    {
        public ApplicationDbContext()
            : base("IdentityConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Citizens");
            modelBuilder.Entity<Specialist>().ToTable("Specialists");
            modelBuilder.Entity<CPRole>().ToTable("Roles");
            modelBuilder.Entity<CPUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<CPUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<CPUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<CPRole>().Property(r => r.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CPUserClaim>().Property(r => r.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

        }
    }
    

    public class CPRole : IdentityRole<long, CPUserRole>
    {
        public CPRole() { }
        public CPRole(string name) { Name = name; }
    }

    public class CPUserStore : UserStore<ApplicationUser, CPRole, long,
        CPUserLogin, CPUserRole, CPUserClaim>
    {
        public CPUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CPRoleStore : RoleStore<CPRole, long, CPUserRole>
    {
        public CPRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}