using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
     partial class GetawayProcI_Result
    {
        public override bool Equals(object obj)
        {
            return Code == ((GetawayProcI_Result)obj).Code;
        }

        public static int GetI(List<GetawayProcI_Result> points, int code)
        {
            var v = points.Where(x => x.Code == code).FirstOrDefault();
            if (v == null) return code;
            else return Convert.ToInt32( v.I);

        }
    }
}
