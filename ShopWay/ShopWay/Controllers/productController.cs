using System;
using System.Collections.Generic;
using System.Web.Http;
using Bll.DTO;
using Bll.Logic;
using System.Net;
using System.Net.Http;
using Bll.Utilities;
using System.Windows.Forms;

namespace ShopWay.Controllers
{

    [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
    public class productController : ApiController
    {
        //GET: list of product to select one
        [System.Web.Http.HttpGet]
        [Route("api/product/GetProductList")]
        public List<dtoProduct> GetProductList()
        {
            try
            {
                List<dtoProduct> l = dtoProduct.GetDtoProduct();
                return l;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //public List<ExtraAlias> GetExtraAliasList(int codeShop = 1)


        ///Get: extra alias
        [System.Web.Http.HttpGet]
        [Route("api/product/GetExtraAliasList")]
        public List<ExtraAlias> GetExtraAliasList(int code)
        {//TODO: לקבל מוצרים לחנות
            try
            {
                List<ExtraAlias> l = ExtraAlias.GetAliasesShop(code);
                return l;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        //GET: list of product to select one
        [System.Web.Http.HttpGet]
        [Route("api/product/GetAliasList")]
        public List<dtoAlias> GetAliasList()
        {
            try
            {
                List<dtoAlias> l = dtoAlias.GetAliases();
                return l;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        //GET: list of product to select one
        [System.Web.Http.HttpGet]
        [Route("api/product/GetDepartmentList")]
        public List<ExtraAlias> GetDepartmentList()
        {
            try
            {
                List<ExtraAlias> l = ExtraAlias.GetDepartment();
                return l;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        //GET: list of product shop
        [System.Web.Http.HttpGet]
        [Route("api/product/GetProdcutsShop")]
        public List<dtoProductShop> GetProdcutsShop(int code)
        {
            try
            {
                List<dtoProductShop> l = dtoProductShop.GetProdcutsShop(code);
                return l;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        [System.Web.Http.HttpPost]
        //[Route("api/product")]
        public RequestResponse Post(dtoProduct product)
        {
            RequestResponse r = new RequestResponse();
            try
            {
                dtoProduct dtoProduct= product.Insert();
                r.Data = dtoProduct;
                r.Message = "save product ok";
                r.Result = true;
            }
            catch
            {
                r.Message = "problem saving product";
                r.Result = false;
                r.Data = null;
            }
            //זה מה שצריך לעבוד בסוף
           
           
            return r;
        }

        //Post:PostAlias
        [System.Web.Http.HttpPost]
        [Route("api/product/PostAlias")]
        public dtoAlias PostAlias(dtoAlias alias)
        {
            //זה מה שצריך לעבוד בסוף
            return alias.Insert();
        }


        ///Post: Way
        [System.Web.Http.HttpPost]
        [Route("api/product/saveProductsShop")]
        public void saveProductsShop(RequestResponse request)
        {
            try
            {
                dynamic data = request.Data;
                dynamic d = data.ToObject<List<object>>();
                List<object> objectsData = (List<object>)d;
                dynamic d0 = objectsData[0];
                dynamic d1 = objectsData[1];
                dynamic d2 = objectsData[2];
                dynamic d3 = objectsData[3];
                dynamic d4 = objectsData[4];
                dynamic d5 = objectsData[5];
                dynamic d00 = d0.ToObject<List<dtoProductShelf>>();
                dynamic d11 = d1.ToObject<List<dtoProductAlias>>();
                dynamic d22 = d2.ToObject<List<dtoProduct>>();
                dynamic d44=d4.ToObject<List<dtoProductShelf>>();
                dynamic d55=d5.ToObject<List<dtoProductShop>>();
                List<dtoProductShelf> productShelves = (List<dtoProductShelf>)d00;
                List<dtoProductAlias> ProductAliases = (List<dtoProductAlias>)d11;
                List<dtoProduct> products = (List<dtoProduct>)d22;
                List<dtoProductShelf> UpdateProductShelves = (List<dtoProductShelf>)d44;
                List<dtoProductShop> DeleteProductShops = (List<dtoProductShop>)d55;
                int CodeShop = Convert.ToInt32(d3);

                dtoProductShelf.Insert(productShelves);
                dtoProductAlias.Insert(ProductAliases);
                dtoProduct.Update(products);
                dtoProductShelf.Update(UpdateProductShelves,CodeShop);
                dtoProductShop.Delete(DeleteProductShops);
            }
            catch(Exception e)
            {
                MessageBox.Show("problem casting");
            }
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
