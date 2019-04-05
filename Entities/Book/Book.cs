using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Book : BaseEntity
    {
        #region Properties
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string AuthorName { get; set; }
        #endregion


    }
}
