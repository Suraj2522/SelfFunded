using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Runtime.InteropServices;
namespace SelfFunded.Models
{
    public class IntimationSheetInbound
    {

        
        public string? insuranceCompany { get; set; }
        
        public string? claimType { get; set; }
        public decimal? netAmount { get; set; }
        public string? gender { get; set; }

        public int? srNo { get;set; }
        public string? preAuthNo { get; set; }
   
        public long? intimationId { get; set; }
        public string? intimationNo { get; set; }
        public DateTime? dateOfIntimation { get; set; }
        public string timeOfIntimation { get; set; }
        public int? insuranceCompanyId { get; set; }
        public string? policyNo { get; set; }
        public string insuredName { get; set; }
        public string intimationType { get; set; }
        public string personEmailId { get; set; }
        public string diagnosis { get; set; }
        public DateTime? treatmentDate { get; set; }

        public string personContactNo { get; set; }
        public string intimationFrom { get; set; }
        public DateTime? dateOfLoss { get; set; }
        public string intimationFromEmail { get; set; }
        public string intimationFromContactNo { get; set; }
        public string natureOfIllness { get; set; }
        public string hospitalName { get; set; }
        public string hospitalAddress1 { get; set; }
        public string hospitalContactNo { get; set; }
        public int? claimTypeId { get; set; }
        public int? claimId { get; set; }
        public DateTime? createDate { get; set; }
    
        public DateTime? policyEndDate { get; set; }
        public DateTime? policyStartDate { get; set; }
        public int? createByUserId { get; set; }
        public int? modifyByUserId { get; set; }
        public string recStatus { get; set; }
        public string? caseType { get; set; }
        public string? insuranceIDs { get; set; }
        public string? loginTypeId { get; set; }
        public string? orderByCol{ get; set; }
        public int? boundType { get; set; }
        public string treatment { get; set; }
        public string remarks { get; set; }
        public int? noteTypeId { get; set; }
        public string requestType { get; set; }
        public int? claimStatusId { get; set; }
        public int? stageId { get; set; }
        public int? srNos { get; set; }
        public string uniqueId { get; set; }
        public string? primaryMember { get; set; }
        public int? relationId { get; set; }
        public int? employeeTypeId { get; set; }
        public string? contactNo { get; set; }
        public string alternateContactNo { get; set; }
        public string intimationRemarks { get; set; }
        public long? preAuthId { get; set; }
        public string phsFirNo { get; set; }
        public string isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }

        public string? fromDate { get; set; }
        public string? toDate { get; set; }
    }
}
