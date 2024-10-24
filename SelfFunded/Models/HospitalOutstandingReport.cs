using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class HospitalOutstandingReport
    {
        public int? insuranceID { get; set; }
        public string? debitNoteNo { get; set; }
        public string? invoiceNo { get; set; }
        public string? claimNO { get; set; }
        public string? outwardNo { get; set; }
        //public DateTime? fromDate { get; set; }
        //public DateTime? toDate { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
    }
}
