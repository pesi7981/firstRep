using Bll.DTO;
using Bll.Logic;
using Bll.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows.Forms;

namespace ShopWay.Controllers
{
    
    [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
    //[System.Web.Http.RoutePrefix("api/Way")]
    public class WayController : ApiController
    {
        //GET: map shop
        [System.Web.Http.HttpGet]
        [Route("api/Way/GetMap")]
        public RequestResponse GetMap(int code=1)
        {
            RequestResponse r = new RequestResponse();
            //TODO: לקבל קוד חנות ולשלוח לפנוקציה
            int[][] matShop= dtoShop.GetMap(code);
            r.Data = matShop;
            r.Message = "this is your map";
            r.Result = true;
            return r;
        }

        // GET: api/Way/5
        public string Get(int id)
        {
            return "value";
        }

        ///Post: Way
        [System.Web.Http.HttpPost]
        [Route("api/Way/PostWay")]
        public RequestResponse PostWay(RequestResponse r)
        {
            RequestResponse r2 = new RequestResponse();
            try
            {
                dynamic data = r.Data;
                List<object> objectsData = data.ToObject<List<object>>();
                dynamic d0 = objectsData[0];
                dynamic d1 = objectsData[1];
                dynamic d2 = objectsData[2];
                dynamic d3 = objectsData[3];
                int[] CodeProducts = (int[])d0.ToObject<int[]>();
                bool endCash = (Boolean)d3;
                int numShop = Convert.ToInt32(d2);
                object d4 = d1;
                //TODO: לטפל שישתמש בנקודת התחלה

                Point pStart = d1.ToObject<Point>();
                if (pStart.X == 0 && pStart.Y == 0)
                {  //MessageBox.Show("no p");
                    pStart = null; }
                List<Goal> answers = TravelComputer.MainComputeTravel(CodeProducts, pStart, numShop, endCash,ShopWay.Properties.Resources.path);
                r2.Data = answers;
                r2.Result = true;
                r2.Message = "this is your way";
            }
            catch
            {
                r2.Data = null;
                r2.Message = "problem in compute travel";
                r2.Result = false;
            }
            
            return r2;
        }

        // PUT: api/Way/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Way/5
        public void Delete(int id)
        {
        }
    }
}
