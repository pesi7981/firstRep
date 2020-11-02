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
    [System.Web.Http.RoutePrefix("api/ownerManagment")]
    public class ownerManagmentController : ApiController
    {

        //GET: list of shops to select one
        [System.Web.Http.HttpGet]
        [Route("GetShopsList")]
        public List<dtoShop> GetShopsList()
        {
            List<dtoShop> l = dtoShop.GetDtoShops();
            return l;
        }

        //POST: api/ownerManagment/Post
        [System.Web.Http.HttpPost]
        //[Route("Post")]
        public void Post(dtoShop s)
        {
            //זה מה שצריך לעבוד בסוף
            s.Insert();
        }




        //// PUT: api/ownerManagment/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ownerManagment/5
        //public void Delete(int id)
        //{
        //}
    }
}
