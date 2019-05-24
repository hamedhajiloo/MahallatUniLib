using Common;
using Common.Enums;
using Common.Utilities.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Book : BaseEntity
    {
        public Book()
        {
            Language = Language.Persion;
            CourseType = CourseType.General;
            BookStatus = BookStatus.Free;
            IsDeleted = false;
        }
        #region Properties
        
       
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string ISBN { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name ="تصویر")]
        public string ImageUrl { get; set; }


        [DisplayName("نام کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string Name { get; set; }

        [DisplayName("نویسنده")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string AuthorName { get; set; }

        [DisplayName("ناشر")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string Publisher { get; set; }

        [DisplayName("سال انتشار")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public int PublishYear { get; set; }

        [DisplayName("نوبت چاپ")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        [Range(minimum:1,maximum:1000000,ErrorMessage =DataAnotations.Range)]
        public int Edition { get; set; }

        [DisplayName("زبان")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public Language Language { get; set; }

        [DisplayName("نوع درس")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public CourseType CourseType { get; set; }

        [DisplayName("وضعیت کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public BookStatus BookStatus { get; set; }

        #endregion Properties

        #region Relations

        public virtual List<StudentBookList> StudentBookList { get; set; }
        public virtual List<TeacherBookList> TeacherBookList { get; set; }
        public virtual List<FieldBookList> FieldBookList { get; set; }
        public virtual List<Penalty> Penalties { get; set; }



        #endregion Relations
    }

    #region Config
    //public class BookConfiguration : IEntityTypeConfiguration<Book>
    //{
    //    public void Configure(EntityTypeBuilder<Book> builder)
    //    {
    //       // builder.HasKey(c => new { c.Id, c.ISBN });
    //    }
    //}
    #endregion
}