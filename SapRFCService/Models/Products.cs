using Newtonsoft.Json;
using SAP.Middleware.Connector;
using SapRFCService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SapRFCService.Models
{
    public class Products
    {
        public static string GetAllProducts(string P_DATE_from, string P_DATE_TO)
        {
            string result = "";

            try {

                IRfcTable IRetTable = null;
                List<Z_MM_QUBE_MATNR> Z_MM_QUBE_MATNR_List = new List<Z_MM_QUBE_MATNR>();
                RfcConfigParameters parms = RFC.GetSettingParms();
                RfcDestination rfcDest = RfcDestinationManager.GetDestination(parms);
                RfcRepository rfcRep = rfcDest.Repository;
                IRfcFunction IReader = rfcRep.CreateFunction("Z_MM_QUBE_MATNR");
                IReader.SetValue("P_DATE_from", P_DATE_from);
                IReader.SetValue("P_DATE_TO", P_DATE_TO);
                IReader.Invoke(rfcDest);
                IRetTable = IReader.GetTable("ET_MATNR");
                Z_MM_QUBE_MATNR_List = IRetTable.AsQueryable().Select(x => new Z_MM_QUBE_MATNR
                {
                    MATNR = x.GetString("MATNR") ?? "",
                    MAKTX = x.GetString("MAKTX") ?? "",
                    ZZCT2 = x.GetString("ZZCT2") ?? "",
                    LIFNR = x.GetString("LIFNR") ?? "",
                    ZZMATU = x.GetString("ZZMATU") ?? "",
                    ZFVOLUME = x.GetString("ZFVOLUME") ?? "",
                    ZZPAGE = x.GetString("ZZPAGE") ?? "",
                    WGBEZ = x.GetString("WGBEZ") ?? "",
                    ZFPRESERVE_D = x.GetString("ZFPRESERVE_D") ?? "",
                    ZZSE = x.GetString("ZZSE") ?? "",
                    NETPR = x.GetString("NETPR") ?? "",
                    WAERS = x.GetString("WAERS") ?? "",
                    DMBTR = x.GetString("DMBTR") ?? "",
                    ERDAT = x.GetString("ERDAT") ?? "",
                    UDATE = x.GetString("UDATE") ?? ""
                }
                 ).ToList();

                AddTempTable(Z_MM_QUBE_MATNR_List);
                result = JsonConvert.SerializeObject(Z_MM_QUBE_MATNR_List);
               
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

        public static string GetAllProducts(string ProductsList)
        {
            string[] ProductArray = ProductsList.Split(',');

            IRfcTable IRetTable = null;
            List<Z_MM_QUBE_MATNR> Z_MM_QUBE_MATNR_List = new List<Z_MM_QUBE_MATNR>();
            RfcConfigParameters parms = RFC.GetSettingParms();
            RfcDestination rfcDest = RfcDestinationManager.GetDestination(parms);
            RfcRepository rfcRep = rfcDest.Repository;
            IRfcFunction IReader = rfcRep.CreateFunction("Z_MM_QUBE_MATNR");
            IReader.Invoke(rfcDest);
            IRfcTable Itab = IReader.GetTable("IT_MATNR");

            for(int i = 0; i < ProductArray.Length; i++)
            {
                Itab.Append();
                Itab[i].SetValue("MATNR", ProductArray[i].ToString());
            }

            IReader.SetValue("IT_MATNR", Itab);
            IReader.Invoke(rfcDest);

            IRetTable = IReader.GetTable("ET_MATNR");
            Z_MM_QUBE_MATNR_List = IRetTable.AsQueryable().Select(x => new Z_MM_QUBE_MATNR
            {
                MATNR = x.GetString("MATNR") ?? "",
                MAKTX = x.GetString("MAKTX") ?? "",
                ZZCT2 = x.GetString("ZZCT2") ?? "",
                LIFNR = x.GetString("LIFNR") ?? "",
                ZZMATU = x.GetString("ZZMATU") ?? "",
                ZFVOLUME = x.GetString("ZFVOLUME") ?? "",
                ZZPAGE = x.GetString("ZZPAGE") ?? "",
                WGBEZ = x.GetString("WGBEZ") ?? "",
                ZFPRESERVE_D = x.GetString("ZFPRESERVE_D") ?? "",
                ZZSE = x.GetString("ZZSE") ?? "",
                NETPR = x.GetString("NETPR") ?? "",
                WAERS = x.GetString("WAERS") ?? "",
                DMBTR = x.GetString("DMBTR") ?? "",
                ERDAT = x.GetString("ERDAT") ?? "",
                UDATE = x.GetString("UDATE") ?? ""
            }
             ).ToList();

            AddTempTable(Z_MM_QUBE_MATNR_List);

            string result = JsonConvert.SerializeObject(Z_MM_QUBE_MATNR_List);
            return result;
        }

        public static void AddTempTable(List<Z_MM_QUBE_MATNR> Z_MM_QUBE_MATNR_List)
        {
            using (var db = new CubeRFCEntities())
            {

                var ProductList = db.Product.AsEnumerable().Select(x => x.BatchNo).ToList();
                int BatchNo = ProductList.Count==0?1:ProductList.Max() + 1;

                foreach (var item in Z_MM_QUBE_MATNR_List)
                {
                    if (item.UDATE == "0000-00-00")
                    {
                        item.UDATE = "1900-01-01";
                    }

                    string InsertCommand = "Insert Into Product " +
                        "(MATNR,MAKTX,ZZCT2,LIFNR,ZZMATU,ZFVOLUME,ZZPAGE,WGBEZ,ZFPRESERVE_D,ZZSE,NETPR,WAERS,DMBTR,ERDAT,UDATE,CreateDate,BatchNo) " +
                        "values(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,GetDate(),@p15) ;";

                    db.Database.ExecuteSqlCommand(InsertCommand.ToString(), item.MATNR, item.MAKTX, item.ZZCT2, item.LIFNR, item.ZZMATU, item.ZFVOLUME, item.ZZPAGE, item.WGBEZ, item.ZFPRESERVE_D, item.ZZSE, item.NETPR, item.WAERS, item.DMBTR, item.ERDAT, item.UDATE, BatchNo);
                }

                #region EntityFrameWork的寫法
                //foreach (var item in Z_MM_QUBE_MATNR_List)
                //{
                //    db.Product.Add(new Product()
                //    {
                //        MATNR = item.MATNR,
                //        MAKTX = item.MAKTX,
                //        ZZCT2 = item.ZZCT2,
                //        LIFNR = item.LIFNR,
                //        ZZMATU = item.ZZMATU,
                //        ZFVOLUME = item.ZFVOLUME,
                //        ZZPAGE = item.ZZPAGE,
                //        WGBEZ = item.WGBEZ,
                //        ZFPRESERVE_D = item.ZFPRESERVE_D,
                //        ZZSE = item.ZZSE,
                //        NETPR = item.NETPR,
                //        WAERS = item.WAERS,
                //        DMBTR = item.DMBTR,
                //        ERDAT = DateTime.Parse(item.ERDAT),
                //        UDATE = item.UDATE == "0000-00-00" ? DateTime.Parse("1900-01-01") : DateTime.Parse(item.UDATE),
                //        CreateDate = DateTime.Now
                //    });
                //}

                //try { db.SaveChanges(); }
                //catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                //{
                //    throw ex;
                //}
                #endregion

            }
        }
    }

   

    public class Z_MM_QUBE_MATNR
    {
        //SAP料號
        public string MATNR = "";
        //商品中文名稱
        public string MAKTX = "";
        //作品名2
        public string ZZCT2 = "";
        //供應商編號
        public string LIFNR = "";
        //媒材
        public string ZZMATU = "";
        //容量(組件)
        public string ZFVOLUME = "";
        //所有人
        public string ZZPAGE = "";
        //作品類型
        public string WGBEZ = "";
        //年代
        public string ZFPRESERVE_D = "";
        //作品尺幅(寬)
        public string ZZSE = "";
        //採購價
        public string NETPR = "";
        //採購幣
        public string WAERS = "";
        //成本價
        public string DMBTR = "";
        //新增日期
        public string ERDAT = "";
        //修改日期
        public string UDATE = "";

    }
}