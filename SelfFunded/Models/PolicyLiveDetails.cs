using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class PolicyLiveDetails
    {
        public int srNo { get; set; }
        public int lDID { get; set; }
        public int policyId { get; set; }
        public string atPolicyInception { get; set; }        
        public string addition { get; set; }
        public string deletion { get; set; }
        public string liveActive { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }
        public int isActive { get; set; }

    }
}
