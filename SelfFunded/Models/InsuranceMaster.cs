using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfFunded.Models
{
    public class InsuranceMaster
    {
        public int srNo { get; set; }
        public int insuranceCompanyId { get; set; }
        public string insuranceCompany { get; set; }
        public string insuranceCompanyCode { get; set; }

         public decimal? reinsurerGICLiability { get; set; }
        public string localName { get; set; }
        public string? alias { get; set; }
        public string registrationNo { get; set; }
        public DateTime? registrationDate { get; set; }
        public string emailId { get; set; }
        public string businessEmailId { get; set; }
        public string contactNo { get; set; }
        public string officePhone { get; set; }
        public string fax { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string country { get; set; }

        public int countryID { get; set; }
        public string city { get; set; }
        public int cityID { get; set; }
        public string state { get; set; }
        public int stateID { get; set; }
        public string website { get; set; }
        public IFormFile file { get; set; }
        public string insuranceLogo { get; set; }
        public string createdByUserId { get; set; }
        public DateTime? createDate { get; set; }
        public int isDeleted { get; set; }
        public DateTime? deletedDate { get; set; }


        public string irdaInsCode { get; set; }
        public string inboundOutbound { get; set; }

      
       
      
        public int? pinCode { get; set; }


        public string concernedPerson { get; set; }
      //  public decimal? reinsurerGicLiability { get; set; }
        public decimal? insuranceLiability { get; set; }
        public bool? isPayee { get; set; }
        public int? accountId { get; set; }
        public int? currencyId { get; set; }
        public decimal? minAmount { get; set; }
        public decimal? maxAmount { get; set; }
        public char recStatus { get; set; } = 'A';
        public DateTime? modifyDate { get; set; }
        public int? createByUserId { get; set; }
        public int? modifyByUserId { get; set; }
        public string notes { get; set; }
        public string panNo { get; set; }
        public string gstNo { get; set; }
        public bool? isSmsFacility { get; set; }
        public bool? isEmailFacility { get; set; }
        public int? isHeadOffice { get; set; }
        public int? headOfficeId { get; set; }
        public bool? isAutoGenerateMemberId { get; set; }
        public string unit { get; set; }
        public string location { get; set; }
        public int? providerNo { get; set; }
    }
}