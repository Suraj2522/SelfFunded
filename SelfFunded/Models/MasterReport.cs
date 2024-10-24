using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class MasterReport
    {
        public int? insuranceCompanyId { get; set; }
        public string preAuthNumber { get; set; }
        public int? diagnosisId { get; set; }
        public string insuredName { get; set; }
        public string policyNo { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string type { get; set; }
        public string claimStatus { get; set; }
        public int? providerId { get; set; }
        public int? ailmentId { get; set; }
        public string insuranceType { get; set; }
        public int? groupPolicyId { get; set; }
        public int loginTypeId { get; set; }
        public string insuranceIDs { get; set; }

        public string cityName { get; set; }
      //  public int? providerId { get; set; }
        public string providerNo { get; set; }
        public string utrNo { get; set; }
        public string employeeCode { get; set; }
      //  public int? groupPolicyId { get; set; }
        public string remarks { get; set; }
        public string phsFirNo { get; set; }
    }
}
