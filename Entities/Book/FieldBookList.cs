using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class FieldBookList : BaseEntity
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public virtual Book Book { get; set; }


        [ForeignKey(nameof(Field))]
        public int FieldId { get; set; }
        public virtual Field Field { get; set; }
    }

    public class FieldBookListConfiguration : IEntityTypeConfiguration<FieldBookList>
    {
        public void Configure(EntityTypeBuilder<FieldBookList> builder)
        {
            builder.HasOne(c => c.Book)
                .WithMany(c => c.FieldBookList)
                .HasForeignKey(c => new { c.BookId});
        }
    }
}