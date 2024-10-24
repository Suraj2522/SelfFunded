using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfFunded.Models
{
    public class CorporateMaster
    {
        public int? srNo { get; set; } 
        public IFormFile corporateLogo { get; set; }
        public int? corporateId { get; set; }
        public string? industry { get; set; }
        public int? industryId { get; set; }
        public int? insuranceCompanyId { get; set; }

        public string? insuranceName { get; set; }
        public string? corporateName { get; set; }
        public string? localName { get; set; }
        public string? alias { get; set; }
        public int? numberOfEmployees { get; set; }
        public string? emailID { get; set; }
        public string? pinCode { get; set; }
        public string? businessEmailID { get; set; }
        public string? contactNo { get; set; } // Assuming it's stored as a string? to preserve leading zeroes
        public string? officePhone { get; set; }
        public string? fax { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? country { get; set; }
        public int? countryID { get; set; }
        public int? modifyByUserId { get; set; }
        public string? city { get; set; }
        public int? cityID { get; set; }
        public string? state{ get; set; }
        public int? stateID { get; set; }
        public string? website { get; set; }
        public string? feeType { get; set; } // Assuming it's stored as a string? to match "Percentage" or "Amount"
        public string? tPAFees { get; set; }
        public int? createByUserId { get; set; }
        public string? recStatus { get; set; }
        public string? isSelfFunded { get; set; }


        public string? corporateLogoName { get; set; }
        public int? isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }
        public DateTime? modifyDate { get; set; }

        public int? isActive { get; set; }
       

    }
}