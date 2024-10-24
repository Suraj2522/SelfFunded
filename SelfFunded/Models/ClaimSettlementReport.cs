using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class ClaimSettlementReport
    {
        public int? insuranceID { get; set; }
        public string? insuredName { get; set; }
        public string? claimNO { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? providerNo { get; set; }
    }
}
