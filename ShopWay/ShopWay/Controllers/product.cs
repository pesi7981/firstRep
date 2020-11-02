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
    [System.Web.Http.RoutePrefix("api/product")]
    public class product : ApiController
    {

        //GET: list of product to select one
        [System.Web.Http.HttpGet]
        [Route("GetProductList")]
        public List<dtoProduct> GetProductList()
        {
            List<dtoProduct> l = dtoProduct.GetDtoProduct();
            return l;
        }

        // POST: api/product
        //POST: api/product/Post
        [System.Web.Http.HttpPost]
        //[Route("Post")]
        public void Post(dtoProductShop product)
        {
            //זה מה שצריך לעבוד בסוף
            product.Insert();
        }

        // PUT: api/customer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/customer/5
        public void Delete(int id)
        {
        }
    }
}
