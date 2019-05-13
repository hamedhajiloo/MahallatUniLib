using Common;
using Common.Enums;
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

        #endregion ctor

        #region Properties

        [DisplayName("سال ورود")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public int EntryYear { get; set; }


        [DisplayName("روزانه / شبانه")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public StudentStatus StudentStatus { get; set; }

        [DisplayName("مبلغ جریمه")]
        public decimal PenaltyAmount { get; set; }


        #endregion Properties

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

        public virtual List<StudentBookList> StudentBookList { get; set; }

        #endregion Relations
    }
}