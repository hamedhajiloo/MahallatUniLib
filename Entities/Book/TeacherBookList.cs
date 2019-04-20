using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class TeacherBookList : BaseEntity
    {
      
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public virtual Book Book { get; set; }

        [ForeignKey(nameof(Teacher))]
        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
    #region Configure

    public class TeacherBookListConfiguration : IEntityTypeConfiguration<TeacherBookList>
    {
        public void Configure(EntityTypeBuilder<TeacherBookList> builder)
        {
            builder.HasOne(c => c.Book)
                .WithMany(c => c.TeacherBookList)
                .HasForeignKey(c => new { c.BookId});
        }
    }
    #endregion
}