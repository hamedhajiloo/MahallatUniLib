using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class StudentBookList : BaseEntity
    {
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }

        public virtual Student Student { get; set; }
    }
}