using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class PlanDetails
    {
        public int srNo { get; set; }
        public int planId { get; set; }
        public int policyNo { get; set; }
        public int policyId { get; set; }
        public string planCodeExternal { get; set; }
        public string copayBuyBack { get; set; }
        public string planDescription { get; set; }       
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }
        public int isActive { get; set; }
    }
}
