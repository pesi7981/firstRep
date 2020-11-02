using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bll.DTO;

namespace ShopWay.Controllers
{
    [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
    [System.Web.Http.RoutePrefix("api/gateway")]
    public class gateway : ApiController
    {
        // GET: api/gateway
        //GET: list of getwys to select one
        [System.Web.Http.HttpGet]
        [Route("GetGatewyList")]
        public List<dtoGetaway> GetGatewyList()
        {
            List<dtoGetaway> l = dtoGetaway.GetDtoGetaways();
            return l;
            dtoStand s = new dtoStand();
            //שרשרת הגישה
            var v = s.DtoShelves.First().ProductShelves.First().DtoProduct;
        }

        //POST: api/gateway/Post
        [System.Web.Http.HttpPost]
        //[Route("Post")]
        public void Post(dtoGetaway g)
        {
            //זה מה שצריך לעבוד בסוף
            g.Insert();
        }

        // PUT: api/gateway/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/gateway/5
        public void Delete(int id)
        {
        }
    }
}
