﻿using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Setting : BaseEntity
    {
        /// <summary>
        /// جریمه برای برگرداندن کتاب
        /// </summary>
        //[DataType(DataType.Currency)]
        [Display(Name = "جریمه امانت دادن کتاب")]
        public decimal Amount_Of_Punishment_For_Returning_The_Book { get; set; }

        /// <summary>
        /// جریمه برای رزرو کتاب
        /// </summary>
        //[DataType(DataType.Currency)]
        [Display(Name = "جریمه رزرو کتاب")]
        public decimal Amount_Of_Punishment_For_Reserving_The_Book { get; set; }

        /// <summary>
        /// تعداد کتابی که هر نفر می تواند رزرو کند
        /// </summary>
        [Display(Name = "تعداد کتابی که هر نفر میتواند رزرو کند")]
        public int ReservCount { get; set; }


        /// <summary>
        /// تعداد کتابی که هر نفر می تواند امانت بگیرد
        /// </summary>
        [Display(Name = "تعداد کتابی که هر نفر میتواند امانت بگیرد")]
        public int BorrowCount { get; set; }

        /// <summary>
        /// تعداد روز هایی که کتاب میتواند رزرو شود
        /// </summary>
        [Display(Name = "تعداد روز هایی که کتاب می تواند رزرو شود")]
        public int ReservDay { get; set; }

        /// <summary>
        /// تعداد روز هایی که کتاب میتواند امانت داده شود
        /// </summary>
        [Display(Name = "تعداد روز هایی که کتاب می تواند امانت داده شود")]
        public int BorrowDay { get; set; }

        [Display(Name = "از چند روز قبل باید رزرو باشد؟")]
        public int BDay4Reserve { get; set; }
    }
}