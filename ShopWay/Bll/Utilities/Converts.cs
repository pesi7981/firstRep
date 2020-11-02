using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dal;
using Bll.DTO;
using System.Data.Entity;
using Bll.Logic;
using System.Windows.Forms;

namespace Bll.Utilities
{
    public static class Converts
    {
        #region Alias
        public static Alias ToAlias(dtoAlias dtoAlias)
        {
            if (dtoAlias == null) return null;
            return new Alias() { Code = dtoAlias.Code, IsPrivate = dtoAlias.IsPrivate, Products = null, Shelves = null, Stands = null, TextAlias = dtoAlias.TextAlias, Walls = null, Parent = dtoAlias.Parent,Color=dtoAlias.Color };
        }
        public static dtoAlias ToDtoAlias(Alias Alias)
        {
            if (Alias == null) return null;
            return new dtoAlias() { Code = Alias.Code, TextAlias = Alias.TextAlias, IsPrivate = Alias.IsPrivate,  Parent = Alias.Parent,Color=Alias.Color };
        }
        public static List<dtoAlias> ToDtoAliases(List<Alias> Alias)
        {
            if (Alias == null)
                return null;
            List<dtoAlias> dtoAlias = new List<dtoAlias>();
            Alias.ForEach(x => dtoAlias.Add(Converts.ToDtoAlias(x)));
            return dtoAlias;
        }

        public static List<Alias> ToAliases(List<dtoAlias> dtoAlias)
        {
            if (dtoAlias == null)
                return null;
            List<Alias> Alias = new List<Alias>();
            dtoAlias.ForEach(x => Alias.Add(Converts.ToAlias(x)));
            return Alias;
        }
        #endregion

        #region extrAlias
        public static Alias ToAlias(ExtraAlias extraAlias)
        {
            return new Alias() { Code = extraAlias.Code, IsPrivate = extraAlias.IsPrivate, Products = null, Shelves = null, Stands = null, TextAlias = extraAlias.TextAlias, Walls = null,  Parent = extraAlias.Parent, ProductAlias = new List<ProductAlia>() };
        }
        public static ExtraAlias ToExtraAlias(Alias Alias)
        {
            return new ExtraAlias() { Code = Alias.Code, TextAlias = Alias.TextAlias, IsPrivate = Alias.IsPrivate, products = new List<dtoProduct>(),  Parent = Convert.ToInt32(Alias.Parent) };
        }
        public static List<ExtraAlias> ToExtraAliases(List<Alias> Alias)
        {
            if (Alias == null)
                return null;
            List<ExtraAlias> extraAlias = new List<ExtraAlias>();
            Alias.ForEach(x => extraAlias.Add(Converts.ToExtraAlias(x)));
            return extraAlias;
        }



        public static List<Alias> ToAliases(List<ExtraAlias> extraAliases)
        {
            if (extraAliases == null)
                return null;
            List<Alias> Alias = new List<Alias>();
            extraAliases.ForEach(x => Alias.Add(Converts.ToAlias(x)));
            return Alias;
        }
        #endregion


        #region ProductAlias
        public static ProductAlia ToProductAlias(dtoProductAlias dtoProductAlias)
        {
            return new ProductAlia() { Code = dtoProductAlias.Code, Alias = Converts.ToAlias(dtoProductAlias.Alias), CodeAlias = dtoProductAlias.CodeAlias, CodeProduct = dtoProductAlias.CodeProduct };
        }
        public static dtoProductAlias ToDtoProductAlias(ProductAlia ProductAlias)
        {
            return new dtoProductAlias() { Code = ProductAlias.Code, Alias = Converts.ToDtoAlias(ProductAlias.Alias), CodeAlias = Convert.ToInt32(ProductAlias.CodeAlias), CodeProduct = Convert.ToInt32(ProductAlias.CodeProduct) };
        }
        public static List<dtoProductAlias> ToDtoProductAliases(List<ProductAlia> ProductAlias)
        {
            if (ProductAlias == null)
                return null;
            List<dtoProductAlias> dtoProductAlias = new List<dtoProductAlias>();
            ProductAlias.ForEach(x => dtoProductAlias.Add(Converts.ToDtoProductAlias(x)));
            return dtoProductAlias;
        }



        public static List<ProductAlia> ToProductAliases(List<dtoProductAlias> dtoProductAlias)
        {
            if (dtoProductAlias == null)
                return null;
            List<ProductAlia> ProductAlias = new List<ProductAlia>();
            dtoProductAlias.ForEach(x => ProductAlias.Add(Converts.ToProductAlias(x)));
            return ProductAlias;
        }
        #endregion

        #region Connection
        public static Connection ToConnection(dtoConnection dtoConnection)
        {
            return new Connection() { Code = dtoConnection.Code, CodeDest = dtoConnection.CodeDest, CodeSource = dtoConnection.CodeSource, Distance = dtoConnection.Distance, Getaway = null, Nearest = false, CodeShop = dtoConnection.CodeShop, Shop = null, TypeDest = Convert.ToInt32(dtoConnection.TypeDest) };
        }
        public static dtoConnection ToDtoConnection(Connection Connection)
        {
            return new dtoConnection() { Code = Connection.Code, CodeDest = Convert.ToInt32(Connection.CodeDest), CodeSource = Convert.ToInt32(Connection.CodeSource), Distance = Convert.ToInt32(Connection.Distance), Nearest = Connection.Nearest != null ? Convert.ToBoolean(Connection.Nearest) : false, CodeShop = Convert.ToInt32(Connection.CodeShop), TypeDest = (eTypeConnection)Convert.ToInt32(Connection.TypeDest) };
        }
        public static List<dtoConnection> ToDtoConnections(List<Connection> Connection)
        {
            if (Connection == null)
                return null;
            List<dtoConnection> dtoConnection = new List<dtoConnection>();
            Connection.ForEach(x => dtoConnection.Add(Converts.ToDtoConnection(x)));
            return dtoConnection;
        }

        public static List<Connection> ToConnections(List<dtoConnection> dtoConnection)
        {
            if (dtoConnection == null)
                return null;
            List<Connection> Connection = new List<Connection>();
            dtoConnection.ForEach(x => Connection.Add(Converts.ToConnection(x)));
            return Connection;
        }
        #endregion


        #region Getaway
        public static Getaway ToGetaway(dtoGetaway dtoGetaway)
        {
            return new Getaway() { Code = dtoGetaway.Code, CodeShop = dtoGetaway.CodeShop, Shop = null, X1 = dtoGetaway.P1.X, Y1 = dtoGetaway.P1.Y, X2 = dtoGetaway.P2.X, Y2 = dtoGetaway.P2.Y, Connections = null };
        }
        public static dtoGetaway ToDtoGetawayI(GetawayProcI_Result Getaway)
        {
            var v = new dtoGetaway() { Code = Getaway.Code, CodeShop = Convert.ToInt32(Getaway.CodeShop), P1 = new Point(Convert.ToInt32(Getaway.X1), Convert.ToInt32(Getaway.Y1)), P2 = new Point(Convert.ToInt32(Getaway.X2), Convert.ToInt32(Getaway.Y2)), I = Convert.ToInt32(Getaway.I) };
            return v;
        }
        public static List<dtoGetaway> ToDtoGetawaysI(List<GetawayProcI_Result> Getaways)
        {
            if (Getaways == null)
                return null;
            List<dtoGetaway> dtoGetaways = new List<dtoGetaway>();
            Getaways.ForEach(x => dtoGetaways.Add(Converts.ToDtoGetawayI(x)));
            return dtoGetaways;
        }
        public static List<Getaway> ToGetaways(List<dtoGetaway> dtoGetaways)
        {
            if (dtoGetaways == null)
                return null;
            List<Getaway> Getaways = new List<Getaway>();
            dtoGetaways.ForEach(x => Getaways.Add(Converts.ToGetaway(x)));
            return Getaways;
        }
        #endregion

        #region product
        public static Product ToProduct(dtoProduct dtoProduct)
        {
            if (dtoProduct == null) return null;
            return new Product() { Barcode = dtoProduct.Barcode, Company = dtoProduct.Company, CodeAlias = dtoProduct.CodeAlias, Size = dtoProduct.Size, Code = dtoProduct.Code, Alias = Converts.ToAlias(dtoProduct.Alias), ProductAlias = Converts.ToProductAliases(dtoProduct.ProductAlias), Src = dtoProduct.Src };
        }

        public static dtoProduct ToDtoProduct(Product product)
        {
            if (product == null) return null;
            try
            {
            return new dtoProduct() { Barcode = product.Barcode, Code = product.Code, Company = product.Company, Size = product.Size , Alias = Converts.ToDtoAlias(product.Alias), CodeAlias = Convert.ToInt32(product.CodeAlias), ProductAlias =  product.ProductAlias==null?null:Converts.ToDtoProductAliases(product.ProductAlias.ToList()), Src = product.Src };
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public static List<dtoProduct> ToDtoProducts(List<Product> products)
        {
            if (products == null)
                return null;
            List<dtoProduct> dtoProducts = new List<dtoProduct>();
            products.ForEach(x => dtoProducts.Add(Converts.ToDtoProduct(x)));
            return dtoProducts;
        }
        public static List<Product> ToProducts(List<dtoProduct> dtoProducts)
        {
            if (dtoProducts == null)
                return null;
            List<Product> products = new List<Product>();
            dtoProducts.ForEach(x => products.Add(Converts.ToProduct(x)));
            return products;
        }
        #endregion

        #region productShelf
        public static ProductShelf ToProductShelf(dtoProductShelf dtoProductShelf)
        {
            return new ProductShelf() { Code = dtoProductShelf.Code, CodeProduct = dtoProductShelf.CodeProduct, CodeShelf = dtoProductShelf.CodeShelf, Product = Converts.ToProduct(dtoProductShelf.Product), Shelf = null };
        }
        public static dtoProductShelf ToDtoProductShelf(ProductShelf productshelf)
        {
            return new dtoProductShelf() { Code = productshelf.Code, CodeProduct = productshelf.CodeProduct != null ? Convert.ToInt32(productshelf.CodeProduct) : 0, CodeShelf = productshelf.CodeShelf != null ? Convert.ToInt32(productshelf.CodeShelf) : 0, Product = Converts.ToDtoProduct(productshelf.Product) };
        }
        public static List<dtoProductShelf> ToDtoProductShelves(List<ProductShelf> products)
        {
            if (products == null)
                return null;
            List<dtoProductShelf> dtoproduct = new List<dtoProductShelf>();
            products.ForEach(x => dtoproduct.Add(Converts.ToDtoProductShelf(x)));
            return dtoproduct;
        }
        public static List<ProductShelf> ToProductShelves(List<dtoProductShelf> dtoProductsShelf)
        {
            if (dtoProductsShelf == null)
                return null;
            List<ProductShelf> products = new List<ProductShelf>();
            dtoProductsShelf.ForEach(x => products.Add(Converts.ToProductShelf(x)));
            return products;
        }

        #endregion

        #region productShop
        public static ProductShop ToProductShop(dtoProductShop dtoProductShop)
        {
            return new ProductShop() { Code = dtoProductShop.Code, CodeProduct = dtoProductShop.CodeProduct, CodeShop = dtoProductShop.CodeShop };
        }
        public static dtoProductShop ToDtoProductShop(ProductShop productShop)
        {
            return new dtoProductShop() { Code = Convert.ToInt32( productShop.Code), CodeProduct = Convert.ToInt32(productShop.CodeProduct), CodeShop = Convert.ToInt32(productShop.CodeShop),  Product = Converts.ToDtoProduct(productShop.Product) };
        }
        public static List<dtoProductShop> ToDtoProductShops(List<ProductShop> products)
        {
            if (products == null)
                return null;
            List<dtoProductShop> dtoproductShops = new List<dtoProductShop>();
            products.ForEach(x => dtoproductShops.Add(Converts.ToDtoProductShop(x)));
            return dtoproductShops;
        }
        public static List<ProductShop> ToProductshops(List<dtoProductShop> dtoProductsShops)
        {
            if (dtoProductsShops == null)
                return null;
            List<ProductShop> products = new List<ProductShop>();
            dtoProductsShops.ForEach(x => products.Add(Converts.ToProductShop(x)));
            return products;
        }
        #endregion

        #region shelf
        public static Shelf ToShelf(dtoShelf dtoShelf)
        {
            return new Shelf() { Code = dtoShelf.Code, Num = dtoShelf.Num, CodeStand = dtoShelf.CodeStand, ProductShelves = dtoShelf.ProductShelves==null?null: ToProductShelves(dtoShelf.ProductShelves.ToList()), Alias = Converts.ToAlias(dtoShelf.Alias), Stand = null, CodeAlias = dtoShelf.CodeAlias };
        }
        public static dtoShelf ToDtoShelf(Shelf shelf)
        {
            return new dtoShelf() { Code = shelf.Code, CodeStand = Convert.ToInt32(shelf.CodeStand), ProductShelves = shelf.ProductShelves == null ? null : ToDtoProductShelves(shelf.ProductShelves.ToList()), Alias = Converts.ToDtoAlias(shelf.Alias), Num = shelf.Num, CodeAlias = Convert.ToInt32(shelf.CodeAlias) };
        }
        public static List<dtoShelf> ToDtoShelves(List<Shelf> Shelf)
        {
            if (Shelf == null)
                return null;
            List<dtoShelf> dtoShelf = new List<dtoShelf>();
            Shelf.ForEach(x => dtoShelf.Add(Converts.ToDtoShelf(x)));
            return dtoShelf;
        }
        public static List<Shelf> ToShelves(List<dtoShelf> dtoShelf)
        {
            if (dtoShelf == null)
                return null;
            List<Shelf> Shelves = new List<Shelf>();
            dtoShelf.ForEach(x => Shelves.Add(Converts.ToShelf(x)));
            return Shelves;
        }
        #endregion

        #region shop
        public static dtoShop ToDtoShop(Shop shop)
        {// כל רשימה שממירים יש לבדוק האם אינה ריקה 
         //משום שהרשימות של אוייקט הדאל צריכות המרה לטיפוס רשימה, ולא ניתן להמיר אובייקט ריק
            dtoShop dto = new dtoShop();
            dto.Code = shop.Code;
            dto.NameShop = shop.NameShop;
            dto.Stands = shop.Stands != null ? Converts.ToDtoStands(shop.Stands.ToList()) : null;
            dto.Walls = shop.Walls != null ? Converts.ToDtoWalls(shop.Walls.ToList()) : null;
            dto.Connections = shop.Connections != null ? Converts.ToDtoConnections(shop.Connections.ToList()) : null;
           
            dto.Getaways = shop.Stands != null ? Converts.ToDtoGetawaysI(dtoShop.GetawayProcI_Results(shop.Code)) : null;
            dto.CodeGetaway = Convert.ToInt32(shop.CodeGetaway);
            return dto;
        }
        public static Shop ToShop(dtoShop dtoShop,string password)
        {
            return new Shop() { Code = dtoShop.Code, NameShop = dtoShop.NameShop, Password =password, Connections = Converts.ToConnections(dtoShop.Connections), Getaways = Converts.ToGetaways(dtoShop.Getaways), Walls = Converts.ToWalls(dtoShop.Walls), Stands = Converts.ToStands(dtoShop.Stands) ,CodeGetaway=dtoShop.CodeGetaway};
        }
        public static List<Shop> ToShops(List<dtoShop> dtoShops)
        {
            if (dtoShops == null)
                return null;
            return dtoShops.Select(x => Converts.ToShop(x,"")).ToList();
        }
        public static List<dtoShop> ToDtoShops(List<Shop> shops)
        {
            if (shops == null)
                return null;
            return shops.Select(x => Converts.ToDtoShop(x)).ToList();
        }
        #endregion

        #region stand
        public static Stand ToStand(dtoStand dtoStand)
        {
            return new Stand() { Code = dtoStand.Code, CodeShop = dtoStand.CodeShop, X2 = dtoStand.P2.X, X1 = dtoStand.P1.X, Y2 = dtoStand.P2.Y, Y1 = dtoStand.P1.Y, Alias = Converts.ToAlias(dtoStand.Alias), Shelves = Converts.ToShelves(dtoStand.Shelves), Shop = null, CodeAlias = dtoStand.CodeAlias };
        }
        public static dtoStand ToDtoStand(Stand stand)
        {
            return new dtoStand() { Code = stand.Code, CodeShop = stand.CodeShop != null ? Convert.ToInt32(stand.CodeShop) : 0, P2 = new Point() { X = Convert.ToInt32(stand.X2), Y = Convert.ToInt32(stand.Y2) }, P1 = new Point() { X = Convert.ToInt32(stand.X1), Y = Convert.ToInt32(stand.Y1) }, Shelves = stand.Shelves == null ? null : Converts.ToDtoShelves(stand.Shelves.ToList()), Alias = Converts.ToDtoAlias(stand.Alias),CodeAlias=stand.CodeAlias };
        }
        public static List<dtoStand> ToDtoStands(List<Stand> stands)
        {
            if (stands == null)
                return null;
            List<dtoStand> dtoStands = new List<dtoStand>();

            stands.ForEach(x => dtoStands.Add(Converts.ToDtoStand(x)));
            return dtoStands;
        }
        public static List<Stand> ToStands(List<dtoStand> dtoStands)
        {
            if (dtoStands == null)
                return null;
            List<Stand> stands = new List<Stand>();
            dtoStands.ForEach(x => stands.Add(Converts.ToStand(x)));
            return stands;
        }

        #endregion

        #region wall
        public static Wall ToWall(dtoWall dtoWall)
        {
            return new Wall() { Code = dtoWall.Code, CodeShop = dtoWall.CodeShop, X1 = dtoWall.P1.X, X2 = dtoWall.P2.X, Y1 = dtoWall.P1.Y, Y2 = dtoWall.P2.Y, Alias = Converts.ToAlias(dtoWall.Alias), Shop = null, CodeAlias = dtoWall.CodeAlias };
        }
        public static dtoWall ToDtoWall(Wall Wall)
        {
            return new dtoWall() { Code = Wall.Code, CodeShop = Convert.ToInt32(Wall.CodeShop), P1 = new Point(Convert.ToInt32(Wall.X1), Convert.ToInt32(Wall.Y1)), P2 = new Point(Convert.ToInt32(Wall.X2), Convert.ToInt32(Wall.Y2)), Alias = Converts.ToDtoAlias(Wall.Alias), CodeAlias = Wall.CodeAlias };
        }
        public static List<dtoWall> ToDtoWalls(List<Wall> Walls)
        {
            if (Walls == null)
                return null;
            List<dtoWall> dtoWalls = new List<dtoWall>();
            Walls.ForEach(x => dtoWalls.Add(Converts.ToDtoWall(x)));
            return dtoWalls;
        }
        public static List<Wall> ToWalls(List<dtoWall> dtoWalls)
        {
            if (dtoWalls == null)
                return null;
            List<Wall> Walls = new List<Wall>();
            dtoWalls.ForEach(x => Walls.Add(Converts.ToWall(x)));
            return Walls;
        }
        #endregion


        //מקבל גטוויס מסויימים [של הדאל, בזכות הרשימות המקושרות] וממיר אותם לדיטיאו כי צריך את האינדקס
        public static List<GetawayProcI_Result> ToGetawayProcResult(List<Getaway> getaways, List<GetawayProcI_Result> getawayProcI_Results)
        {
            return getawayProcI_Results.Where(x => getaways.Where(y => x.Code == y.Code).Count() > 0).ToList();
        }

        //מקבל גטווי [של הדאל, ] וממיר אותם  כי צריך את האינדקס
        public static GetawayProcI_Result ToGetawayProcResult(Getaway g, List<GetawayProcI_Result> getawayProcI_Results)
        {
            return getawayProcI_Results.Where(x => x.Code==g.Code).First();
        }
        public static ProductShelf ToProductShelf(dtoProductShop dtoProductShop)
        {
            try
            {
                string str = "";
            GeneralDB db = new GeneralDB();
                //זה חייב להיות כזה מסובך?
                //var q5 = db.MyDb.ProductShelves.Where(x => x.CodeProduct == dtoProductShop.CodeProduct).ToList();
                //var q6 = db.MyDb.ProductShops.Where(x => x.CodeProduct == dtoProductShop.CodeProduct && x.CodeShop == dtoProductShop.CodeShop).ToList();

                var q = db.MyDb.Shops.First(shop => shop.Code == dtoProductShop.CodeShop);
                var q1=q.Stands.ToList();
                var q2 = q1.ToList().Select(stand => stand.Shelves).ToList();
                var q3 = q2.Select(x => x.Select(y => y.ProductShelves).ToList()).ToList();
                var q4 = q3.Select(a => a.Select(b => b.Where(c => c.CodeProduct == dtoProductShop.CodeProduct).ToList()).ToList()).ToList();
                ProductShelf ps = null ;
                q4.ForEach(a => a.ForEach(b => b.ForEach(c => { str += "codeProduct: " + c.CodeProduct + " codeShelf: " + c.CodeShelf + "\n"; ps = c; })));
              //  MessageBox.Show(str);
                //.Select(shelf => shelf.ProductShelves.Where(productShelf => productShelf.CodeProduct == dtoProductShop.CodeProduct)));         
            return ps;
            }
            catch
            {
                MessageBox.Show("לא מצליח להמיר ממוצר חנות למוצר מדף");
                return null;
            }

        }
        public static Cell[][] ToMatrix(Cell[,] distanceMatrix)
        {
            int length = distanceMatrix.GetLength(0);
            Cell[][] matrix = new Cell[length][];
            for (int i = 0; i < length; i++)
            {
                matrix[i] = new Cell[length];
                for (int j = 0; j < length; j++)
                {
                    matrix[i][j] = distanceMatrix[i, j];
                }
            }
            return matrix;
        }
        public static Cell[,] ToMat(Matrix m1)
        {
            int length = m1.Mat.GetLength(0);
            Cell[,] distanceMatrix = new Cell[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                   distanceMatrix[i, j]  = m1.Mat[i][j];
                }
            }
            return distanceMatrix;
        }

    }


}
