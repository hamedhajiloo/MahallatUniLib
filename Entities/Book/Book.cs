using Common;
using Common.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Book : BaseEntity
    {
        #region Properties

        [DisplayName("نام کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string Name { get; set; }

        [DisplayName("شناسه کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string ISBN { get; set; }

        [DisplayName("نویسنده")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string AuthorName { get; set; }

        [DisplayName("ناشر")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string Publisher { get; set; }

        [DisplayName("تاریخ انتشار")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public DateTimeOffset PublishDate { get; set; }

        [DisplayName("نوبت چاپ")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        [Range(minimum:1,maximum:1000000,ErrorMessage =DataAnotations.Range)]
        public int Edition { get; set; }

        [DisplayName("زبان")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public Language? Language { get; set; }

        [DisplayName("نوع درس")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public CourseType? CourseType { get; set; }

        #endregion Properties

        #region Relations

        public virtual List<StudentBookList> StudentBookList { get; set; }
        public virtual List<Teacher> TeacherBookList { get; set; }
        public virtual List<FieldBookList> FieldBookList { get; set; }


        #endregion Relations
    }
}