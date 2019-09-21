using Common;
using Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class ReserveBook : BaseEntity
    {
        
        public int BookId { get; set; }
        public virtual Book Book { get; set; }


        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual User User { get; set; }


        [DisplayName("وضعیت کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public BookStatus BookStatus { get; set; }

        public DateTime? ReserveDate { get; set; }
        public DateTime? BorrowDate { get; set; }
    }
    #region Config

    public class StudentBookConfiguration : IEntityTypeConfiguration<ReserveBook>
    {
        public void Configure(EntityTypeBuilder<ReserveBook> builder)
        {
            builder.HasOne(c => c.Book)
                .WithMany(c => c.StudentBook)
                .HasForeignKey(c => new { c.BookId});
        }
    }
    #endregion
}