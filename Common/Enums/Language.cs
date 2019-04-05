using System.ComponentModel.DataAnnotations;

namespace Common
{
    public enum Language
    {
        [Display(Name = "فارسی")]
        Persion = 1,

        [Display(Name = "انگلیسی")]
        English = 2
    }
}