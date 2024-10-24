using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class ImpPolicyInfo
    {
        public int srNo { get; set; }
        public int iD { get; set; }
        public int policyId { get; set; }
        public int corporateFloat { get; set; }
        public int fastTrackCorp { get; set; }
        public string cSICorporateFloat { get; set; }
        public string cSICorporateFloatUtilized { get; set; }
        public string cSICorporateFloatBalance { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }
        public int isActive { get; set; }
    }
}
