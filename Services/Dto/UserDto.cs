using Common;
using Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Services.Dto
{
    public class UserDto : BaseIEntityDto<UserDto, User, string>
    {
        [DisplayName("نام و نام خانوادگی")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        [StringLength(100)]
        public string FullName { get; set; }

        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        [StringLength(100)]
        public string UserName { get; set; }
    }


    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "رمز عبور را وارد نمایید")]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = " تکرار عبور رمز را وارد نمایید")]
        [Display(Name = " تکرار رمز عبور")]
        [Compare(nameof(Password), ErrorMessage = "رمز عبور با تکرار آن یکسان نیست")]
        public string ConfirmPassword { get; set; }

        public string UserId { get; set; }


    }
}