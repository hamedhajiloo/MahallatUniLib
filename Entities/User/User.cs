using Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class User : IdentityUser, IEntity
    {
        #region Ctor

        public User()
        {
            IsActive = true;
        }

        #endregion Ctor

        #region Properties

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public int Age { get; set; }
        public GenderType Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }

        #endregion Properties
    }

    #region Config

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.Property(p => p.UserName).IsRequired().HasMaxLength(100);
        }
    }

    #endregion Config
}