using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class FieldBook : BaseEntity
    {
        public int BookId { get; set; }
        public virtual Book Book { get; set; }


        [ForeignKey(nameof(Field))]
        public int FieldId { get; set; }
        public virtual Field Field { get; set; }
    }

    public class FieldBookConfiguration : IEntityTypeConfiguration<FieldBook>
    {
        public void Configure(EntityTypeBuilder<FieldBook> builder)
        {
            builder.HasOne(c => c.Book)
                .WithMany(c => c.FieldBook)
                .HasForeignKey(c => new { c.BookId});
        }
    }
}