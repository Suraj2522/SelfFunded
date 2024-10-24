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
    public class ImpPolicyInfoController : Controller
    {
        private readonly ImpPolicyInfoDal _impPolicyInfoDal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;
        public ImpPolicyInfoController(ImpPolicyInfoDal impPolicyInfoDal, CommonDal common, IConfiguration configuration)
        {
            _impPolicyInfoDal = impPolicyInfoDal;
            commondal = common;
        }

       

        [Route("api/ImpPolicyInfo/InsertImpPolicyInfo")]
        [HttpPost]
        public IActionResult InsertImpPolicyInfo([FromBody]ImpPolicyInfo impPolinfo)
        {
            string msg = "";
            try
            {
                msg = _impPolicyInfoDal.insertImpPolicyInfo(impPolinfo);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertImpPolicyInfo", "ImpPolicyInfoController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/ImpPolicyInfo/UpdateImpPolicyInfo/{ID}")]
        [HttpPut]
        public IActionResult UpdateImpPolicyInfo(int ID,[FromBody] ImpPolicyInfo impPolinfo)
        {
            string msg = "";
            try
            {
                msg = _impPolicyInfoDal.updateImpPolicyInfo(ID, impPolinfo);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("UpdateImpPolicyInfo", "ImpPolicyInfoController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/ImpPolicyInfo/GetImpPolicyInfo")]
        [HttpGet]
        public IActionResult GetImpPolicyInfo()
        {
            try
            {
                List<ImpPolicyInfo> impPolinfo = new List<ImpPolicyInfo>();
                impPolinfo = _impPolicyInfoDal.getImpPolicyInfo();
                return Ok(impPolinfo);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetImpPolicyInfo", "ImpPolicyInfoController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
