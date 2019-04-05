using System.ComponentModel.DataAnnotations;

namespace Common
{
    public enum CourseType
    {
        [Display(Name = "عمومی")]
        General = 1,

        [Display(Name = "اختصاصی")]
        Special = 2
    }
}