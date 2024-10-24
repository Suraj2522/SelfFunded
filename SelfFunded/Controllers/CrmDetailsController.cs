using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;

namespace SelfFunded.Controllers
{
    public class CrmDetailsController : Controller
    {
        private readonly CrmDetailsDal _crmDetailsDal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;
        public CrmDetailsController(CrmDetailsDal crmDetailsDal, CommonDal common, IConfiguration configuration)
        {
            _crmDetailsDal = crmDetailsDal;
            commondal = common;
        }

       

        [Route("api/CrmDetails/InsertCrmDetails")]
        [HttpPost]
        public IActionResult InsertCrmDetails([FromBody] CrmDetails crmdtls)
        {
            string msg = "";
            try
            {
                msg = _crmDetailsDal.insertCrmDetails(crmdtls);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertCrmDetails", "CrmDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/CrmDetails/UpdateCrmDetails/{policyId}")]
        [HttpPut]
        public IActionResult UpdateCrmDetails(int policyId,[FromBody] CrmDetails crmdtls)
        {
            string msg = "";
            try
            {
                msg = _crmDetailsDal.updateCrmDetails(policyId, crmdtls);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("UpdateCrmDetails", "CrmDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/CrmDetails/GetCrmDetails")]
        [HttpGet]
        public IActionResult GetCrmDetails()
        {
            try
            {
                List<CrmDetails> crmdtls = new List<CrmDetails>();
                crmdtls = _crmDetailsDal.getCrmDetails();
                return Ok(crmdtls);
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertCrmDetails", "CrmDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
