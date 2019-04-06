using Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Teacher : BaseEntity<string>
    {
        //public string Code { get; set; }

        #region Relations

        [ForeignKey(nameof(User))]
        [DisplayName("کاربر")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey(nameof(Field))]
        [DisplayName("رشته")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public int FieldId { get; set; }

        public virtual Field Field { get; set; }

        public virtual List<TeacherBookList> TeacherBookList { get; set; }

        #endregion Relations
    }
}