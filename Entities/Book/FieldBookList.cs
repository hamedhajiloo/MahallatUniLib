using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class FieldBookList : BaseEntity
    {
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }


        [ForeignKey(nameof(Field))]
        public int FieldId { get; set; }
        public virtual Field Field { get; set; }
    }
}