using AutoMapper;
using Common;
using Common.Enums;
using Common.Utilities;
using Common.Utilities.Validation;
using Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Services.Dto
{
    public class StudentDto : BaseDto<StudentDto, Student, string>, IValidatableObject
    {

        [DisplayName("نام و نام خانوادگی")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        [StringLength(100)]
        public string FullName { get; set; }

        [DisplayName("شماره دانشجویی")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string UserName { get; set; }

        [DisplayName("سال ورود")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public int EntryYear { get; set; }


        [DisplayName("روزانه / شبانه")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public StudentStatus StudentStatus { get; set; }


        [DisplayName("کاربر")]
        public string UserId { get; set; }

        [DisplayName("رشته")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public int FieldId { get; set; }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (!Validation.StudentNumber(UserName))
                yield return new ValidationResult("کد دانشجویی را صحیح وارد کنید", new[] { nameof(UserName) });
        }
    }
    public class StudentSelectDto : BaseDto<StudentSelectDto, Student, string>
    {
        [DisplayName("شماره دانشجویی")]
        public string UserName { get; set; }


        [DisplayName("روزانه / شبانه")]
        public string StudentStatus { get; set; }


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
            mapping.ForMember(des => des.UserName, opt => opt.MapFrom(src => src.User.UserName));
            mapping.ForMember(des => des.StudentStatus, opt => opt.MapFrom(src => src.StudentStatus.ToDisplay(DisplayProperty.Name)));
            mapping.ForMember(des => des.FiledName, opt => opt.MapFrom(src => src.Field.Name));
        }


    }
}
