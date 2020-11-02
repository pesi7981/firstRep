using Bll.Logic;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.DTO
{
    public class dtoGetaway:I2Points
    {
        public int Code { get; set; }
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public int CodeShop { get; set; }
        public int I { get; set; }

        public static List<dtoGetaway> GetDtoGetaways()
        {
            throw new NotImplementedException();
        }

        public void Insert()
        {
            throw new NotImplementedException();
        }
    }
}
