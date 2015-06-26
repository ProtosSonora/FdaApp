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
       
        [AcceptVerbs(HttpVerbs.Post)]
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
                    string _parameters = string.Empty;
                    string _parametersWithoutLimit = string.Empty;
                    string baseUrl = string.Empty;
                    
                    string selectedCount = string.Empty;
                    string searchTitle = string.Empty;
                    if (!string.IsNullOrWhiteSpace(objFDAInputModel.url))
                    {
                        baseUrl = objFDAInputModel.url;
                    }
                    if (!string.IsNullOrWhiteSpace(objFDAInputModel.selectedtype))
                    {
                        selectedtype = objFDAInputModel.selectedtype;
                    }
                    if (!string.IsNullOrWhiteSpace(objFDAInputModel.selectedcount))
                    {
                        selectedCount = objFDAInputModel.selectedcount;
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
                            if(selectedCount == "report_date" || selectedCount == "recall_initiation_date")
                            {
                                if (_parameters != "")
                                { _parametersWithoutLimit += "&search=" + selectedCount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }
                                else
                                { _parametersWithoutLimit += "search=" + selectedCount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }

                                if (!string.IsNullOrWhiteSpace(searchTitle))
                                {
                                    _parametersWithoutLimit += "+AND+_exists_:" + searchTitle;
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(searchTitle))
                            {
                                if (_parameters != "")
                                { _parametersWithoutLimit += "&search=" + selectedCount + ":" + '"' + objFDAInputModel.searchTitle + '"'; }
                                else
                                { _parametersWithoutLimit += "search=" + selectedCount + ":"+ '"' + objFDAInputModel.searchTitle + '"'; }
                            }

                            _parameters += _parametersWithoutLimit;
                        }
                        else if (selectedtype == "Device")
                        {
                            if (selectedCount == "date_of_event" || selectedCount == "date_report" || selectedCount == "date_received")
                            {
                                if (_parameters != "")
                                { _parametersWithoutLimit += "&search=" + selectedCount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }
                                else
                                { _parametersWithoutLimit += "search=" + selectedCount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }

                                if (!string.IsNullOrWhiteSpace(searchTitle))
                                {
                                    _parametersWithoutLimit += "+AND+_exists_:" + searchTitle;
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(searchTitle))
                            {
                                if (_parameters != "")
                                { _parametersWithoutLimit += "&search=" + selectedCount + ":" + '"' + objFDAInputModel.searchTitle + '"'; }
                                else
                                { _parametersWithoutLimit += "search=" + selectedCount + ":" + '"' + objFDAInputModel.searchTitle + '"'; }
                            }
                            _parameters += _parametersWithoutLimit;
                        }
                        else if (selectedtype == "Drug")
                        {
                            if (selectedCount == "receivedate" || selectedCount == "receiptdate" || selectedCount == "transmissiondate")
                            {
                                if (_parameters != "")
                                { _parametersWithoutLimit += "&search=" + selectedCount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }
                                else
                                { _parametersWithoutLimit += "search=" + selectedCount + ":[" + objFDAInputModel.fromDate + "+TO+" + objFDAInputModel.toDate + "]"; }

                                if (!string.IsNullOrWhiteSpace(searchTitle))
                                {
                                    _parametersWithoutLimit += "+AND+_exists_:" + searchTitle;
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(searchTitle))
                            {
                                if (_parameters != "")
                                { _parametersWithoutLimit += "&search=" + selectedCount + ":" + '"' + objFDAInputModel.searchTitle + '"'; }
                                else
                                { _parametersWithoutLimit += "search=" + selectedCount + ":" + '"' + objFDAInputModel.searchTitle + '"'; }
                            }
                            _parameters += _parametersWithoutLimit;
                        }
                    }

                    if (_parameters != "")
                    {
                        baseUrl += "?" + _parameters;
                    }

                    WebClient webAPi = new WebClient();
                    Stream streamFDAApiResult = webAPi.OpenRead(baseUrl);
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
                string errorMessage = string.Empty;
                if (((System.Net.WebException)(ex)).Status == WebExceptionStatus.ProtocolError)
                {
                    errorMessage = "No matches found!";
                }
                else
                {
                    errorMessage = ex.Message;
                }

                if (selectedtype == "Food")
                {
                    return Json(new { GridList = objFoodList, ChartList = objCountResultsChartList, Message = errorMessage }, JsonRequestBehavior.AllowGet);
                }
                else if (selectedtype == "Device")
                {
                    return Json(new { GridList = objDeviceList, ChartList = objCountResultsChartList, Message = errorMessage }, JsonRequestBehavior.AllowGet);
                }
                else if (selectedtype == "Drug")
                {
                    return Json(new { GridList = objDrugList, ChartList = objCountResultsChartList, Message = errorMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
    }
}