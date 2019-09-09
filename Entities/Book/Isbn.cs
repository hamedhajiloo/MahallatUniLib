using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class Isbn:BaseEntity
    {
        public string Value { get; set; }


        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public bool IsDeleted { get; set; }


    }
}
