using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Enums
{
    public enum StudentStatus
    {
        [Display(Name = "نامشخص")]
        None = 0,


        [Display(Name = "روزانه")]
        Daily = 1,


        [Display(Name = "شبانه")]
        Nightly = 2,



    }
}
