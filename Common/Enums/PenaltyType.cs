using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Enums
{
    public enum PenaltyType
    {
        [Display(Name = "نامشخص")]
        None = 0,

        [Display(Name = "جریمه رزرو")]
        Reserve = 1,

        [Display(Name = "جریمه برگرداندن امانت")]
        Return = 2,
    }
}
