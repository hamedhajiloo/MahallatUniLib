﻿using AutoMapper;
using Common;
using Common.Enums;
using Common.Utilities;
using Common.Utilities.Validation;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Services.Dto
{
    public class BookDto : BaseDto<BookDto, Book>/*, IValidatableObject*/
    {
        public BookDto()
        {
            BookStatus = BookStatus.Free;
        }
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

        [DisplayName("نوع درس")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public CourseType CourseType { get; set; }


        [DisplayName("وضعیت کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public BookStatus BookStatus { get; set; }

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

        [DisplayName("شابک")]
        public string ISBN { get; set; }

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


        [Display(Name = "نوع درس")]
        public string CourseType { get; set; }


        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }


        [DisplayName("وضعیت کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public BookStatus BookStatus { get; set; }


        [DisplayName("وضعیت کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string BookStatusNum { get; set; }

        public override void CustomMappings(IMappingExpression<Book, BookSelectDto> mapping)
        {
            mapping.ForMember(des => des.CourseType, opt => opt.MapFrom(src => src.CourseType.ToDisplay(DisplayProperty.Name)));
            mapping.ForMember(des => des.Language, opt => opt.MapFrom(src => src.Language.ToDisplay(DisplayProperty.Name)));
            mapping.ForMember(des => des.BookStatusNum, opt => opt.MapFrom(src => src.BookStatus.ToDisplay(DisplayProperty.Name)));
        }

    }


}