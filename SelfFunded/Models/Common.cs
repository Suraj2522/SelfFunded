using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfFunded.Models
{
    public class Common
    {
      
    }
    public class CommonCountry
    {
        public int countryID { get; set; }
        public string countryName { get; set; }

    }
    public class CommonCity
    {
        public int cityID { get; set; }
        public string cityName { get; set; }
    }
    public class BindInsurance
    {
        public int insuranceCompanyId { get; set; }
        public string insuranceCompany{ get; set; }
    }

    public class CommonState
    {
        public int stateID { get; set; }
        public string stateName { get; set; }
    }
    public class BindIndustry
    {
        public int industryId { get; set; }
        public string industryName { get; set; }
    }
    public class BindLevel1
    {
        public string level1Id { get; set; }
        public string level1Master { get; set; }
        public string level1TransId { get; set; }
    }

    public class BindLevel2
    {
        public string level2Id { get; set; }
        
        public string level2Master { get; set; }
        public string level2TransId { get; set; }
    }

    public class BindLevel3
    {
        public string level3Code { get; set; }
        public string level3Master { get; set; }        
        public string level3TransId { get; set; }
    }

    public class BindStatus
    {
        public string status { get; set; }
        public int statusId { get; set; }
    }
    public class BindRelation
    {
        public string relation { get; set; }
        public int relationId { get; set; }        
    }

    public class BindEmployeetype
    {
        public int employeetypeid { get; set; }
        public string employeetype { get; set; }
    }

    public class BindIntimationType
    {
        public int intimationtypeid { get; set; }
        public string intimationtype { get; set; }
    }

    public class BindIntimationFrom
    {
        public int intimationfromid { get; set; }
        public string intimationfrom { get; set; }
    }

    public class BindClaimType
    {
        public int claimid { get; set; }
        public string claimtype { get; set; }
    }

    public class BindCaseType
    {
        public int caseid { get; set; }
        public string casetype { get; set; }
    }

    public class BindRequestType
    {
        public int reqtypeid { get; set; }
        public string requesttype { get; set; }
    }

    public class ErrorLog
    {
        //public void ErrorLogs(string methodNames, string controllerNames, string errorMessages, string dalNames)
        // {
        //     this.error = errorMessages;
        //     this.methodName = methodNames;   
        //     this.controllerName = controllerNames;
        //     this.dalName = dalNames;
        // }
        public string error { get; set; }
        public string methodName { get; set; }
        public string controllerName { get; set; }

        public string? dalName { get; set; }
    }

    public class BindLoginType
    {
        public int loginTypeId { get; set; }
        public string loginType { get; set; }
    }


}