using AutoMapper;
using Common;
using Common.Enums;
using Common.Utilities.Validation;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Dto
{
    public class StudentDto : BaseDto<StudentDto, Student, string>,IValidatableObject
    {
        [DisplayName("شماره دانشجویی")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string Code { get; set; }

        [DisplayName("سال ورود")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public int EntryYear { get; set; }


        [DisplayName("روزانه / شبانه")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public StudentStatus StudentStatus { get; set; }


        [DisplayName("کاربر")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string UserId { get; set; }

        [DisplayName("رشته")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public int FieldId { get; set; }

        public override void CustomMappings(IMappingExpression<Student, StudentDto> mapping)
        {
            mapping.ForMember(des => des.EntryYear, opt => opt.MapFrom(src=>int.Parse(src.Code.Substring(0, 2))));
        }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (!Validation.StudentNumber(Code))
                yield return new ValidationResult("کد دانشجویی را صحیح وارد کنید", new[] { nameof(Code) });
        }
    }
    public class StudentSelectDto : BaseDto<StudentSelectDto, Student, string>
    {
        [DisplayName("شماره دانشجویی")]
        public string Code { get; set; }


        [DisplayName("روزانه / شبانه")]
        public StudentStatus StudentStatus { get; set; }


        [DisplayName("شناسه کاربر")]
        public string UserId { get; set; }

        [DisplayName("شناسه رشته")]
        public int FieldId { get; set; }

        [DisplayName(" نام و نام خانوادگی")]
        public string StudentFullName { get; set; }

        [DisplayName("رشته تحصیلی")]
        public string FiledName { get; set; }

        [DisplayName("مبلغ جریمه")]
        public decimal PenaltyAmount { get; set; }

        public override void CustomMappings(IMappingExpression<Student, StudentSelectDto> mapping)
        {
            mapping.ForMember(des => des.StudentFullName, opt => opt.MapFrom(src => src.User.FullName));
        }


    }
}
