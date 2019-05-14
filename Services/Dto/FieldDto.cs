using Common;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Dto
{
    public class FieledCreateDto:BaseDto<FieledCreateDto,Field>
    {
        [DisplayName("عنوان انگلیسی")]
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string Name { get; set; }
    }


}
