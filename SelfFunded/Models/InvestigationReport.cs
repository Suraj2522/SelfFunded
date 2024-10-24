using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class InvestigationReport
    {
        public int? insuranceId { get; set; }
        public string? insuredName { get; set; }
        public string? claimId { get; set; }
        //public DateTime? fromDate { get; set; }
        //public DateTime? toDate { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? orderByCol { get; set; }
    }
}
