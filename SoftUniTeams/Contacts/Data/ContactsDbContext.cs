using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Data
{
    public class ContactsDbContext : IdentityDbContext<ApplicationUser>
    {
        public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<ApplicationUserContact> ApplicationUsersContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Composite key for ApplicationUserContact
            builder.Entity<ApplicationUserContact>()
                .HasKey(ac => new { ac.ContactId, ac.ApplicationUserId });

            // Define relationship between Contact and ApplicationUserContact with proper navigation
            builder.Entity<ApplicationUserContact>()
                .HasOne(ac => ac.Contact)
                .WithMany() // Use the navigation property here
                .HasForeignKey(ac => ac.ContactId)
                .OnDelete(DeleteBehavior.NoAction);

            // Define relationship between ApplicationUser and ApplicationUserContact with proper navigation
            builder.Entity<ApplicationUserContact>()
                .HasOne(ac => ac.ApplicationUser)
                .WithMany() // Use the navigation property here
                .HasForeignKey(ac => ac.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<Contact>()
                .HasData(new Contact()
                {
                    Id = 1,
                    FirstName = "Bruce",
                    LastName = "Wayne",
                    PhoneNumber = "+359881223344",
                    Address = "Gotham City",
                    Email = "imbatman@batman.com",
                    Website = "www.batman.com"
                });

            base.OnModelCreating(builder);
        }
    }
}