using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class HospitalClaimDetails
    {
        public int insuranceId { get; set; }
        public string claimNumber { get; set; }
        public string insuredName { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string orderByCol { get; set; }
        public int corporateId { get; set; }
        public string claimType { get; set; }
        public string billNo { get; set; }
        public string name { get; set; }
        public string fir { get; set; }
        public decimal amountPaid { get; set; }
        public string bankChequeNo { get; set; }
        public DateTime dateOfPaymentHosp { get; set; }
        public string nameOfHospital { get; set; }
        public decimal amtPaidMem { get; set; }
        public string chequeNoMem { get; set; }
        public DateTime dateOfPymtMem { get; set; }
        public string reasonsForDeduction { get; set; }
        public string employeeName { get; set; }
        public string chequeInTheNameOf { get; set; }
        public string bankAcNumber { get; set; }
        public string nameOfInsuranceCo { get; set; }
        public string floatAcNo { get; set; }
        public string chequeDispatchLocation { get; set; }
        public string addressMem { get; set; }
        public string hospitalAddress { get; set; }
        public decimal finalBillAmt { get; set; }
        public decimal deductions { get; set; }
        public decimal claimLodged { get; set; }
        public decimal amtPaidHosp { get; set; }
        public decimal amtPaidInsured { get; set; }
        public string policyNumber { get; set; }
        public string city { get; set; }
        public string groupName { get; set; }
        public string benfBankAccNo { get; set; }
        public string? bankName { get; set; }
        public string ifscCode { get; set; }
        public string pymtMethod { get; set; }
        public string accountType { get; set; }
        public decimal tdsAmt { get; set; }
        public string emailId { get; set; }
        public string emailId1 { get; set; }
        public string emailId2 { get; set; }
        public string emailId3 { get; set; }
        public string emailId4 { get; set; }
        public string groupCode { get; set; }
        public string phm { get; set; }
        public string floatAcNoMem { get; set; }
        public string clLodgementNo { get; set; }
        public string insCcnNo { get; set; }
        public string lotNo { get; set; }
        public string assignee { get; set; }
        public decimal chqNoAmtRecdFrmCorp { get; set; }
        public DateTime dtOfPymtRecdFrmCorporate { get; set; }
        public DateTime dtOfClRecd { get; set; }
        public string branchFir { get; set; }
        public string branchFirExt { get; set; }
        public string accountNotes { get; set; }
        public string underWritterOffCd { get; set; }
        public DateTime dtOfSettlement { get; set; }
        public string bankAccNoMem { get; set; }
        public string bankNameMem { get; set; }
        public string providerNo { get; set; }
        public DateTime cheqReissuedDt { get; set; }
        public DateTime dtOfPayment { get; set; }
        public string employeeNo { get; set; }
        public string plantDept { get; set; }
        public string firExt { get; set; }
        public string partialPymntSeq { get; set; }
        public int sNo { get; set; }
        public decimal taxRateBeforeLimit { get; set; }
        public decimal taxRateAfterLimit { get; set; }
        public decimal netAmountBeforeLimit { get; set; }
        public decimal netAmountAfterLimit { get; set; }
        public decimal tdsValueBeforeLimit { get; set; }
        public decimal tdsValueAfterLimit { get; set; }
        public decimal netAmountBeforeLimitWithTds { get; set; }
        public decimal netAmountAfterLimitWithTds { get; set; }     
        public int claimId { get; set; }
        public int userId { get; set; }
    }
}
