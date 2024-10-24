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
    public class PolicyController : Controller
    {
        private readonly PolicyDal _policyDal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;

        public PolicyController(PolicyDal policyDal, CommonDal common, IConfiguration configuration)
        {
            _policyDal = policyDal;
            commondal = common;
        }

      

        [Route("api/Policy/InsertPolicyMaster")]
        [HttpPost]
        public IActionResult InsertPolicyMaster([FromBody] PolicyMaster policy)
        {
            string msg = "";
            try
            {
                msg = _policyDal.insertPolicy(policy);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertPolicyMaster", "PolicyController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/Policy/UpdatePolicyMaster")]
        [HttpPut]
        public IActionResult UpdatePolicyMaster(int id,[FromBody]PolicyMaster policy)
        {
            string msg = "";
            try
            {               
                msg = _policyDal.updatePolicy(id,policy);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("UpdatePolicyMaster", "PolicyController", ex.Message,"");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/Policy/GetAllPolicyDetails")]
        [HttpGet]
        public IActionResult GetAllPolicy()
        {
            try
            {
                List<PolicyMaster> policy = new List<PolicyMaster>();
                policy = _policyDal.getAllPolicyDetails();
                return Ok(policy);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllPolicyDetails", "PolicyController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");

            }
        }

        [Route("api/Policy/deletePolicy")]
        [HttpPut]
        public IActionResult DeletePolicy(int id, [FromBody] PolicyMaster item)
        {
            try
            {
                string msg = "";
                {
                    msg = _policyDal.deletePolicy(item);
                }
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                    commondal.LogError("DeletePolicy", "PolicyController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");


            }
        }
    }
}
