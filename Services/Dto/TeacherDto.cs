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
    public class TeacherDto : BaseDto<TeacherDto, Teacher, string>
    {

        [DisplayName("نام و نام خانوادگی")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        [StringLength(100)]
        public string UserFullName { get; set; }

        [DisplayName("کاربر")]
        public string UserId { get; set; }

        [DisplayName("کد ملی")]
        public string NationalCode { get; set; }


    }

}
