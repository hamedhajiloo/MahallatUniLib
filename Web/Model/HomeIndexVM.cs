using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model
{
    public class HomeIndexVM
    {
        public IList<Field> Fields { get; set; }
        public IList<BookList> ComputerBooks { get; set; }
        public IList<BookList> SanayeBooks { get; set; }
        public IList<BookList> OmranBooks { get; set; }
        public IList<BookList> MechanickBooks { get; set; }
        public IList<BookList> UlomBooks { get; set; }
        public IList<BookList> Memary { get; set; }
        public IList<BookList> General { get; set; }
    }
}
