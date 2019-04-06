using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class TeacherBookList : BaseEntity
    {
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        [ForeignKey(nameof(Teacher))]
        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}