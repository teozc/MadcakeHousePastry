using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MadcakeHousePastry.Models
{
    public class Pastry
    {
        public int PastryID { get; set; }
        public string PastryName { get; set; }
        public DateTime PastryProducedDate { get; set; }
        public string PastryType { get; set; }
        public decimal PastryPrice { get; set; }
    }
}
