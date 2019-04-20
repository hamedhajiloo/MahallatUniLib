using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class StudentBookList : BaseEntity
    {
        
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public virtual Book Book { get; set; }

        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }

        public virtual Student Student { get; set; }
    }
    #region Config

    public class StudentBookListConfiguration : IEntityTypeConfiguration<StudentBookList>
    {
        public void Configure(EntityTypeBuilder<StudentBookList> builder)
        {
            builder.HasOne(c => c.Book)
                .WithMany(c => c.StudentBookList)
                .HasForeignKey(c => new { c.BookId});

        }
    }
    #endregion
}