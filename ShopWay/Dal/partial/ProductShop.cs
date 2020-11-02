using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public partial class ProductShop
    {
        public Product Product
        {
            get
            {
                ShopWayEntities entities = new ShopWayEntities();
                return entities.Products.ToList().Where(x => x.Code == this.CodeProduct).FirstOrDefault();
            }
        }
    }
}
