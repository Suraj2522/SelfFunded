using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
    public class SettlementLetter
    { 
        public int? claimId {  get; set; }
        public string claimNo { get; set; }
        public string employeeCode { get; set; }
        public int insuranceCompanyId { get; set; }
        public string insuredName { get; set; }
        public string intimationNo { get; set; }
        public decimal amountPaid { get; set; }
        public string debitNoteNo { get; set; }
        public string policyNo { get; set; }
        public string paymentTo { get; set; }
        public string insuranceCompany { get; set; }
        public string contactNo { get; set; }
        public string contactAddress { get; set; }
        public string city { get; set; }
        public string stateName { get; set; }
        public string country { get; set; }
        public string placeOfLoss { get; set; }
        public decimal finalPayableAmountRoundOff { get; set; }
        public decimal netPayableUsdAmount { get; set; }
        public decimal standardDeductibleUsdAmount { get; set; }
        public decimal totalPayableUsdAmount { get; set; }
        public DateTime paymentDate { get; set; }
        public DateTime settlementDate { get; set; }
        public string finalCurrencyCode { get; set; }
        public string claimBenefit { get; set; }
        public string natureOfClaim { get; set; }
        public string paymentType { get; set; }
        public string chequeNeftNo { get; set; }
        public string accountNo { get; set; }
        public string bankName { get; set; }
        public string notes { get; set; }
    }
}
