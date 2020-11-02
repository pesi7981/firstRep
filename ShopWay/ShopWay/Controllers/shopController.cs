using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using Bll.DTO;
using Bll.Utilities;


namespace ShopWay.Controllers
{


    [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
    //[System.Web.Http.RoutePrefix("api/shop")]
    public class shopController : ApiController
    {
    string path = ShopWay.Properties.Resources.path;
        //GET: list of shops to select one
        [System.Web.Http.HttpGet]
        [Route("api/shop/GetShopList")]
        public List<dtoShop> GetShopList()
        {
            List<dtoShop> l = dtoShop.GetDtoShops();
            return l;
        }
        //Post: shop
        [System.Web.Http.HttpPost]
        [Route("api/shop/PostAllow")]
        public RequestResponse PostAllow(RequestResponse r)
        {
            dynamic v = r.Data;
            dynamic d = v.ToObject<List<string>>();
            List<string> l = (List<string>)d;
            bool b = dtoShop.comparePassword(l[0], Convert.ToInt32(l[1]));
            RequestResponse response = new RequestResponse();
            response.Result = b;
            if (!b)
                response.Message = "סיסמא לא תקינה";
            else
                response.Message = "לא יאומן, אבל הסיסמא תקינה!!!";
            return response;

        }


        //Post: shop


        //POST: InsertShop1
        [System.Web.Http.HttpPost]
        [Route("api/shop/InsertShop1")]
        public RequestResponse InsertShop1(dtoShop s)
        {
            //הכנסת חנות ע"י בניה ללא קשתות
            RequestResponse r = s.Insert("1234",path);
            return r;
        }
        //POST: InsertShop2
        [System.Web.Http.HttpPost]
        [Route("api/shop/InsertShop2")]
        public RequestResponse InsertShop2(RequestResponse request)
        {
            dynamic data = request.Data;
            List<dtoConnection> connections = data.ToObject<List<dtoConnection>>();
            //הוספת קשתות לחנות
            RequestResponse r = dtoConnection.InsertList(connections);
            return r;
        }

/*
        //POST: PostXml
        [System.Web.Http.HttpPost]
        [Route("api/shop/PostXml")]
        public RequestResponse PostXml(RequestResponse request)
        {

            //הוספת XML של החנות
            //string xml= System.Text.Encoding.Default.GetString(request);
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(xml);
            //doc["shop"][]


            RequestResponse r = new RequestResponse();
            try
            {
 //TODO: מה עושים איתו
            r.Data = null;
            r.Message = "thank you for the file";
            r.Result = true;

            dtoShop s3 = new dtoShop();
            XmlSerializer xml2 = new XmlSerializer(typeof(dtoShop));
            string str = (string)request.Data;

            byte[] arr = Encoding.ASCII.GetBytes(str);
            Stream stream = new MemoryStream(arr);
            s3 = (dtoShop)xml2.Deserialize(stream);
            s3.Insert("");


            }
            catch(Exception e)
            {
                r.Message = "there is problems with the file";
            }
           
            return r;
        }
        */



        //POST: PostXml
        [System.Web.Http.HttpPost]
        [Route("api/shop/PostXml")]
        public RequestResponse PostXml(RequestResponse request)
        {

            //הוספת XML של החנות
            //string xml= System.Text.Encoding.Default.GetString(request);
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(xml);
            //doc["shop"][]


            RequestResponse r = new RequestResponse();
            try
            {
                //TODO: מה עושים איתו
                r.Data = null;
                r.Message = "thank you for the file";
                r.Result = true;

                dtoShop s3 = new dtoShop();
                XmlSerializer xml2 = new XmlSerializer(typeof(dtoShop));
                string str = (string)request.Data;
                List<byte> arr = new List<byte>();
                str.Split(',').ToList().ForEach(x => arr.Add(byte.Parse(x)));
                Encoding.ASCII.GetBytes(str);
                var str1 = System.Text.Encoding.Default.GetString(arr.ToArray());

                Stream stream = new MemoryStream(arr.ToArray());
                s3 = (dtoShop)xml2.Deserialize(stream);
               r= s3.Insert("",path);
                r.Result = true;
                r.Message = "החנות נוספה בהצלחה. ביכולתך לשבץ בה מוצרים";

            }
            catch (Exception e)
            {
                r.Result = false;
                r.Message = "קיימת בעיה בקובץ שהזנת, בדוק שהוא בתבנית הנדרשת";
            }

            return r;
        }









                //POST: stands of shop
                [System.Web.Http.HttpPost]
        [Route("api/shop/GetStands")]
        public RequestResponse GetStands(int codeShop)
        {
            //  int codeShop = Convert.ToInt32(request.Data.ToString());
            RequestResponse response = new RequestResponse();
            List<dtoStand> l = dtoShop.GetStands(codeShop);
            response.Data = l;
            return response;
        }


        //POST: api/shop/PostShopBasic
        [System.Web.Http.HttpPost]
        //[ActionName("Post")]
        [Route("api/shop/PostShopBasic")]
        public RequestResponse PostShopBasic(RequestResponse request)
        {
            dynamic data = request.Data;
            List<object> objectsData = data.ToObject<List<object>>();
            dynamic d0 = objectsData[0];
            dynamic d1 = objectsData[1];
           dtoShop shop=(dtoShop)d0.ToObject<dtoShop>();
            string password = (string)d1;
 
            //זה מה שצריך לעבוד בסוף
            return shop.Insert(password,path);
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
