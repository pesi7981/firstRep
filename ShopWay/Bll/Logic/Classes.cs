using Bll.DTO;
using Bll.Utilities;
using Dal;
using System;
using System.Collections.Generic;

using System.Linq;

namespace Bll.Logic
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x,int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point()
        {
        }
    }
    public class Matrix
    {
        public Cell[][] Mat { get; set; }
    }

    #region class Cell
    public class Cell
    {
        public int i { get; set; }/*מקור*/
        public int j { get; set; }/*יעד*/
        public int distance { get; set; }
        public int Parent { get; set; }/*צריך לבדוק איך האבא הגיע ליעד, משנים את המקור*/
    }
    #endregion
    #region class KeyArea
    public class KeyArea
    {
        public List<Getaway> Nearestes { get; set; }
        public List<GetawayProcI_Result> Getaways { get; set; }
        public override bool Equals(object obj)
        {
            KeyArea k = this;
            KeyArea key = (KeyArea)obj;
            if (k == null || key == null||k.Nearestes==null) return false;
            foreach (var item in key.Getaways)
            {
                if (this.Getaways.Contains(item) == false)
                    return false;
            }

            foreach (var item in key.Nearestes)
                if (this.Nearestes.Contains(item) == false)
                    return false;
            if (Getaways.Count() != key.Getaways.Count || Nearestes.Count() != key.Nearestes.Count)
                return false;
            return true;
        }
    }
    #endregion
    public class Goal
    {
        public int num;
        public char kind;
        public List<dtoProduct> products { get; set; }
        public Point midPoint;
        public Goal(char kind, int num, List<dtoProduct> products, Point p)
        {
            this.kind = kind;
            this.num = num;
            this.products = products;
            midPoint = p;
           
        } 

    }

    //public class OrderStand { public Stand s { get; set; } public List<ProductShop> listproduct { get; set; } public int dist { get; set; } }

    public interface I2Points
    {
        Point P1 { get; set; }
        Point P2 { get; set; }
    }

    public class Area : I2Points
    {
        public Area()
        {
            this.P1 = new Point();
            this.P2 = new Point();
            this.ExtraStand = new Dictionary<Stand, List<Product>>();
        }
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public Dictionary<Stand, List<Product>> ExtraStand { get; set; }
        public void calculatePoints()
        {
            int xS = Convert.ToInt32(ExtraStand.Keys.Min(x => x.X1));
            int yS = Convert.ToInt32(ExtraStand.Keys.Min(x => x.Y1));
            P1 = new Point() { X = xS, Y = yS };
            P2 = new Point()
            {
                X = Convert.ToInt32(ExtraStand.Keys.Max(y => y.X2)),
                Y = Convert.ToInt32(ExtraStand.Keys.Max(y => y.Y2))
            };
        }

        private void calculateDistances(Point closer)
        {
            Point mid;
            foreach (Stand s in ExtraStand.Keys)
            {
                mid = UtilitiesFunctions.MidPoint(new Point() { X = Convert.ToInt32(s.X1), Y = Convert.ToInt32(s.Y1.ToString()) }, new Point { X = Convert.ToInt32(s.X2), Y = Convert.ToInt32(s.Y2) });
                s.Distance = UtilitiesFunctions.CalculteDist(mid, closer);
            }
        }

        public List<int> sortStands(I2Points g)
        {
            dtoStand stand;
            List<int> arr = new List<int>
            {
                Capacity = ExtraStand.Count()
            };
            Point midOfG = UtilitiesFunctions.MidPoint(g.P1, g.P2);
            Point closerer;
            if (UtilitiesFunctions.CalculteDist(midOfG, P1) < UtilitiesFunctions.CalculteDist(midOfG, P2))
                closerer = P1;
            else
                closerer = P2;
            for (int i = 0; i < ExtraStand.Count(); i++)
            {
                /* מחשב מרחק לכל סטנד*/
                calculateDistances(closerer);
                ExtraStand.Keys.ToList().Sort();
                stand = Converts.ToDtoStand(ExtraStand.Keys.Where(x => !arr.Contains(x.Code)).First());
                arr.Add(stand.Code);
                closerer = UtilitiesFunctions.MidPoint(stand.P1, stand.P2);
            }
            return arr;
        }
    }
    public enum eMatMeaning { empy, wall, stand, getaway }
    public enum eTypeConnection { wall, getaway, stand }

    public class ExtraAlias : dtoAlias
    {
        public List<dtoProduct> products {
            get;
            set;
        }

        public static List<ExtraAlias> GetAliasesShop(int codeShop)
        {
            GeneralDB generalDB = new GeneralDB();
            Shop s = generalDB.MyDb.Shops.Where(x => x.Code == codeShop).First();
            List<ProductShop> productShops = s.ProductShops;
            Product pr = productShops[0].Product;
            List<Product> products = s.ProductShops.Select(x=>x.Product).ToList();
            List<ExtraAlias> extraAliases = new List<ExtraAlias>();
            ExtraAlias e;
            foreach (var item in products)
            {
                if (item.Code == 191)
                    s = s;
                foreach (var item1 in item.ProductAlias)
                {
                     e = Converts.ToExtraAlias(item1.Alias);
                    if (item1.Alias.Parent == null&&item1.Alias.Color!=null)
                        e.Color = item1.Alias.Color;
                   else if(item1.Alias.Parent != null)
                        e.Color = item1.Alias.Alias2.Color;
                    extraAliases.Add(Converts.ToExtraAlias(item1.Alias));
                }
                extraAliases.Add(Converts.ToExtraAlias(item.Alias));
            }
            extraAliases.Distinct();
            extraAliases = extraAliases
                .GroupBy(p => p.Code)
                .Select(g => g.First())
                .ToList();
            GetAliasesShopProduct(codeShop, extraAliases,products);
            return extraAliases;
        }

        public static void GetAliasesShopProduct(int codeShop, List<ExtraAlias> extraAliases,List<Product> products)
        {
           GeneralDB generalDB = new GeneralDB();
            List<dtoProduct> dtoProducts = Converts.ToDtoProducts(products);
            ExtraAlias e;
            int? codeAlias;
            foreach (var product in dtoProducts)
            {
                codeAlias = product.CodeAlias;
                e = extraAliases.Where(eAlias => eAlias.Code == codeAlias).First();
                e.products.Add(product);
                if(product.ProductAlias!=null)
                product.ProductAlias.ForEach(productAlias =>extraAliases.Where(eAlias => eAlias.Code == productAlias.Alias.Code).ToList().First().products.Add(product));
            }
        }

        public static List<ExtraAlias> GetDepartment()
        {
           GeneralDB generalDB = new GeneralDB();
            List<Alias> aliases = generalDB.MyDb.Aliases.Where(x => x.Parent == null).ToList();
            List<ExtraAlias> departments = Converts.ToExtraAliases(aliases);
            departments.Remove(departments.Where(x => x.TextAlias == "placeAlias").FirstOrDefault());

            List<dtoProduct> dtoProducts = Converts.ToDtoProducts(generalDB.MyDb.Products.ToList());
            var q = dtoProducts.Where(x => x.Alias.TextAlias == "cash" || x.Alias.TextAlias == "wall" || x.Alias.TextAlias == "סטנד").ToList();
            q.ForEach(x=>dtoProducts.Remove(x));
            ExtraAlias e;
            foreach (var item in dtoProducts)
            {
                if(item.Alias.Parent !=  null&&item.Alias.Parent!=0)
                {
                e = departments.Where(x => x.Code == item.Alias.Parent).First();
                e.products.Add(item);
                }
            }
            //departments = departments.Where(x => x.products.Count>0).ToList();
            return departments;

        }
    }

}

