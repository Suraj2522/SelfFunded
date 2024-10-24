using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class DebitNote
    {
        public int? userId { get; set; }
        public int? insuranceID { get; set; }
        public string claimNumber { get; set; }
        public int? numberOfClaims { get; set; }
        public decimal amount { get; set; }
        public int? accountId { get; set; }

        public int? planId { get; set; }
        public int? claimTypeId { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string insuredName { get; set; }
        public string policyNo { get; set; }
        public string claimNo { get; set; }
        public int? claimId { get; set; }
        public string? claimIds { get; set; }
        public string insuranceCompany { get; set; }
        public string inboundOutbound { get; set; }
        public DateTime treatmentDate { get; set; }
        public int lengthOfStay { get; set; }
        public decimal netAmount { get; set; }
        public DateTime dischargeDate { get; set; }
        public int? insuranceCompanyId { get; set; }
        public string caseType { get; set; }
        public string preAuthID { get; set; }
        public string claimType { get; set; }
    }
}
