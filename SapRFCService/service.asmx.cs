using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SapRFCService.Models;

namespace SapRFCService
{
    /// <summary>
    ///service 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class service : System.Web.Services.WebService
    {
        
        [WebMethod]
        public string GetAllVendors(string P_DATE_from,string P_DATE_TO)
        {     
            return Vendors.GetAllVendor(P_DATE_from, P_DATE_TO);
        }

        [WebMethod]
        public string GetAllWarehouses()
        {
            return Warehouses.GetAllWarehouse();
        }

        [WebMethod]
        public string GetAllProducts (string P_DATE_from, string P_DATE_TO)
        {
            return Products.GetAllProducts(P_DATE_from,P_DATE_TO);
        }

        [WebMethod]
        public string GetAllProductsByID(string ProductsList)
        {
            return Products.GetAllProducts(ProductsList);
        }

        [WebMethod]
        public string GetAllStock(string P_DATE)
        {
            return Stocks.GetAllStock(P_DATE);
        }

        [WebMethod]
        public string GetAllPrice(string P_DATE)
        {
            return Prices.GetAllPrice(P_DATE);
        }
    }
}
