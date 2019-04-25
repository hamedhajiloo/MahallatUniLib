using System.ComponentModel.DataAnnotations;

namespace Common
{
    public enum GenderType
    {
        [Display(Name = "نامشخص")]
        None = 0,

        [Display(Name = "مرد")]
        Male = 1,

        [Display(Name = "زن")]
        Female = 2
    }
}