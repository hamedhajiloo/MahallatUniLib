using Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel;
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

        [DisplayName("نام و نام خانوادگی")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        [StringLength(100)]
        public string FullName { get; set; }

        [DisplayName("جنسیت")]
        public GenderType Gender { get; set; }

        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }

        [DisplayName("آخرین ورود")]
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