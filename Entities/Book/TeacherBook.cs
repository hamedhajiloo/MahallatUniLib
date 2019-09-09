using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class TeacherBook : BaseEntity
    {
      
        public int BookId { get; set; }
        public virtual Book Book { get; set; }


        [ForeignKey(nameof(Teacher))]
        public string TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
    #region Configure

    public class TeacherBookConfiguration : IEntityTypeConfiguration<TeacherBook>
    {
        public void Configure(EntityTypeBuilder<TeacherBook> builder)
        {
            builder.HasOne(c => c.Book)
                .WithMany(c => c.TeacherBook)
                .HasForeignKey(c => new { c.BookId});
        }
    }
    #endregion
}