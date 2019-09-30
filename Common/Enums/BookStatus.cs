using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Enums
{
    public enum BookStatus
    {

        [Display(Name = "آزاد")]
        Free = 0,

        [Display(Name = "رزرو شده")]
        Reserved = 1,

        [Display(Name = "امانت داده شده")]
        Borrowed = 2

    }
}
