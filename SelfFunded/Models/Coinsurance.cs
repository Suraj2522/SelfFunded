using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class Coinsurance
    {
        public int srNo { get; set; }
        public int coinsuranceId { get; set; }
        public int policyId { get; set; }
        public required string coinsurance { get; set; }
        public string coinsuranceratio { get; set; }
        public string businessReceivePersonName { get; set; }
        public string rennewalpolicy { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }
        public int isActive { get; set; }
    }
}
