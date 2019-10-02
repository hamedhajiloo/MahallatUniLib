using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Enums
{
    public enum PenaltyType
    {
        /// <summary>
        /// جریمه رزرو
        /// </summary>
        [Display(Name = "جریمه رزرو")]
        Reserve = 0,

        /// <summary>
        /// جریمه برگرداندن امانت
        /// </summary>
        [Display(Name = "جریمه برگرداندن امانت")]
        Return = 1,
    }
}
