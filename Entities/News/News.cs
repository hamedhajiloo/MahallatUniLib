using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.News
{
    public class News: BaseEntity
    {
        [Display(Name ="عنوان")]
        public string Title { get; set; }

        [Display(Name = "متن ")]
        public string Message { get; set; }

        [Display(Name = "لینک ")]
        public string Link { get; set; }

        [Display(Name = "تصویر ")]
        public string Picture { get; set; }

        public string ThumbNail { get; set; }
        public bool Deleted { get; set; }


        public DateTime InsertDate { get; set; }

        [Display(Name ="تاریخ ثبت")]
        public string InserDateP { get; set; }

    }
}
