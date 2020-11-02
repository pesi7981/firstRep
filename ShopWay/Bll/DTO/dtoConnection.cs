using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Bll.Logic;
using Bll.Logic;
using Bll.Utilities;

namespace Bll.DTO
{
   public class dtoConnection
    {
        public int Code { get; set; }
        public int CodeSource { get; set; }
        public int CodeDest { get; set; }
        public double Distance { get; set; }
        public eTypeConnection TypeDest { get; set; }
        public bool Nearest { get; set; }
        public int CodeShop { get; set; }

        public static RequestResponse InsertList(List<dtoConnection> connections)
        {
            GeneralDB dB = new GeneralDB();
            RequestResponse r = new RequestResponse();
                r.Message = "shop number "+connections[0].CodeShop+ "inserted "+connections.Count();
            try
            {
                int c = connections[0].CodeShop;
                r.Result = true;
            connections.ForEach(x => dB.MyDb.Connections.Add(Converts.ToConnection( x)));
                dB.MyDb.SaveChanges();
                //Shop s = dB.MyDb.Shops.Where(x => x.Code == connections[0].CodeShop).First();
                Shop s=new Shop();
                List<Shop> shops = dB.MyDb.Shops.ToList();

                for (int i = 0; i < shops.Count(); i++)
                {
                    if (shops[i].Code == c) {
                        s = shops[i];
                        break; }
                }
                r.Data = Converts.ToDtoShop(s);
            }
            catch
            {
                r.Result = false;
                r.Message = "can't add connections";
            }
            return r;
        }
    }
}
