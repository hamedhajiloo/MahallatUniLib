﻿using Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
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
            Gender = GenderType.Male;
        }

        #endregion Ctor

        #region Properties

        public bool Deleted { get; set; }

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

        #region Relations
        public virtual List<Penalty> Penalties { get; set; }
        public virtual List<ReserveBook> ReserveBooks { get; set; }
        #endregion
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