using System;
using System.Collections.Generic;
using Dal;

namespace Bll.DB
{
  public  class GeneralDB
    {
        private ShopWayEntities PrivateDb;
        public ShopWayEntities MyDb
        {
            get
            {
                if (PrivateDb == null)
                    PrivateDb = new ShopWayEntities();
                return PrivateDb;
            }
        }
        public bool Save()
        {
            try
            {
                this.MyDb.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
