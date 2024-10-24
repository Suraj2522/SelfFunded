using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;

namespace SelfFunded.Controllers
{
    public class CoinsuranceDetailsController : Controller
    {
        private readonly CoinsuranceDetailsDal _coinsuranceDetailsDal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;
        public CoinsuranceDetailsController(CoinsuranceDetailsDal coinsuranceDetailsDal, CommonDal common, IConfiguration configuration)
        {
            _coinsuranceDetailsDal = coinsuranceDetailsDal;
            commondal = common;
        }

       

        [Route("api/CoinsuranceDetails/InsertCoinsuranceDetails")]
        [HttpPost]
        public IActionResult InsertPlanDetails([FromBody]Coinsurance coins)
        {
            string msg = "";
            try
            {
                msg = _coinsuranceDetailsDal.insertCoinsuranceDetails(coins);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertCoinsuranceDetailsetails", "CoinsuranceDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/CoinsuranceDetails/UpdateCoinsuranceDetails/{coinsuranceId}")]
        [HttpPut]
        public IActionResult UpdateCoinsuranceDetails(int coinsuranceId, [FromBody]Coinsurance coins)
        {
            string msg = "";
            try
            {
                msg = _coinsuranceDetailsDal.updateCoinsuranceDetails(coinsuranceId, coins);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("UpdateCoinsuranceDetailsetails", "CoinsuranceDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/CoinsuranceDetails/GetCoinsuranceDetails")]
        [HttpGet]
        public IActionResult GetCoinsuranceDetails()
        {
            try
            {
                List<Coinsurance> plandtls = new List<Coinsurance>();
                plandtls = _coinsuranceDetailsDal.getCoinsuranceDetails();
                return Ok(plandtls);
            }
            catch (Exception ex)
            {
                    commondal.LogError("GetCoinsuranceDetailsetails", "CoinsuranceDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
