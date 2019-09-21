using AutoMapper;
using Common;
using Common.Enums;
using Common.Utilities;
using Common.Utilities.Validation;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Services.Dto
{
    public class BookDto : BaseDto<BookDto, Book>/*, IValidatableObject*/
    {
        public BookDto()
        {
            BookStatus = BookStatus.Free;
            BooksISBN = new List<string>();
        }
        [DisplayName("نام کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string Name { get; set; }



        [DisplayName("شناسه کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public List<string> BooksISBN { get; set; }



        [DisplayName("نویسنده")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string AuthorName { get; set; }



        [DisplayName("ناشر")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string Publisher { get; set; }



        [DisplayName("سال انتشار ")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public int? PublishYear { get; set; }



        [DisplayName("نوبت چاپ")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        [Range(minimum: 1, maximum: 1000000, ErrorMessage = DataAnotations.Range)]
        public int? Edition { get; set; }



        [DisplayName("زبان")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public Language Language { get; set; }




        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }


        [DisplayName("وضعیت کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public BookStatus BookStatus { get; set; }


        [DisplayName("رشته")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public FieldStatus FieldId { get; set; }

        public override void CustomMappings(IMappingExpression<Book, BookDto> mapping)
        {
            mapping.ForMember(des => des.BooksISBN, opt => opt.MapFrom(src => src.ISBNs.Select(p=>p.Value).ToList()));
            mapping.ForMember(des => des.FieldId,opt=>opt.Ignore());

        }
        //public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        //{
        //    if (!Validation.ISBN(ISBN))
        //        yield return new ValidationResult("شابک را صحیح وارد نمایید", new[] { nameof(ISBN) });
        //}
    }
    public class BookSelectDto : BaseDto<BookSelectDto, Book>
    {
        [DisplayName("نام کتاب")]
        public string Name { get; set; }

        [DisplayName("نویسنده")]
        public string AuthorName { get; set; }

        [DisplayName("ناشر")]
        public string Publisher { get; set; }

        [DisplayName("سال انتشار ")]
        public int PublishYear { get; set; }


        [DisplayName("نوبت چاپ")]
        public int Edition { get; set; }

        [DisplayName("زبان")]
        public string Language { get; set; }




        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }


        [DisplayName("وضعیت کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public BookStatus BookStatus { get; set; }


        //[DisplayName("وضعیت کتاب")]
        //[Required(ErrorMessage = DataAnotations.EnterMessage)]
        //public string BookStatusNum { get; set; }

        public List<Isbn> Isbns { get; set; }

        [DisplayName("تعداد کتاب")]
        public int Count { get; set; }


        [DisplayName("رشته")]
        public string Field { get; set; }


        public override void CustomMappings(IMappingExpression<Book, BookSelectDto> mapping)
        {
            mapping.ForMember(des => des.Language, opt => opt.MapFrom(src => src.Language.ToDisplay(DisplayProperty.Name)));
            mapping.ForMember(des => des.Field, opt => opt.MapFrom(src => src.Field.Name));
           // mapping.ForMember(des => des.BookStatusNum, opt => opt.MapFrom(src => src.BookStatus.ToDisplay(DisplayProperty.Name)));
            mapping.ForMember(des => des.Count, opt => opt.MapFrom(src => src.ISBNs.Where(c=>c.IsDeleted==false).Count()));
            mapping.ForMember(des => des.Isbns, opt => opt.MapFrom(src => src.ISBNs.Where(c=>c.IsDeleted==false)));
        }

    }

    public enum FieldStatus
    {
        /// <summary>
        /// عمومی
        /// </summary>
        [Display(Name = "عمومی")]
        N =1,

        /// <summary>
        /// کامپیوتر
        /// </summary>
        [Display(Name = "کامپیوتر")]
        C =2,


        /// <summary>
        /// علوم کامپیوتر
        /// </summary>
        [Display(Name = "علوم کامپیوتر")]
        PC =3,

        /// <summary>
        /// صنایع
        /// </summary>
        [Display(Name = "صنایع")]
        S=4,

        /// <summary>
        /// مکانیک
        /// </summary>
        [Display(Name = "مکانیک")]
        M = 5,

        /// <summary>
        /// عمران
        /// </summary>
        [Display(Name = "عمران")]
        O = 6
    }
}