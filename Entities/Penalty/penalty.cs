using Common;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class Penalty : BaseEntity
    {
        [DisplayName("نوع جریمه")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public PenaltyType PenaltyType { get; set; }

        [DisplayName("مبلغ")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public decimal Amount { get; set; }

        [DisplayName("کاربر")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual User User { get; set; }


        [DisplayName("کتاب")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public DateTime InsertDate { get; set; }


    }
}
