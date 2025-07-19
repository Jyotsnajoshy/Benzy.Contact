using Contact.Entity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Contact.Entity.Data
{

    public class ContactdbContext : IdentityDbContext<ApplicationUser>
    {
        public ContactdbContext(DbContextOptions<ContactdbContext> option) : base(option)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ContactUser> Contacts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ContactdbContext).Assembly);

        }
    }
}



