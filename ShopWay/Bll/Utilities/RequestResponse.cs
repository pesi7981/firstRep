using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bll.Utilities
{
    public class RequestResponse
    {
        public string Message { get; set; }
        public bool Result { get; set; }
        public object Data { get; set; }
    }
}