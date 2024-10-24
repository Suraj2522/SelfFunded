using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfFunded.Models
{
    public class Dashboard
    {
        public string claimStatus { get; set; }
        public string status { get; set; }
        public string claimCount { get; set; }
        public string claimType { get; set; }
        public string preAuthCount { get; set; }
        public string isactive { get; set; }
        public string caseType { get; set; }
        public string recStatus { get; set; }
        public string ageGroup { get; set; }
        public int male { get; set; }
        public int female { get; set; }
        public int total { get; set; }
        public string percentage { get; set; }
        public string enrollmentCount { get; set; }
        public string typeOfEnrollment { get; set; }
        public string relationType { get; set; }
        public int other { get; set; }

        public string intimationCount { get; set; }
        public string intimationYear { get; set; }
        public string insuranceCompany { get; set; }
    }


}
