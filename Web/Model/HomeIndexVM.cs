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
        public IList<Book> ComputerBooks { get; set; }
        public IList<Book> SanayeBooks { get; set; }
        public IList<Book> OmranBooks { get; set; }
        public IList<Book> MechanickBooks { get; set; }
        public IList<Book> UlomBooks { get; set; }
        public IList<Book> Memary { get; set; }
        public IList<Book> General { get; set; }
    }
}
