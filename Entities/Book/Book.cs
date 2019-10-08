using Common;
using Common.Enums;
using Common.Utilities.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sepehran.Pooshako.Utilities.Attribute;
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
            BookIsDeleted = false;
            Language = Language.Persion;
        }
        #region Properties
       


        public bool BookIsDeleted { get; set; }

        [EnsureOneElement(ErrorMessage ="شابک را وارد کنید")]
        public List<Isbn> ISBNs { get; set; }

        [Display(Name = "تصویر")]
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
        [Range(minimum: 1, maximum: 1000000, ErrorMessage = DataAnotations.Range)]
        public int Edition { get; set; }

        [DisplayName("زبان")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public Language Language { get; set; }

        [ForeignKey(nameof(Field))]
        public int FieldId { get; set; }
        public virtual Field Field { get; set; }


        #endregion Properties

        #region Relations

        public virtual List<ReserveBook> StudentBook { get; set; }
        public virtual List<Penalty> Penalties { get; set; }

        #endregion Relations
    }
}