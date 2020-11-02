using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bll.DTO;
using Bll.Utilities;


namespace ShopWay.Controllers
{
    [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
    [System.Web.Http.RoutePrefix("api/shop")]
    public class shop : ApiController
    {

        //GET: list of shops to select one
        [System.Web.Http.HttpGet]
        [Route("GetShopList")]
        public List<dtoShop> GetShopList()
        {
            List<dtoShop> l = dtoShop.GetDtoShops();
            return l;
        }

        //POST: api/shop/Post
        [System.Web.Http.HttpPost]
        //[Route("Post")]
        public void Post(dtoShop s)
        {
            //זה מה שצריך לעבוד בסוף
            s.Insert();
        }




        //// PUT: api/shop/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ownerManagment/5
        //public void Delete(int id)
        //{
        //}
    }
}
