using Contact.Entity.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Entity.Configuration;
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(u => u.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

           

        }

    }
