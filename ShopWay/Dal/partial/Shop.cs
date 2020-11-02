using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public partial class Shop
    {
        public List<ProductShop> ProductShops { get
            {
                ShopWayEntities entities = new ShopWayEntities();
                return entities.ProductShops.Where(x => x.CodeShop == this.Code).ToList();
            }
           }

        public List<ProductShop> GetProductShop()
        {
            return ProductShops;
        }
    }
}
