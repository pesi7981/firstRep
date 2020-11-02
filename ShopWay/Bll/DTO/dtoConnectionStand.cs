using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
namespace Bll.DTO
{
    public class dtoConnectionStand
    {
        public int Code { get; set; }
        public int CodeSource { get; set; }
        public int CodeDest { get; set; }
        public double Distance { get; set; }
        public bool Nearest { get; set; }
        public  dtoStand Stand { get; set; }
        public  dtoGetaway Getaway { get; set; }
    }
}
