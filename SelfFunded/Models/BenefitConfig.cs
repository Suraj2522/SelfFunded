using System;

namespace SelfFunded.Models
{
    public class BenefitConfig
    {
        public int benfitTransId { get; set; }
     
        public string policyId { get; set; }
        public string planId { get; set; }
        public int benefit { get; set; }
        public string? level1 { get; set; }
        public string? level2 { get; set; }
        public string? level3 { get; set; }
        public string limit { get; set; }
        public string applicablePercentage { get; set; }
        public string applicableOn { get; set; }
        public string fixedAmount { get; set; }
        public string _operator { get; set; }
        public string sessionAllowed { get; set; }
        public string selectedCheckbox { get; set; }
        public string benefitGender { get; set; }
        public string minAge { get; set; }
        public string maxAge { get; set; }
        public string incrementalApplicable { get; set; }
        public string coPayApplicable { get; set; }
        public string coPayApplicableOn { get; set; }
        public string coPayPercentage { get; set; }
        public string coPayFixedAmount { get; set; }
        public string coPayOperator { get; set; }
        public string claimType { get; set; }
        public string waitingApplicable { get; set; }
        public string waiting { get; set; }
        public string applicableFromDate { get; set; }
        public string payableFrom { get; set; }
        public string groupItemwise { get; set; }
        public string providerType { get; set; }
        public string annualLimit { get; set; }
        public string annualLimitValuesInDays { get; set; }
        public string exclusionDaysFromDOA { get; set; }
        public string maximumPayableDays { get; set; }
        public string minimumHospitalizationDays { get; set; }
        public string natureOfStay { get; set; }
        public string relation { get; set; }
        public string searchFor { get; set; }
        public int isActive { get; set; }
        public int createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }
    }
}
