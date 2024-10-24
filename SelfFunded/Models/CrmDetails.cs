using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class CrmDetails
    {
        public int srNo { get; set; }
        public int crmId { get; set; }
        public int policyId { get; set; }
        public string crmServicingLocation { get; set; }
        public string crmVerticalHead { get; set; }
        public string mobileNo { get; set; }
        public string emailId { get; set; }
        public string brokerName { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }
        public int isActive { get; set; }
    }
}
