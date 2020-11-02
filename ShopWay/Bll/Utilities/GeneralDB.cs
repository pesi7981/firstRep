using System;
using System.Collections.Generic;
using Dal;
using Bll.Utilities;
using System.Linq;
using System.Windows.Forms;

namespace Bll.Utilities
{
    public class GeneralDB
    {
        private ShopWayEntities privateDb;
        public ShopWayEntities MyDb
        {
            get
            {
                if (privateDb == null)
                    privateDb = new ShopWayEntities();
                return privateDb;
            }

        }
    }
}

        //void func()
        //{
        //    ShopWayEntities s = new ShopWayEntities();
        //    var v = s.Products.ToList();
        //    BaseRepository<Product> l = new BaseRepository<Product>();
        //}

//        //עבור כל טבלה 
//        //Shop
//        private BaseRepository<Shop> privateShops=null;
//        public BaseRepository<Shop> Shops
//        {
//            get
//            {
//                if (privateShops == null)
//                    privateShops = new BaseRepository<Shop>();
//               var q = MyDb.Shops;
//                foreach (var item in q)
//                {
//                    privateShops.Add(item);
//                }
//                return privateShops;
//            }
//        }


//        //product
//        private BaseRepository<Product> privateProducts = null;
//        public BaseRepository<Product> Products
//        {
//            get
//            {
//                if (privateProducts == null)
//                    privateProducts = new BaseRepository<Product>();
//                ShopWayEntities s = new ShopWayEntities();

//                foreach (var item in MyDb.Products)
//                {
//                    privateProducts.Add(item);
//                }
//                return privateProducts;
//            }
//        }
//        //productShop
//        private BaseRepository<ProductShop> privateProductsShop = null;
//        public BaseRepository<ProductShop> ProductShop
//        {
//            get
//            {
//                if (privateProductsShop == null)
//                    privateProductsShop = new BaseRepository<ProductShop>();
//                ShopWayEntities s = new ShopWayEntities();

//                foreach (var item in MyDb.ProductShops)
//                {
//                    privateProductsShop.Add(item);
//                }
//                return privateProductsShop;
//            }
//        }
//        //productShelf
//        private BaseRepository<ProductShelf> privateProductsShelf = null;
//        public BaseRepository<ProductShelf> ProductShelf
//        {
//            get
//            {
//                if (privateProductsShop == null)
//                    privateProductsShelf = new BaseRepository<ProductShelf>();
//                ShopWayEntities s = new ShopWayEntities();

//                foreach (var item in MyDb.ProductShelves)
//                {
//                    privateProductsShelf.Add(item);
//                }
//                return privateProductsShelf;
//            }
//        }

//        //Self
//        private BaseRepository<Shelf> privateShelf = null;
//        public BaseRepository<Shelf> Shelfhop
//        {
//            get
//            {
//                if (privateShelf == null)
//                    privateShelf = new BaseRepository<Shelf>();
//                ShopWayEntities s = new ShopWayEntities();

//                foreach (var item in MyDb.Shelves)
//                {
//                    privateShelf.Add(item);
//                }
//                return privateShelf;
//            }
//        }
//        //getaway
//        private BaseRepository<Getaway> privateGetaway = null;
//        public BaseRepository<Getaway> ProductGetaway
//        {
//            get
//            {
//                if (privateGetaway == null)
//                    privateGetaway = new BaseRepository<Getaway>();
//                ShopWayEntities s = new ShopWayEntities();

//                foreach (var item in MyDb.Getaways)
//                {
//                    privateGetaway.Add(item);
//                }
//                return privateGetaway;
//            }
//        }
//        //wall
//        private BaseRepository<Wall> privatewall = null;
//        public BaseRepository<Wall> Wall
//        {
//            get
//            {
//                if (privatewall == null)
//                    privatewall = new BaseRepository<Wall>();
//                ShopWayEntities s = new ShopWayEntities();

//                foreach (var item in MyDb.Walls)
//                {
//                    privatewall.Add(item);
//                }
//                return privatewall;
//            }
//        }
//        //Stand
//        private BaseRepository<Stand> privateStand = null;
//        public BaseRepository<Stand> Stand
//        {
//            get
//            {
//                if (privateProductsShop == null)
//                    privateStand = new BaseRepository<Stand>();
//                ShopWayEntities s = new ShopWayEntities();

//                foreach (var item in MyDb.Stands)
//                {
//                    privateStand.Add(item);
//                }
//                return privateStand;
//            }
//        }
//        //Connection
//        private BaseRepository<Connection> privateConnectionStand = null;
//        public BaseRepository<Connection> ConnectionStand
//        {
//            get
//            {
//                if (privateConnectionStand == null)
//                    privateConnectionStand = new BaseRepository<Connection>();
//                ShopWayEntities s = new ShopWayEntities();

//                foreach (var item in MyDb.Connections)
//                {
//                    privateConnectionStand.Add(item);
//                }
//                return privateConnectionStand;
//            }
//        }
     


//        public bool Save()
//        {
//            try
//            {
//                MessageBox.Show(privateDb.Shops.ToList().Last().NameShop+" code: "+privateDb.Shops.ToList().Last().Code);
//                this.privateDb.SaveChanges();
//                return true;
//            }

//            catch (Exception ex)
//            {
//                return false;
//            }
//        }
//    }
//}
