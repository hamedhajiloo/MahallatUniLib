using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model
{
    public class PenaltySelectModel
    {
        [Display(Name ="شناسه کاربری")]
        public string UserId { get; set; }

        [Display(Name ="نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "کاربر")]
        public string FullName { get; set; }

        [Display(Name = "کتاب")]
        public string BookName { get; set; }

        [Display(Name = "نویسنده")]
        public string BookAuthor { get; set; }

        [Display(Name = "شناسه کتاب")]
        public int BookId { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime InsertDate { get; set; }

        [Display(Name = "تاریخ")]
        public string InsertDateP { get; set; }

        [Display(Name = "مبلغ")]
        public decimal Amount { get; set; }
    }
}
