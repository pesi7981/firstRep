using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.DTO
{
   public class dtoShelf
    {
        public int Code { get; set; }
        public int CodeStand { get; set; }
        public int Num { get; set; }
        public int? CodeAlias { get; set; }

        public  List<dtoProductShelf> ProductShelves { get; set; }
        public  dtoAlias Alias { get; set; }


        public static List<dtoShelf> GetDtoShelves()
        {
            throw new NotImplementedException();
        }

        public void Insert()
        {
            throw new NotImplementedException();
        }
    }
}
