using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;

namespace SelfFunded.Controllers
{
    public class PlanDetailsController : Controller
    {
        private readonly PlanDetailsDal _planDetailsDal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;
        public PlanDetailsController(PlanDetailsDal planDetailsDal, CommonDal common, IConfiguration configuration)
        {
            _planDetailsDal = planDetailsDal;
            commondal = common;
        }

       

        [Route("api/PlanDetails/InsertPlanDetails")]
        [HttpPost]
        public IActionResult InsertPlanDetails([FromBody]PlanDetails plandtls)
        {
            string msg = "";
            try
            {
                msg = _planDetailsDal.insertPlanDetails(plandtls);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertPlanDetails", "PlanDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/PlanDetails/UpdatePlanDetails/{planId}")]
        [HttpPut]
        public IActionResult UpdatePlanDetails(int planId, [FromBody] PlanDetails plandtls)
        {
            string msg = "";
            try
            {
                msg = _planDetailsDal.updatePlanDetails(planId, plandtls);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("UpdatePlanDetails", "PlanDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/PlanDetails/GetPlanDetails")]
        [HttpGet]
        public IActionResult GetPlanDetails()
        {
            try
            {
                List<PlanDetails> plandtls = new List<PlanDetails>();
                plandtls = _planDetailsDal.getPlanDetails();
                return Ok(plandtls);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetPlanDetails", "PlanDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
