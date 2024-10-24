using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace SelfFunded.Models
{
   
public class BrokerMaster

    {
        public int srNo { get; set; }

        public int brokerId { get; set; }

        public string brokerName { get; set; }

        public string emailId { get; set; }

        public string businessEmailId { get; set; }

        public string mobileNo { get; set; }

        public string officePhone { get; set; }

        public string fax { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string country { get; set; }

        public string city { get; set; }

        public string website { get; set; }
        public IFormFile file { get; set; }

        public string brokerLogo { get; set; }

        public string createdBy { get; set; }

        public DateTime? createdDate { get; set; }

        public int isDeleted { get; set; }

        public DateTime? deletedDate { get; set; }

        public int isActive { get; set; }
       // public int countryId { get; set; }
        public string countryName { get; set; }
        public int cityID { get; set; }
        public int countryID { get; set; }
        public int stateID { get; set; }
        public string cityName { get; set; }
        public string state{ get; set; }
        
           //  public string createdBy { get; set; }
    }
}