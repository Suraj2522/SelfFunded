using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class DebitSummary
    {
        //        public int totalClaims { get; set; }
        //        public decimal claimAmtPaid { get; set; }
        //        public string debitNoteNo { get; set; }
        //        public int debitId { get; set; }
        //        public int visibleDelete { get; set; }
        //        public DateTime invoiceDate { get; set; }
        //        public int? insuranceCompanyId { get; set; }
        //        public string insuredName { get; set; }
        //        public string claimNo { get; set; }
        //        public string fromDate { get; set; }
        //        public string toDate { get; set; }
        //        public string insuranceCompany { get; set; }
        //        public int numberOfClaims { get; set; }
        //        public decimal amount { get; set; }
        //        public string claimType { get; set; }
        //        public string planName { get; set; }
        //        public string address1 { get; set; }
        //        public string cityName { get; set; }
        //        public string stateName { get; set; }
        //        public string pinCode { get; set; }
        //        public string concernedPerson { get; set; }
        //        public string panNo { get; set; }
        //        public string gstNo { get; set; }
        //        public string bankName { get; set; }
        //        public string accountNo { get; set; }
        //        public string accountName { get; set; }
        //        public string branchAddress { get; set; }
        //        public string ifscCode { get; set; }
        //        public string micrCode { get; set; }
        //        public string swiftCode { get; set; }
        //        public string remitanceBankName { get; set; }
        //        public string remitanceBranchName { get; set; }
        //        public string remitanceAccountNo { get; set; }
        //        public string remitanceSwiftCode { get; set; }
        //        public string branchName { get; set; }
        //        public string monthYear { get; set; }
        public int? userId { get; set; }
        public int? debitTransactionId { get; set; }
        public int? claimId { get; set; }
        public int? totalClaims { get; set; }
        public decimal claimAmtPaid { get; set; }
        public string debitNoteNo { get; set; }
        public int debitId { get; set; }
        public int visibleDelete { get; set; }
        public DateTime invoiceDate { get; set; }
        public int? insuranceCompanyId { get; set; }
        public string insuredName { get; set; }
        public string claimNo { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string insuranceCompany { get; set; }
        public int numberOfClaims { get; set; }
        public decimal amount { get; set; }
        public string claimType { get; set; }
        public string planName { get; set; }
        public string address1 { get; set; }
        public string cityName { get; set; }
        public string stateName { get; set; }
        public string pinCode { get; set; }
        public string concernedPerson { get; set; }
        public string panNo { get; set; }
        public string gstNo { get; set; }
        public string bankName { get; set; }
        public string accountNo { get; set; }
        public string accountName { get; set; }
        public string branchAddress { get; set; }
        public string ifscCode { get; set; }
        public string micrCode { get; set; }
        public string swiftCode { get; set; }
        public string remitanceBankName { get; set; }
        public string remitanceBranchName { get; set; }
        public string remitanceAccountNo { get; set; }
        public string remitanceSwiftCode { get; set; }
        public string branchName { get; set; }
        public string monthYear { get; set; }

        public int srNo { get; set; }
        public string billingMonth { get; set; }
        public string groupPolicyNo { get; set; }
        public string entity { get; set; }
        public string policyNo { get; set; }
        public string empId { get; set; }
        public string hospitalName { get; set; }
        public DateTime admissionDate { get; set; }
        public DateTime dischargeDate { get; set; }
        public string complaints { get; set; }
        public string treatment { get; set; }
        public decimal grossAmount { get; set; }
        public decimal rejectedAmount { get; set; }
        public decimal netAmount { get; set; }
        public string status { get; set; }
        public string ipdOrOpd { get; set; }
        public string debitNo { get; set; }
    }
}
