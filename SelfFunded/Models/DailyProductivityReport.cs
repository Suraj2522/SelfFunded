using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class DailyProductivityReport
    {
        public int? insuranceId { get; set; }
        public int? userCode { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string firstName { get; set; }
        public int? total { get; set; }
    }
}
