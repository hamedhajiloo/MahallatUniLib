using System.ComponentModel.DataAnnotations;

namespace Common
{
    public enum CourseType
    {
        [Display(Name = "نامشخص")]
        None = 0,

        [Display(Name = "عمومی")]
        General = 1,

        [Display(Name = "اختصاصی")]
        Special = 2
    }
}