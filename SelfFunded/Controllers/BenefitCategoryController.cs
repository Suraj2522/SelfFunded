using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data.SqlClient;
using static SelfFunded.Models.BenefitCategory;

namespace SelfFunded.Controllers
{
    public class BenefitCategoryController : Controller
    {
        private readonly BenefitCategoryDal _benefitCategoryDal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;

        public BenefitCategoryController(BenefitCategoryDal benefitCategoryDal, CommonDal common, IConfiguration configuration)
        {
            _benefitCategoryDal = benefitCategoryDal;
            commondal = common;
        }

        [Route("api/BenefitCategory/GetBenefitCategory")]
        [HttpGet]
        public IActionResult getBenefitCategory()
        {
            List<BindBenefitCategory> benfitcat = new List<BindBenefitCategory>();
         
            try
            {
                benfitcat = _benefitCategoryDal.getBenefitCategory();
                return Ok(benfitcat);
            }
            catch (Exception ex)
            {
                commondal.LogError("getBenefitCategory", "BenefitCategoryController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
           
        }
    }
}
