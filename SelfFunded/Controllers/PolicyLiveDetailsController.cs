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
    public class PolicyLiveDetailsController : Controller
    {
        private readonly PolicyLiveDetailsDal _policylivedetailsDal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;
        public PolicyLiveDetailsController(PolicyLiveDetailsDal policylivedetailsDal, CommonDal common, IConfiguration configuration)
        {
            _policylivedetailsDal = policylivedetailsDal;
            commondal = common;
        }

       

        [Route("api/PolicyLiveDetails/InsertPolicyLiveDetails")]
        [HttpPost]
        public IActionResult InsertPolicyLiveDetails([FromBody]PolicyLiveDetails pollivedtls)
        {
            string msg = "";
            try
            {
                msg = _policylivedetailsDal.insertPolicyLiveDetails(pollivedtls);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertPolicyLiveDetails", "PolicyLiveDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/PolicyLiveDetails/UpdatePolicyLiveDetails/{policyid}")]
        [HttpPut]
        public IActionResult UpdatePolicyLiveDetails(int policyid,[FromBody] PolicyLiveDetails pollivedtls)
        {
            string msg = "";
            try
            {
                msg = _policylivedetailsDal.updatePolicyLiveDetails(policyid, pollivedtls);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("UpdatePolicyLiveDetails", "PolicyLiveDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/PolicyPemiumInf/GetAllPolicyLiveDetails")]
        [HttpGet]
        public IActionResult GetAllPolicyLiveDetails()
        {
            try
            {
                List<PolicyLiveDetails> pollivedtls = new List<PolicyLiveDetails>();
                pollivedtls = _policylivedetailsDal.getAllPolicyLiveDetails();
                return Ok(pollivedtls);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllPolicyLiveDetails", "PolicyLiveDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
