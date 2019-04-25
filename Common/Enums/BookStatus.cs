using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Enums
{
    public enum BookStatus
    {

        [Display(Name = "نامشخص")]
        None=0,

        [Display(Name = "آزاد")]
        Free = 1,

        [Display(Name = "رزرو شده")]
        Reserved = 2,

        [Display(Name = "امانت داده شده")]
        Borrowed = 3

    }
}
