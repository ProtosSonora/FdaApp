using FdaApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;


namespace FdaApp.Controllers
{
    public class FDAController : Controller
    {
        // GET: FDA
        public ActionResult Index()
        {
            try
            {
                FDAModel objFDAModel = new FDAModel();
                objFDAModel.FDATypesList.Add(new SelectListItem { Text = "Food", Value = "1" });
                objFDAModel.FDATypesList.Add(new SelectListItem { Text = "Device", Value = "2" });
                objFDAModel.FDATypesList.Add(new SelectListItem { Text = "Drug", Value = "3" });
               
                objFDAModel.FDACountsList.Add(new SelectListItem { Text = "Select Count", Value = "" });

                return View(objFDAModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        ////public ActionResult GetResult(FDAModel objFDAModel)
        //public ActionResult GetResult(string url)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(url))
        //        {
        //            string baseurl = string.Empty;
        //            //string fromdate = objFDAModel.FromDate.ToShortDateString();
        //            //string todate = objFDAModel.ToDate.ToShortDateString();

        //            //string parameters = string.Empty;
        //            //if (objFDAModel.SelectedType == 1)
        //            //{
        //            //    baseurl = "https://api.fda.gov/food/enforcement.json?";
        //            //}
        //            //else if (objFDAModel.SelectedType == 2)
        //            //{
        //            //    baseurl = "https://api.fda.gov/device/event.json?";
        //            //}
        //            //else if (objFDAModel.SelectedType == 3)
        //            //{
        //            //    baseurl = "https://api.fda.gov/drug/label.json?";
        //            //}

        //            //if (!string.IsNullOrWhiteSpace(objFDAModel.SelectedCount))
        //            //{
        //            //    parameters = "count=" + objFDAModel.SelectedCount;
        //            //}
        //            //if (objFDAModel.Limit>0)
        //            //{
        //            //    parameters = "limit=" + objFDAModel.Limit;
        //            //}
        //            //if (objFDAModel.Skip >= 0)
        //            //{
        //            //    parameters = "skip=" + objFDAModel.Skip;
        //            //}
        //            //if (objFDAModel.FromDate != null && objFDAModel.ToDate != null)
        //            //{
        //            //    //[19910101+TO+20150101]
        //            //    string FYear = objFDAModel.FromDate.Year.ToString();
        //            //    string FMonth = objFDAModel.FromDate.Month.ToString();
        //            //    string FDate = objFDAModel.FromDate.Date.ToString();
        //            //    string TYear = objFDAModel.ToDate.Year.ToString();
        //            //    string TMonth = objFDAModel.ToDate.Month.ToString();
        //            //    string TDate = objFDAModel.ToDate.Date.ToString();

        //            //    parameters = "search=" + objFDAModel.Skip;
        //            //}

        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return null;
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult GetResult(FDAModel objFDAModel)
        public ActionResult GetResult(FDAInputModel objFDAInputModel)
        {
            string selectedtype = string.Empty;
            List<Food> objFoodList = new List<Food>();
            List<Device> objDeviceList = new List<Device>();
            List<Drug> objDrugList = new List<Drug>();
            List<CountResults> objCountResultsChartList = new List<CountResults>();
            try
            {
                if (objFDAInputModel!=null)
                {
                    string url = string.Empty;
                    string _parameters = string.Empty;
                    string _parameterswithoutlimit = string.Empty;
                    string baseurl = string.Empty;
                    
                    string selectedcount = string.Empty;
                    string searchTitle = string.Empty;
                    if (!string.IsNullOrWhiteSpace(objFDAInputModel.url))
                    {
                        baseurl = objFDAInputModel.url;
                        url = objFDAInputModel.url;
                    }
                    if (!string.IsNullOrWhiteSpace(objFDAInputModel.selectedtype))
                    {
                        selectedtype = objFDAInputModel.selectedtype;
                    }
                    if (!string.IsNullOrWhiteSpace(objFDAInputModel.selectedcount))
                    {
                        selectedcount = objFDAInputModel.selectedcount;
                    }

                    if (!string.IsNullOrWhiteSpace(objFDAInputModel.searchTitle))
                    {
                        searchTitle = objFDAInputModel.searchTitle;
                    }

                    if (objFDAInputModel.limit > 0)
                    {
                        if (_parameters != "")
                        { _parameters += "&limit=" + objFDAInputModel.limit; }
                        else
                        { _parameters += "limit=" + objFDAInputModel.limit; }
                    }
                    if ((objFDAInputModel.skip > 0) || (objFDAInputModel.limit > 0 && objFDAInputModel.skip == 0))
                    {
                        if (_parameters != "")
                        { _parameters += "&skip=" + objFDAInputModel.skip; }
                        else
                        { _parameters += "skip=" + objFDAInputModel.skip; }
                    }

                    if (!string.IsNullOrWhiteSpace(objFDAInputModel.fromDate) && !string.IsNullOrWhiteSpace(objFDAInputModel.toDate))
                    {
                        if (selectedtype == "Food"){
                            if(selectedcount == "report_date" || selectedcount == "recall_initiation_date")
                            {
                                if (_parameters != "")
                                { _parameterswithoutlimit += "&search=" + selectedcount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }
                                else
                                { _parameterswithoutlimit += "search=" + selectedcount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }

                                if (!string.IsNullOrWhiteSpace(searchTitle))
                                {
                                    _parameterswithoutlimit += "+AND+_exists_:" + searchTitle;
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(searchTitle))
                            {
                                if (_parameters != "")
                                { _parameterswithoutlimit += "&search=" + selectedcount + ":" + '"' + objFDAInputModel.searchTitle + '"'; }
                                else
                                { _parameterswithoutlimit += "search=" + selectedcount + ":"+ '"' + objFDAInputModel.searchTitle + '"'; }
                            }

                            _parameters += _parameterswithoutlimit;
                        }
                        else if (selectedtype == "Device")
                        {
                            if (selectedcount == "date_of_event" || selectedcount == "date_report" || selectedcount == "date_received")
                            {
                                if (_parameters != "")
                                { _parameterswithoutlimit += "&search=" + selectedcount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }
                                else
                                { _parameterswithoutlimit += "search=" + selectedcount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }

                                if (!string.IsNullOrWhiteSpace(searchTitle))
                                {
                                    _parameterswithoutlimit += "+AND+_exists_:" + searchTitle;
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(searchTitle))
                            {
                                if (_parameters != "")
                                { _parameterswithoutlimit += "&search=" + selectedcount + ":" + '"' + objFDAInputModel.searchTitle + '"'; }
                                else
                                { _parameterswithoutlimit += "search=" + selectedcount + ":" + '"' + objFDAInputModel.searchTitle + '"'; }
                            }
                            _parameters += _parameterswithoutlimit;
                        }
                        else if (selectedtype == "Drug")
                        {
                            if (selectedcount == "receivedate" || selectedcount == "receiptdate" || selectedcount == "transmissiondate")
                            {
                                if (_parameters != "")
                                { _parameterswithoutlimit += "&search=" + selectedcount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }
                                else
                                { _parameterswithoutlimit += "search=" + selectedcount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }

                                if (!string.IsNullOrWhiteSpace(searchTitle))
                                {
                                    _parameterswithoutlimit += "+AND+_exists_:" + searchTitle;
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(searchTitle))
                            {
                                if (_parameters != "")
                                { _parameterswithoutlimit += "&search=" + selectedcount + ":" + '"' + objFDAInputModel.searchTitle + '"'; }
                                else
                                { _parameterswithoutlimit += "search=" + selectedcount + ":" + '"' + objFDAInputModel.searchTitle + '"'; }
                            }
                            _parameters += _parameterswithoutlimit;
                        }
                    }
                    //if (objFDAInputModel.limit == 0 && objFDAInputModel.skip == 0)
                    //{
                    //    if (selectedcount != "")
                    //    {
                    //        if (_parameterswithoutlimit != "")
                    //        { _parameterswithoutlimit += "&count=" + selectedcount; }
                    //        else
                    //        { _parameterswithoutlimit += "count=" + selectedcount; }
                    //        _parameters = _parameterswithoutlimit;
                    //    }
                    //}

                    if (_parameters != "")
                    {
                        baseurl += "?" + _parameters;
                    }

                    JObject JsonResultGrid = null;
                    WebClient webAPi = new WebClient();
                    Stream streamFDAApiResult = webAPi.OpenRead(baseurl);
                    StreamReader sReaderFDAApiResult = new StreamReader(streamFDAApiResult);
                    string sHtmlFDAApiResult = sReaderFDAApiResult.ReadToEnd();
                    
                    if (selectedtype == "Food")
                    {
                        var items = JsonConvert.DeserializeObject<FoodMeta>(sHtmlFDAApiResult);
                        objFoodList = items.results.ToList();
                    }
                    else if (selectedtype == "Device")
                    {
                        var items = JsonConvert.DeserializeObject<DeviceMeta>(sHtmlFDAApiResult);
                        objDeviceList = items.results.ToList();
                    }
                    else if (selectedtype == "Drug")
                    {
                        var items = JsonConvert.DeserializeObject<DrugMeta>(sHtmlFDAApiResult);
                        objDrugList = items.results.ToList();
                    }


                    

                    if (_parameterswithoutlimit != "")
                    {
                        url += "?" + _parameterswithoutlimit;
                    }

                    //JObject JsonResultChart = null;

                    Stream streamFDAApiResultChart = webAPi.OpenRead(url);
                    StreamReader sReaderFDAApiResultChart = new StreamReader(streamFDAApiResultChart);
                    string sHtmlFDAApiResultChart = sReaderFDAApiResultChart.ReadToEnd();
                    //JsonResultChart = JObject.Parse(sHtmlFDAApiResultChart);
                    //return objFoodList;

                    
                    var chartItems = JsonConvert.DeserializeObject<ChartMeta>(sHtmlFDAApiResultChart);
                    objCountResultsChartList = chartItems.results.ToList();

                    //return Json(objFoodList);
                    if (selectedtype == "Food")
                    {
                        return Json(new { GridList = objFoodList, ChartList = objCountResultsChartList, Message="" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (selectedtype == "Device")
                    {
                        return Json(new { GridList = objDeviceList, ChartList = objCountResultsChartList, Message = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (selectedtype == "Drug")
                    {
                        return Json(new { GridList = objDrugList, ChartList = objCountResultsChartList, Message = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                if (selectedtype == "Food")
                {
                    return Json(new { GridList = objFoodList, ChartList = objCountResultsChartList, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                else if (selectedtype == "Device")
                {
                    return Json(new { GridList = objDeviceList, ChartList = objCountResultsChartList, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                else if (selectedtype == "Drug")
                {
                    return Json(new { GridList = objDrugList, ChartList = objCountResultsChartList, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //throw new HttpResponseException(ThrowCustomException(ex));
                    return null;
                }
            }
            return null;
        }

        //public static HttpResponseMessage ThrowCustomException(Exception ex)
        //{
        //    HttpResponseMessage objMSG = new System.Net.Http.HttpResponseMessage();
        //    if (((System.Web.Http.HttpResponseException)(ex)).Response.StatusCode.ToString() == "Unauthorized")
        //    {
        //        objMSG.StatusCode = HttpStatusCode.Unauthorized;
        //    }
        //    else
        //    {
        //        objMSG.StatusCode = HttpStatusCode.InternalServerError;
        //    }
        //    objMSG.ReasonPhrase = ex.Message;

        //    return objMSG;
        //}

        public JsonResult ConvertstringToJsonArray(object jsonobj)
        {
            return Json(jsonobj, JsonRequestBehavior.AllowGet);
        }
    }
}