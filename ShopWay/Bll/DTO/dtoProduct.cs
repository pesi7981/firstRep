using Bll.Utilities;
using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.DTO
{
    public class dtoProduct
    {
        public int Code { get; set; }
        public string Barcode { get; set; }
        public int CodeAlias { get; set; }
        public string Company { get; set; }
        public string Size { get; set; }
        public string Src { get; set; }
        public dtoAlias Alias { get; set; }
        public virtual List<dtoProductAlias> ProductAlias { get; set; }





        public static List<dtoProduct> GetDtoProduct()
        {
            GeneralDB generalDB = new GeneralDB();
            return Converts.ToDtoProducts(generalDB.MyDb.Products.ToList());
        }

        public dtoProduct Insert()
        {
            Product s = Converts.ToProduct(this);
            //s.Alias = null;
            GeneralDB generaldb = new GeneralDB();
            generaldb.MyDb.Products.Add(s);
            generaldb.MyDb.SaveChanges();
            s = generaldb.MyDb.Products.ToList().Last();
            dtoProduct d = Converts.ToDtoProduct(s);
            return d;
        }

        public static void Update(List<dtoProduct> products)
        {
            GeneralDB dB = new GeneralDB();
            List<Product> products2 = dB.MyDb.Products.ToList();
            products.ForEach(x => products2.Where(y => y.Code == x.Code).First().Src = x.Src);
            dB.MyDb.SaveChanges();
        }
    }
}
