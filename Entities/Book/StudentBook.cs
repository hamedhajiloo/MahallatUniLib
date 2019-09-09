using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class StudentBook : BaseEntity
    {
        
        public int BookId { get; set; }
        public virtual Book Book { get; set; }


        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
    #region Config

    public class StudentBookConfiguration : IEntityTypeConfiguration<StudentBook>
    {
        public void Configure(EntityTypeBuilder<StudentBook> builder)
        {
            builder.HasOne(c => c.Book)
                .WithMany(c => c.StudentBook)
                .HasForeignKey(c => new { c.BookId});

        }
    }
    #endregion
}