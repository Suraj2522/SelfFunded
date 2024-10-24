using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class PolicyMaster
    {
        public PolicyPremiumInfo premiumInfo { get; set; }
        public PolicyLiveDetails liveDetails { get; set; }  

        public ImpPolicyInfo impPolicyInfo { get; set; }
        public CrmDetails crmDetails { get; set; } 
        public ContactDetails contactInfo { get; set; }
        
        public Coinsurance coinsurance { get; set; }
        public int srNo { get; set; }
        public int policyId { get; set; }
        public string insurance { get; set; }
        public int insuranceId {  get; set; }
        public string uWOffice { get; set; }

        public int policyNo { get; set; }
        public DateTime? validFrom { get; set; }
        public DateTime? validTo { get; set; }
        public string corporateName { get; set; }
        public int corporateId { get; set; }
        public string groupCode { get; set; }
        public string frequencyofEndorsement { get; set; }
        public string typeofPolicy { get; set; }
        public string floater { get; set; }
        public string typeofIndustries { get; set; }
        
        public int industryId {  get; set; }
        public string address { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public int pinCode { get; set; }
        public string serviceModel { get; set; }
        public string remarks { get; set; }
        
           
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }
        public int isActive { get; set; }   
        
        public int cityID { get; set; }
        public int countryID { get; set; }
        public int stateID { get; set; }
       
    }
}
