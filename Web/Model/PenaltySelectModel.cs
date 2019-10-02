using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model
{
    public class PenaltySelectModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public int BookId { get; set; }
        public DateTime InsertDate { get; set; }
        public string InsertDateP { get; set; }
        public decimal Amount { get; set; }
    }
}
