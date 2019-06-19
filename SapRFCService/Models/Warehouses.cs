using Newtonsoft.Json;
using SAP.Middleware.Connector;
using SapRFCService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapRFCService.Models
{
    public class Warehouses
    {
        public static string GetAllWarehouse()
        {
            string result = "";

            try
            {
                IRfcTable IRetTable = null;
                List<Z_MM_QUBE_WERKS> Z_MM_QUBE_WERKS_List = new List<Z_MM_QUBE_WERKS>();
                RfcConfigParameters parms = RFC.GetSettingParms();
                RfcDestination rfcDest = RfcDestinationManager.GetDestination(parms);
                RfcRepository rfcRep = rfcDest.Repository;
                IRfcFunction IReader = rfcRep.CreateFunction("Z_MM_QUBE_WERKS");

                IReader.Invoke(rfcDest);
                IRetTable = IReader.GetTable("ET_WERKS");
                Z_MM_QUBE_WERKS_List = IRetTable.AsQueryable().Select(x => new Z_MM_QUBE_WERKS
                {
                    WERKS = x.GetString("WERKS") ?? "",
                    NAME1 = x.GetString("NAME1") ?? "",
                    EKORG = x.GetString("EKORG") ?? ""
                }
                 ).ToList();

                using (var db = new CubeRFCEntities())
                {
                    var WarehouseList = db.Warehouse.AsEnumerable().Select(x => x.BatchNo).ToList();
                    int BatchNo = WarehouseList.Count==0?1:WarehouseList.Max() + 1;

                    foreach (var item in Z_MM_QUBE_WERKS_List)
                    {
                        db.Warehouse.Add(new Warehouse()
                        {
                            WERKS = item.WERKS,
                            NAME1 = item.NAME1,
                            EKORG = item.EKORG,
                            CreateDate = DateTime.Now,
                            BatchNo=BatchNo
                        });
                    }

                    db.SaveChanges();
                }

                result = JsonConvert.SerializeObject(Z_MM_QUBE_WERKS_List);
             
            } catch(Exception e)
            {
                result = e.ToString();
                //發送通知信給開發者
                string strMailTitle = "系統發生錯誤";
                string str_mailbody = e.ToString();
                Mail.Send(strMailTitle, result);
            }

            return result;
        }

    }

    public class Z_MM_QUBE_WERKS
    {
        public string WERKS = "";
        public string NAME1 = "";
        public string EKORG = "";

    }
}