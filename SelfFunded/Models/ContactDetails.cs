using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfFunded.Models
{
    public class ContactDetails
    {
        public int id { get; set; }
        public int policyId { get; set; }
        public  string mobileNo { get; set; }
        public string emailId { get; set; }
        public string designation { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }
        public int isActive { get; set; }
    }
}
