using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
  partial  class Getaway
    {
        public override bool Equals(object obj)
        {
            return Code == ((Getaway)obj).Code;
        }
    }
}
