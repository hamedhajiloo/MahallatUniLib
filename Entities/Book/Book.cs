using Common;
using Common.Enums;
using Common.Utilities.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Book : BaseEntity
    {
        public Book()
        {
            BookIsDeleted = false;
        }
        #region Properties
       
        [Required(ErrorMessage = DataAnotations.EnterMessage)]
        public string ISBN { get; set; }

        public bool BookIsDeleted { get; set; }

        #endregion Properties

        #region Relations
        [ForeignKey(nameof(BookList))]
        public int BookListId { get; set; }
        public virtual BookList BookList { get; set; }
        #endregion Relations
    }
}