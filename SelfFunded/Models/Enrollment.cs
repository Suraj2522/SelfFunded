namespace SelfFunded.Models
{
    public class Enrollment
    {


        public Enrollment() { }
        public IFormFile file { get; set; }

        public int? insuranceId { get; set; }
        //public int? insuranceCompanyId { get; set; }
        public int? srNo { get; set; }
        //public int? groupPolicyId { get; set; }
        public string? fileName { get; set; }

        public int? totalRecords { get; set; }

        public int? importId { get; set; }
        public int? validation { get; set; }
        public int? proceed { get; set; }
        public string? typeOfEnrollment { get; set; }
        public int importExcelTempID { get; set; }
        public string member_Name { get; set; }
        //public string gender { get; set; }
        public string date_of_Birth { get; set; }
        public string mobile_Number { get; set; }
        public string email_ID { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        //public string nationality { get; set; }
        public string policy_Number { get; set; }
        //public DateTime? policyIssueDate { get; set; }
        //public DateTime? policyStartDate { get; set; }
        //public DateTime? policyEndDate { get; set; }
        public string marital_Status { get; set; }
        public int insurerID { get; set; }
        //public decimal? sumInsured { get; set; }
        // public string sumInsuredType { get; set; }
        public string planNo { get; set; }
        // public string passportNo { get; set; }
        public string premium { get; set; }
        public string product { get; set; }
        public string policyType { get; set; }
        //public string policyTenure { get; set; }
        //public string premiumCurrency { get; set; }
        public string pincode { get; set; }
        public string phoneNumber { get; set; }
        public string faxNo { get; set; }
        public string policy_Holder { get; set; }
        public string uw_Office_Name { get; set; }
        public string uw_Office_Code { get; set; }
        public string corporate_Name { get; set; }
        public string payee_Name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string family_Id { get; set; }
        public string member_Id { get; set; }
        public string employee_Code { get; set; }
        public string relation { get; set; }
        public string branch_Name { get; set; }
        public string unit_Name { get; set; }
        public string policy_Category { get; set; }
        //  public int groupPolicyId { get; set; }
        public string errorMessage { get; set; }
        public string enrollmentNo { get; set; }
        public DateTime? validFrom { get; set; }
        public DateTime? validTo { get; set; }
        public int? enrollmentId { get; set; }
        public int? insuranceCompanyId { get; set; }
        public string uWOfficeCode { get; set; }
        public string uWOfficeName { get; set; }
        public string policyNo { get; set; }
        public int? policyTypeId { get; set; }
        public string group_CorporateName { get; set; }
        public int? productId { get; set; }
        public int? planId { get; set; }
        public int? familyId { get; set; }
        public int? memberId { get; set; }
        public string policyTenure { get; set; }
        public DateTime? policyStartDate { get; set; }
        public DateTime? policyEndDate { get; set; }
        public decimal? policyPremium { get; set; }
        public string premiumCurrency { get; set; }
        public string insuredName { get; set; }
        public string policyHolder { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string gender { get; set; }
        public int? age { get; set; }
        public decimal? sumInsured { get; set; }
        public int? sIType { get; set; }
        public string primaryAddress1 { get; set; }
        public string primaryAddress2 { get; set; }
        public string primaryCity { get; set; }
        public int? primaryState { get; set; }
        public int? primaryCountry { get; set; }
        public int? primaryPin { get; set; }
        public string primaryContactPhone { get; set; }
        public string primaryContactMobile { get; set; }
        public string primaryContactFax { get; set; }
        public string primaryContactEmail { get; set; }
        public string passportNo { get; set; }
        public string nationality { get; set; }
        public string uploadedPolicyCopy { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? modifyDate { get; set; }
        public int? createByUserId { get; set; }
        public int? modifyByUserId { get; set; }
        public string cancelledCheque { get; set; }
        public int? intimationNo { get; set; }
        public string chequeInFavourOf { get; set; }
        public DateTime? policyIssueDate { get; set; }
        public string maritalStatus { get; set; }
        public string recStatus { get; set; }
        public DateTime? uploadedDate { get; set; }
        public string employeeCode { get; set; }
        public int? relationId { get; set; }
        public string remarks { get; set; }
        public int? groupPolicyId { get; set; }
        public string certificateNumber { get; set; }
        public string tripId { get; set; }
        public string tripType { get; set; }
        public int? tripDuration { get; set; }
        public bool? isCertificateEmailSent { get; set; }
        public bool? isEcardGenerated { get; set; }
        public int? branchId { get; set; }
        public bool? isWelcomeEmailSent { get; set; }
        public DateTime? dateOfJoining { get; set; }
        public int? subMemberId { get; set; }
        public string policyCategory { get; set; }
        public int? unitId { get; set; }
        public int? enrollmentLogId { get; set; }
        public string policyHolder1 { get; set; }
        public string insuredName1 { get; set; }
    }
}