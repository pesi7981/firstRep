using Bll.Logic;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bll.Utilities;

namespace Bll.DTO
{
    public class dtoStand:I2Points
    {
        public int Code { get; set; }
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public int CodeShop { get; set; }
        public int? CodeAlias { get; set; }



        public  dtoAlias Alias { get; set; }
        public List<dtoShelf> Shelves { get; set; }

        public static List<dtoStand> GetDtoStands()
        {
            throw new NotImplementedException();
        }


        public void Insert()
        {
            throw new NotImplementedException();
        }
    }
}
