using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dal
{
    public partial class Stand:IComparable
    {
        public double Distance { get; set; }
        public int CompareTo(object obj)
        {
            return Distance.CompareTo((obj as Stand).Distance);
        }
        public override bool Equals(object obj)
        {
            var s = (Stand)obj;
            return this.Code == s.Code;
        }


        public List<Connection> GetConnections(Shop shop)
        {
            List<Connection> c = shop.Connections.ToList();
            c = c.Where(x => x.TypeDest == 2 && this.Code == x.CodeDest).ToList();
            return c;
        }
    }
}
