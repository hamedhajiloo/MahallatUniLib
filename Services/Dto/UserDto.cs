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
}