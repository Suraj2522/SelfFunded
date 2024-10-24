using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;

namespace SelfFunded.Controllers
{
    public class BenefitConfigController : Controller
    {        
        private readonly BenefitConfigDal _benefitConfigDal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;
        public BenefitConfigController(BenefitConfigDal benefitConfigDal, CommonDal common, IConfiguration configuration)
        {
            _benefitConfigDal = benefitConfigDal;
            commondal = common;
        }

      

        [Route("api/BenefitConfig/InsertBenefitConfig")]
        [HttpPost]
        public IActionResult InsertBenefitConfig([FromBody]List<BenefitConfig> benconfigs)
        {
            try
            {
                foreach (var benconfig in benconfigs)
                {
                    string msg = _benefitConfigDal.InsertBenefitConfig(benconfig);
                    if (msg != "Data saved successfully")
                    {
                        // Log individual errors and return an error response if needed
                        commondal.LogError("InsertBenefitConfig", "BenefitConfigController", msg, "");
                        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
                    }
                }

                // If all records were inserted successfully, return success response
                return Ok(new { message = "Data saved successfully" });
            }
            catch (Exception ex)
            {
                // Log and return a generic error response if an exception occurs
                commondal.LogError("InsertBenefitConfig", "BenefitConfigController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

    }
}

