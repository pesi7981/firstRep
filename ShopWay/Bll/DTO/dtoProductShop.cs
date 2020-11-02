using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Bll.Utilities;
using System.Data.Entity;
namespace Bll.DTO
{
    public class dtoProductShop
    {
        public int Code { get; set; }
        public int CodeProduct { get; set; }
        public int CodeShop { get; set; }
        //public int IsExist { get; set; }


        public dtoProduct Product { get; set; }




        public static List<dtoProductShop> GetDtoProductShop()
        {

            GeneralDB generalDB = new GeneralDB();
            return Converts.ToDtoProductShops(generalDB.MyDb.ProductShops.ToList());
        }

        public bool Insert()
        {
            ProductShop s = Converts.ToProductShop(this);

            GeneralDB generalDB = new GeneralDB();
            generalDB.MyDb.ProductShops.Add(s);
            generalDB.MyDb.SaveChanges();
            return true;
        }

        public static List<dtoProductShop> GetProdcutsShop(int code)
        {
            GeneralDB generalDB = new GeneralDB();
            List<dtoProductShop> l =Converts.ToDtoProductShops( generalDB.MyDb.ProductShops.ToList());
            l = l.Where(x => x.CodeShop == code).ToList();
            return l;
        }

        public static void Delete(List<dtoProductShop> deleteProductShops)
        {
            GeneralDB generalDB = new GeneralDB();
            foreach (var item in deleteProductShops)
            {
            ProductShelf productShelf = Converts.ToProductShelf(item);
             var v=   generalDB.MyDb.ProductShelves.Where(x => x.Code == productShelf.Code).FirstOrDefault();
                if (v!=null)
            generalDB.MyDb.ProductShelves.Remove(v);
            }
            generalDB.MyDb.SaveChanges();
        }

        //public static void Insert(int codeShop, List<dtoProduct> products)
        //{
        //    GeneralDB db = new GeneralDB();
        //    var v = db.MyDb.Shops.ToList().Where(x => x.Code == codeShop);
        //    Shop s = v.First();
        //    products.ForEach(x => s.ProductShops.Add(new ProductShop() { CodeProduct = x.Code, IsExist = 1, Product = Converts.ToProduct(x) }));
        //    db.MyDb.SaveChanges();
        //}

        //public static void Insert(List<dtoProductShop> productShops)
        //{
        //    GeneralDB db = new GeneralDB();
        //    productShops.ForEach(x => x.Product = null);
        //    List<ProductShop> productShops2 = Converts.ToProductshops(productShops);
        //    db.MyDb.ProductShops.AddRange(productShops2);
        //    try
        //    {
        //        db.MyDb.SaveChanges();
        //    }
        //    catch(Exception e)
        //    {
        //        throw e;
        //    }

        //}
    }
}
