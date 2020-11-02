using Bll.Utilities;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.DTO
{
    public class dtoProductAlias
    {
        public int Code { get; set; }
        public int CodeProduct { get; set; }
        public int? CodeAlias { get; set; }

        public dtoAlias Alias { get; set; }
        public dtoProduct Product { get; set; }

        public static void Insert(List<dtoProductAlias> productAliases)
        {
            GeneralDB dB = new GeneralDB();
            productAliases.ForEach(x => x.Alias = null);
            List<ProductAlia> productAliases2 = Converts.ToProductAliases(productAliases);
            dB.MyDb.ProductAlias.AddRange(productAliases2);
            dB.MyDb.SaveChanges();
        }
    }
}
