using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class PolicyPremiumInfo
    {
        public int srNo { get; set; }
        public int premiumId { get; set; }
        public int policyId { get; set; }
        public int premiumType { get; set; }
        public string policyPremium { get; set; }
        public string addition { get; set; }
        public string deletion { get; set; }
        public string nilEndorsement { get; set; }
        public string refund { get; set; }
        public string extra { get; set; }
        public string netPremium { get; set; }       
        
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }
        public int isActive { get; set; }

    }
}
