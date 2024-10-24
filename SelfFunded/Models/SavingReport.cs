using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class SavingReport
    {
        public int? insuranceCompanyId { get; set; }
        public string? preAuthNumber { get; set; }
        public string? insuredName { get; set; }

        public string? patientName { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
    }
}
