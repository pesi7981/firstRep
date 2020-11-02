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
    public class CustomManagmentController : ApiController
    {
        // GET: api/CustomManagment
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CustomManagment/5
        public string Get(int id)
        {
            return "value";
        }
        //GET: list of shops to select one
        [HttpGet]
        [Route("api/GetShopsList")]
        public List<dtoShop> GetShopsList()
        {
        return    dtoShop.GetDtoShops();
        }




        // POST: api/CustomManagment
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/CustomManagment/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CustomManagment/5
        public void Delete(int id)
        {
        }
    }
}
