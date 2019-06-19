using Newtonsoft.Json;
using SAP.Middleware.Connector;
using SapRFCService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SapRFCService.Models
{
    public class Vendors
    {
        public static string GetAllVendor(string P_DATE_from, string P_DATE_TO)
        {
            string result = "";

            try
            {
                IRfcTable IRetTable = null;
                List<Z_MM_QUBE_LIFNR> Z_MM_QUBE_LIFNR_List = new List<Z_MM_QUBE_LIFNR>();
                RfcConfigParameters parms = RFC.GetSettingParms();
                RfcDestination rfcDest = RfcDestinationManager.GetDestination(parms);
                RfcRepository rfcRep = rfcDest.Repository;
                IRfcFunction IReader = rfcRep.CreateFunction("Z_MM_QUBE_LIFNR");

                IReader.SetValue("P_DATE_from", P_DATE_from);
                IReader.SetValue("P_DATE_TO", P_DATE_TO);

                IReader.Invoke(rfcDest);
                IRetTable = IReader.GetTable("ET_LFA1");
                Z_MM_QUBE_LIFNR_List = IRetTable.AsQueryable().Select(x => new Z_MM_QUBE_LIFNR
                {
                    LIFNR = x.GetString("LIFNR") ?? "",
                    NAME1 = x.GetString("NAME1") ?? "",
                    ZZAUTH = x.GetString("ZZAUTH") ?? "",
                    LAND1 = x.GetString("LAND1") ?? "",
                    SORTL = x.GetString("SORTL") ?? "",
                    ERDAT = x.GetString("ERDAT") ?? "",
                    UDATE = x.GetString("UDATE") ?? ""
                }
                 ).ToList();

                using (var db = new CubeRFCEntities())
                {
                    var VendorsList = db.Vendor.AsEnumerable().Select(x => x.BatchNo).ToList();
                    int BatchNo = VendorsList.Count==0?1:VendorsList.Max() + 1;

                    foreach (var item in Z_MM_QUBE_LIFNR_List)
                    {                      
                        db.Vendor.Add(new Vendor()
                        {
                            LIFNR = item.LIFNR,
                            NAME1 = item.NAME1,
                            ZZAUTH = item.ZZAUTH,
                            LAND1 = item.LAND1,
                            SORTL = item.SORTL,
                            ERDAT = DateTime.Parse(item.ERDAT),
                            UDATE = item.UDATE == "0000-00-00" ? DateTime.Parse("1900-01-01") : DateTime.Parse(item.UDATE),
                            CreateDate = DateTime.Now,
                            BatchNo=BatchNo
                        });
                    }

                    db.SaveChanges();
                }
                result = JsonConvert.SerializeObject(Z_MM_QUBE_LIFNR_List);
                
            }
            catch (Exception e)
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

    public class Z_MM_QUBE_LIFNR
    {
        public string LIFNR = "";
        public string NAME1 = "";
        public string ZZAUTH = "";
        public string LAND1 = "";
        public string SORTL = "";
        public string ERDAT = "";
        public string UDATE = "";
    }
}