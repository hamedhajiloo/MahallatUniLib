using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Model
{
    public class DashBoardModel
    {
        public int ReserveCount { get; set; }
        public int BorrowCount { get; set; }
        public decimal ReservePunishAmount { get; set; }
        public decimal BorrowPunishAmount { get; set; }
        public string ReserveLastupdate { get; set; }
        public string BorrowLastUpdate { get; set; }

    }
}
