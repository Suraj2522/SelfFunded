using System;
using Microsoft.AspNetCore.Http;

namespace SelfFunded.Models
{
    public class UserMaster
    {
        public int? srNo { get; set; }
        public int? userId { get; set; }
        public int? userCode { get; set; }
        public string? userPassword { get; set; }
        public string? userType { get; set; }
        public string? status { get; set; }
        public string? corporateName { get; set; }
        public string? employeeCode { get; set; }
        public string? firstName { get; set; }
        public string? middleName { get; set; }
        public string? lastName { get; set; }
        public DateTime? userCreatedOn { get; set; }
        public DateTime? firstLogin { get; set; }
        public DateTime? lastLogin { get; set; }
        public DateTime? lastPasswordChangedOn { get; set; }
        public int? userDepartment { get; set; }
        public string? branchCode { get; set; }
        public DateTime? userDeactivatedOn { get; set; }
        public string? primaryContactNo { get; set; }
        public string? secondaryContactNo { get; set; }
        public string? userEmailId { get; set; }
        public int? userTypeId { get; set; }
        public int? groupId { get; set; }
        public string? recStatus { get; set; } = "A";
        public int? isDelete { get; set; } = 0;
        public int? statusId { get; set; }
        public int? corporateId { get; set; }
        public string? logoName { get; set; }
        public int? loginTypeId { get; set; }
        public int? createByUserId { get; set; }
        public int? modifyByUserId { get; set; }
        public DateTime? modifyDate { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? deletedDate { get; set; }
        public string? userSignature { get; set; }
        public string? loginFrom { get; set; }
        public string? claimStatus { get; set; }
        public string? userName{ get; set; }
        public string? createdBy { get; set; }

        public string? providerName { get; set; }
        public string? fullName { get; set; }
        public string? defaultPassword { get; set; }
        public int? enrollmentId { get; set; }
        public int? payerId { get; set; }
        public string? insuranceIDs { get; set; }

        // Additional fields from your previous model
      
        public IFormFile file { get; set; }
        public IFormFile signature { get; set; }
        public int? roleId { get; set; }
        public string? roleType { get; set; }

    }
}
