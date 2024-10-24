using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class FundReceivedReport
    {

        int?  sRNo { get; set; }
        public int? insuranceID { get; set; }
        public int? debitID { get; set; }
       // public string?  errMsg { get; set; }
        public string? claimNO { get; set; }
        public string? insuranceCompany { get; set; }
        public string? insuredName { get; set; }
        public string? providerName { get; set; }
        public string? policyNo { get; set; }
        public string? debitNoteNo { get; set; }
        public string? toCurrencyCode { get; set; }
        public decimal? invoiceProviderAmount { get; set; }
        public decimal? feesInvoiceAmount { get; set; }
        public decimal? assistanceFees { get; set; }
        public decimal? totalReceivable { get; set; }
        public string? stateName { get; set; }
        public DateTime? outwardDate { get; set; }
        public string? outwardNo { get; set; }
        public int? aging { get; set; }
        public string? certificateNo { get; set; }
        public decimal? invoiceReceivedAmount { get; set; }
        public decimal? feesReceivedAmount { get; set; }
        public decimal? providerOutstandingAmount { get; set; }
        public decimal? feesOutstandingAmount { get; set; }
        public decimal? assistanceFeesOutstanding { get; set; }
        public decimal? totalOutstanding { get; set; }
        public string? remarks { get; set; }
        public int? debitAutoId { get; set; }
       // public int? insuranceId { get; set; }
        public decimal? providerExcessAmount { get; set; }
        public string? assistanceName { get; set; }
        public decimal? phmFeesExcessAmount { get; set; }
        public int? visibleView { get; set; }
    }
}
