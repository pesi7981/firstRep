using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Utilities;
using Dal;

namespace Bll.DTO
{
    public class dtoProductShelf
    {
        public int Code { get; set; }
        public int? CodeProduct { get; set; }
        public int CodeShelf { get; set; }

        public dtoProduct Product { get; set; }

        public static void Insert(List<dtoProductShelf> productShelves)
        {
            GeneralDB dB = new GeneralDB();
            productShelves.ForEach(x =>x.CodeProduct= x.CodeProduct==0?null:x.CodeProduct);
            List<ProductShelf> productShelves2 = Converts.ToProductShelves(productShelves);
            dB.MyDb.ProductShelves.AddRange(productShelves2);
            dB.MyDb.SaveChanges();
        }

        public static void Update(List<dtoProductShelf> updateProductShelves,int codeShop)
        {
            try
            {
            GeneralDB dB = new GeneralDB();
                List<ProductShelf> productShelves2 = dB.MyDb.ProductShelves.Where(x => x.Shelf.Stand.CodeShop == codeShop).ToList();
            //List<ProductShelf> productShelves2 = dB.MyDb.ProductShelves.ToList();
           // var v = updateProductShelves.Select(p => productShelves2.Where(x => x.CodeProduct == p.CodeProduct ).ToList()).ToList();
           // v.ForEach(x => productShelves2.AddRange(x));
                foreach (var updateItem in updateProductShelves)
                {
                    var item = productShelves2.Where(x => x.CodeProduct == updateItem.CodeProduct).FirstOrDefault();
                 if(item!=null)
                        item.CodeShelf = updateItem.CodeShelf;
                    }
            //productShelves2.ForEach(p => p.CodeShelf= updateProductShelves.Where(x => x.CodeProduct == p.CodeProduct && x.CodeShelf == p.CodeShelf).ToList().First().CodeShelf );        
            dB.MyDb.SaveChanges();
            }
            catch(Exception e)
            {
                MessageBox.Show("problem cas update");
            }

        }
    }
}
