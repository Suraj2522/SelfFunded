using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class PhmWeeklyReport
    {
        public int? insuranceId { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
    }
}
