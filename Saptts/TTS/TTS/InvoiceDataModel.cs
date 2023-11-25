using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace TTS
{
    public class InvoiceDataModel
    {
        public string InvoiceHead { get; set; }
        public string WaterMarkHead { get; set; }
        public string Plant { get; set; }
        public string Category { get; set; }
        public string LogoPath { get; set; }
        public string CustCode { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public bool IsProfoma { get; set; }
        public string RefNo { get; set; }
        public int ReviseCount { get; set; }
        public string OrderRefNo { get; set; }
        public string OrderDate { get; set; }
        public string TransporterName { get; set; }
        public string ModeOfTransport { get; set; }
        public string VehicleNo { get; set; }
        public string LRNo { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal CGSTVal { get; set; }
        public decimal SGSTVal { get; set; }
        public decimal IGSTVal { get; set; }
        public string FreightCharges { get; set; }
        public string CcAttached { get; set; }
        public string CreditNote { get; set; }
        public string Paymentdays { get; set; }
        public DataTable dtHsnCode { get; set; }
        public bool PartNo { get; set; }
        public int OID { get; set; }
        public string IRN { get; set; }
        public string IrnQrCode { get; set; }
        public string AckDt { get; set; }
        public long AckNo { get; set; }
        public InvoiceDataModel()
        {

        }
    }
}