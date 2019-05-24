using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class FieldBookList : BaseEntity
    {
        public int BookListId { get; set; }
        public string ISBN { get; set; }
        public virtual BookList BookList { get; set; }


        [ForeignKey(nameof(Field))]
        public int FieldId { get; set; }
        public virtual Field Field { get; set; }
    }

    public class FieldBookListConfiguration : IEntityTypeConfiguration<FieldBookList>
    {
        public void Configure(EntityTypeBuilder<FieldBookList> builder)
        {
            builder.HasOne(c => c.BookList)
                .WithMany(c => c.FieldBookList)
                .HasForeignKey(c => new { c.BookListId});
        }
    }
}