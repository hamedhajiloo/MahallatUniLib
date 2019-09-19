using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Admin.Models
{
    public class IndexModel
    {
        public int AllBook { get; set; }
        public int FreeBook { get; set; }
        public int ReserveBook { get; set; }
        public int BorrowedBook { get; set; }
        ////////////////////////////////////

        public int AllStudent { get; set; }
        //public int FreeStudent { get; set; }
        //public int ReserveStudent { get; set; }
        //public int BorrowedStudent { get; set; }

        ////////////////////////////////////


        public int Teacher { get; set; }
        public int Personel { get; set; }

    }
}
