using Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Student : BaseEntity<string>
    {
        #region ctor
        public Student()
        {
            //EntryYear= az code daneshjoyi bayad bashe
        }
        #endregion

        #region Properties

        [DisplayName("کد دانشجویی")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string Code { get; set; }

        [DisplayName("سال ورود")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public int EntryYear { get; set; }


        #endregion

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


        public virtual List<Book> Books { get; set; }



        #endregion

    }
}