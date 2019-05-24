using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class TeacherBookList : BaseEntity
    {
      
        public int BookListId { get; set; }
        public string ISBN { get; set; }
        public virtual BookList BookList { get; set; }

        [ForeignKey(nameof(Teacher))]
        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
    #region Configure

    public class TeacherBookListConfiguration : IEntityTypeConfiguration<TeacherBookList>
    {
        public void Configure(EntityTypeBuilder<TeacherBookList> builder)
        {
            builder.HasOne(c => c.BookList)
                .WithMany(c => c.TeacherBookList)
                .HasForeignKey(c => new { c.BookListId});
        }
    }
    #endregion
}