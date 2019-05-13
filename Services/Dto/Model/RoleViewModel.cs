using Common;
using Entities;
using System.ComponentModel.DataAnnotations;

namespace Services.Dto
{
    public class RoleViewModel:BaseIEntityDto<RoleViewModel,Role,string>
    {
        [Display(Name = "عنوان نقش(انگلیسی)")]
        [Required(AllowEmptyStrings = false, ErrorMessage =DataAnotations.EnterMessage)]
        public string Name { get; set; }

        [Display(Name = "نام بخش")]
        [Required(AllowEmptyStrings = false, ErrorMessage = DataAnotations.EnterMessage)]
        public string Description { get; set; }

        [Display(Name = "زیردسته ها")]
        public string RoleLevel { get; set; }
    }
}
