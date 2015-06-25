using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FdaApp.Models
{
    public class FDAModel
    {
        public FDAModel()
        {
            FDATypesList = new List<SelectListItem>();
            FDACountsList = new List<SelectListItem>();
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Limit { get; set; }
        public int Skip { get; set; }
        public string Count { get; set; }
        public List<SelectListItem> FDATypesList { get; set; }
        public int SelectedType { get; set; }
        public string SelectedCount { get; set; }
        public List<SelectListItem> FDACountsList { get; set; }

        public string SearchTitle { get; set; }
    }

    public class FDAInputModel
    {
        public string url { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public int limit { get; set; }
        public int skip { get; set; }
        public string selectedcount { get; set; }
        public string selectedtype { get; set; }
        public string searchTitle { get; set; }
    }

    public class Food
    {
        public string recall_number { get; set; }
        public string reason_for_recall { get; set; }
        public string status { get; set; }
        public string distribution_pattern { get; set; }
        public string product_quantity { get; set; }
        public string recall_initiation_date { get; set; }
        public string state { get; set; }
        public string event_id { get; set; }
        public string product_type { get; set; }
        public string product_description { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string recalling_firm { get; set; }
        public string report_date { get; set; }
        public string @epoch { get; set; }
        public string voluntary_mandated { get; set; }
        public string classification { get; set; }
        public string code_info { get; set; }
        public string @id { get; set; }
        //public List<string> openfda { get; set; }
        public string initial_firm_notification { get; set; }
    }

    public class Meta
    {
        public string disclaimer { get; set; }
        public string license { get; set; }
        public DateTime last_updated { get; set; }
        public Metaresults results { get; set; }
    }

    public class Metaresults
    {
        public int skip { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
    }

    public class FoodMeta
    {
        public Meta Meta { get; set; }
        public List<Food> results { get; set; }
    }

    public class ChartMeta
    {
        public Meta Meta { get; set; }
        public List<CountResults> results { get; set; }
    }

    public class CountResults
    {
        public string time { get; set; }
        public string term { get; set; }
        public int count { get; set; }
    }

    public class Device
    {
        public string report_number { get; set; }
        public string manufacturer_contact_state { get; set; }
        public string event_type { get; set; }
        public string manufacturer_g1_address_1 { get; set; }
        public string manufacturer_contact_pcountry { get; set; }
        public string report_to_manufacturer { get; set; }
        public string previous_use_code { get; set; }
        public string manufacturer_postal_code { get; set; }
        public string manufacturer_city { get; set; }
        public string reporter_occupation_code { get; set; }
        public string date_of_event { get; set; }
        public string manufacturer_g1_country { get; set; }
        public string manufacturer_g1_city { get; set; }
        public string date_received { get; set; }
        public string date_report { get; set; }
        public string manufacturer_contact_zip_code { get; set; }
        public string manufacturer_contact_country { get; set; }
        public string report_source_code { get; set; }
        public string date_manufacturer_received { get; set; }
        public string manufacturer_contact_city { get; set; }
        public string event_key { get; set; }
        public string mdr_report_key { get; set; }
    }

    public class mdr_text{
        public string text { get; set; }
        public string mdr_text_key { get; set; }
        public string text_type_code { get; set; }
        public int patient_sequence_number { get; set; }
    }

    public class DeviceMeta
    {
        public Meta Meta { get; set; }
        public List<Device> results { get; set; }
    }

    public class Drug
    {
        //public string set_id { get; set; }
        //public string version { get; set; }
        ////public string id { get; set; }
        //public string effective_time { get; set; }
        //public List<string> indications_and_usage { get; set; }
        //public List<string> keep_out_of_reach_of_children { get; set; }
        //public List<string> dosage_and_administration { get; set; }
        //public List<string> purpose { get; set; }
        //public List<string> package_label_principal_display_panel { get; set; }
        //public List<string> active_ingredient { get; set; }
        //public List<string> inactive_ingredient { get; set; }

        public string safetyreportid { get; set; }
        public string receivedate { get; set; }
        public string receiptdate { get; set; }
        public string transmissiondate { get; set; }
        public string companynumb { get; set; }
        public Patient patient { get; set; }
    }
    
    public class Patient
    {
        public List<reaction> reaction { get; set; }
        public List<PatientDrug> drug { get; set; }
    }

    public class reaction
    {
        public string reactionmeddrapt { get; set; }
        public string reactionmeddraversionpt { get; set; }
    }

    public class PatientDrug
    {
        public string drugauthorizationnumb { get; set; }
        public string drugindication { get; set; }
        public string medicinalproduct { get; set; }
        public string drugdosagetext { get; set; }
        public string drugstartdate { get; set; }
        public string drugenddate { get; set; }
    }

    public class DrugMeta
    {
        public Meta Meta { get; set; }
        public List<Drug> results { get; set; }
    }
}
