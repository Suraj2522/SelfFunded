﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfFunded.Models
{
    public class Claims
    {
        public IntimationSheetInbound intimationSheetInbound { get; set; }
        public long claimId { get; set; }
        
         public int? srNo { get; set; }
        public long? preAuthID { get; set; }
        public string insuredName { get; set; }
        public string gender { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string age { get; set; }
        public string policyNo { get; set; }
        public string contactNo { get; set; }
        public string cardNo { get; set; }
        public bool? networkType { get; set; }
        public int? providerId { get; set; }
        public int? roomTypeId { get; set; }
        public string doctorName { get; set; }
        public string complaints { get; set; }
        public string diagnosis { get; set; }
        public string treatment { get; set; }
        public decimal? lengthOfStay { get; set; }
        public DateTime? treatmentDate { get; set; }
        public int? finalPayableCurrencyId { get; set; }
        public int? createByUserId { get; set; }
        public DateTime? createDate { get; set; }
        public int? modifyByUserId { get; set; }
        public DateTime? modifyDate { get; set; }
        public string recStatus { get; set; }
        public int? insuranceCompanyId { get; set; }
        public int? ailmentId { get; set; }
        public string intimationNo { get; set; }
        public decimal? grossAmount { get; set; }
        public decimal? approvedAmount { get; set; }
        public decimal? rejectedAmount { get; set; }
        public decimal? netAmount { get; set; }
        public string? claimStatus { get; set; }
        public string remarks { get; set; }
        public string certificateNo { get; set; }
        public string coPayType { get; set; }
        public decimal? coPayValue { get; set; }
        public decimal? coInsurance { get; set; }
        public string? claimNo { get; set; }
        public bool? isPaid { get; set; }
        public int? paymentTo { get; set; }

      // public int? SRNO { get; set; }
        public int? paymentTypeID { get; set; }
        public string cheque_Neft_No { get; set; }
        public DateTime? cheque_Neft_Date { get; set; }
        
        public DateTime? claimProcessDate { get; set; }
        public decimal? cheque_Neft_Amt { get; set; }
        public string accountNo { get; set; }
        public string ifscCode { get; set; }
        public string bankName { get; set; }
        public decimal? netAmountINR { get; set; }
        public decimal? netAmountUSD { get; set; }
        public decimal? phmFeesINR { get; set; }
        public decimal? phmFeesUSD { get; set; }
        public int? nationality { get; set; }
        public string prefix { get; set; }
        public int? stageId { get; set; }
        public string caseType { get; set; }
        public DateTime? dischargeDate { get; set; }
        public int? providerNo { get; set; }
        public string providerName { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string stateCode { get; set; }
        public string stateName { get; set; }
        public string cityCode { get; set; }
        public string cityName { get; set; }
        public int? providerAmountCurrency { get; set; }
        public int? phmFeesCurrency { get; set; }
        public DateTime? debitDate { get; set; }
        public string debitNo { get; set; }
        public int? debitID { get; set; }
        public string originalClaimNo { get; set; }
        public string providerAddress1 { get; set; }
        public string providerAddress2 { get; set; }
        public string databaseType { get; set; }
        public string hospitalName { get; set; }
        public string providerCategoryType { get; set; }
        public string providerAddressArea { get; set; }
        public string providerPincode { get; set; }
        public string providerAreaCode { get; set; }
        public string providerContactNo { get; set; }
        public string providerFaxNo { get; set; }
        public string providerEmailId { get; set; }
        public string providerWebsite { get; set; }
        public string oldProviderNO { get; set; }
        public string oldProviderName { get; set; }
        public bool? tdsFlag { get; set; }
        public int? overseasId { get; set; }
        public decimal? charges { get; set; }
        public decimal? hospitalAmount { get; set; }
        public int? hospitalCurrency { get; set; }
        public string previousClaimNo { get; set; }
        public int? previousClaimId { get; set; }
        public int? assistantFeesCurrencyId { get; set; }
        public bool? isPhmFeesApplicable { get; set; }
        public string panNo { get; set; }
        public string? claimType { get; set; }
        public int? intimationOPDId { get; set; }
        public decimal? deductionAmount { get; set; }
        public decimal? paidByInsured { get; set; }
        public decimal? negotiatedAmount { get; set; }
        public decimal? discountAmount { get; set; }
        public decimal? amountWithoutDeduction { get; set; }
        public decimal? minAmountInUSD { get; set; }
        public int? sourceCurrencyId { get; set; }
        public decimal? usdRatePerOneINR { get; set; }
        public decimal? inrRatePerOneUSD { get; set; }
        public decimal? totalBillAmountInUSD { get; set; }
        public decimal? feesPercentage { get; set; }
        public DateTime? invoiceDate { get; set; }
        public DateTime? claimDate { get; set; }
        public int? claimById { get; set; }
        public int? dependentCode { get; set; }
        public DateTime? savingsDownloadedDate { get; set; }
        public int? savingsDownloadedBy { get; set; }
        public decimal? revisedAmount { get; set; }
        public int? payerId { get; set; }
        public string payerName { get; set; }
        public string uniqueId { get; set; }
       // public DateTime? claimProcessDate { get; set; }
        public int? intimationId { get; set; }
        public int? claimBenefitId { get; set; }
        public int? productId { get; set; }
        public int? planId { get; set; }
        public string paymentToReim { get; set; }
        public string logNumber { get; set; }
        public string memberEmailId { get; set; }
        public DateTime? caseReceiptDate { get; set; }
        public DateTime? emailRespondDate { get; set; }
        public string primaryMember { get; set; }
        public int? relationId { get; set; }
        public int? employeeTypeId { get; set; }
        public string alternateContactNo { get; set; }
        public string auditorRemarks { get; set; }
        public string requestType { get; set; }
        public string requestTypeOther { get; set; }
        public string hospitalVerification { get; set; }
        public DateTime? dateOfDeath { get; set; }
        public string causeOfDeath { get; set; }
        public string employeeCode { get; set; }
        public int? memberId { get; set; }
        public int? enrollmentId { get; set; }
        public bool? overrideAlert { get; set; }
        public bool? isSettlementEmailSent { get; set; }
        public int? groupPolicyId { get; set; }
        public int? phmCommentsId { get; set; }
        public string phmCommentsOther { get; set; }
        public DateTime? documentReceivedDate { get; set; }
        public int? branchId { get; set; }
        public string phsFIRNo { get; set; }
        public string hospitalType { get; set; }
        public decimal? tariffDeduction { get; set; }
        public int? hospitalTypeId { get; set; }
        public int? claimLogId { get; set; }
        public decimal? rejectedAmount_Copy { get; set; }
        public string isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public int? loginTypeId { get; set; }
        public string? insuranceType { get; set; }
        public string? insuranceIDs { get; set; }
    }
}