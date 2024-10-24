using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class TDSReport
    {
        public int?  srNo { get; set; }
        public int? insuranceId { get; set; }
        public int? providerNo { get; set; }
        public string? claimNo { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? orderByCol { get; set; }
    }
}
