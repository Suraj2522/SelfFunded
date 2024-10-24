using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System;
using System.Collections.Generic;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SelfFunded.Controllers
{
    public class PolicyPemiumInfoController : Controller
    {
        private readonly PolicyPemiumInfoDal _policypremiuminfoDal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;
        public PolicyPemiumInfoController(PolicyPemiumInfoDal policypremiuminfoDal, CommonDal common, IConfiguration configuration)
        {
            _policypremiuminfoDal = policypremiuminfoDal;
            commondal = common;
        }

       

        [Route("api/PolicyPremiumInf/InsertPolicyPremiumInfo")]
        [HttpPost]
        public IActionResult InsertPolicyPremiumInfo([FromBody]PolicyPremiumInfo polpreminf)
        {
            string msg = "";
            try
            {
                msg = _policypremiuminfoDal.insertPolicyPremiumInfo(polpreminf);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertPolicyPremiumInfo", "PolicyPemiumInfoController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/PolicyPremiumInf/UpdatePolicyPremiumInfo/{pTId}")]
        [HttpPut]
        public IActionResult UpdatePolicyPremiumInfo(int pTId,[FromBody] PolicyPremiumInfo polpreminf)
        {
            string msg = "";
            try
            {
                msg = _policypremiuminfoDal.updatePolicyPremiumInfo(pTId, polpreminf);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                 commondal.LogError("UpdatePolicyPremiumInfo", "PolicyPemiumInfoController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/PolicyPremiumInf/GetAllPolicyPremiumInfo")]
        [HttpGet]
        public IActionResult GetAllPolicyPremiumInfo()
        {
            try
            {
                List<PolicyPremiumInfo> polpreminf = new List<PolicyPremiumInfo>();
                polpreminf = _policypremiuminfoDal.getAllPolicyPremiumInfo();
                return Ok(polpreminf);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllPolicyPremiumInfo", "PolicyPemiumInfoController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
