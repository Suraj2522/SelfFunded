using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System;

namespace SelfFunded.Controllers
{
    public class CommonController : Controller
    {
        private readonly CommonDal codal;

        public CommonController(CommonDal commondal)
        {
            codal = commondal;
        }

        [Route("api/Common/GetCity")]
        [HttpGet]
        public IActionResult GetCity(int stateID)
        {
            try
            {
                var cities = codal.getCity(stateID);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetState")]
        [HttpGet]
        public IActionResult GetState(int countryID)
        {
            try
            {
                var states = codal.getState(countryID);
                return Ok(states);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetCountry")]
        [HttpGet]
        public IActionResult GetCountry()
        {
            try
            {
                var countries = codal.getCountry();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetInsuranceName")]
        [HttpGet]
        public IActionResult GetInsuranceName()
        {
            try
            {
                var insuranceNames = codal.getInsuranceName();
                return Ok(insuranceNames);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetCorporateName")]
        [HttpGet]
        public IActionResult GetCorporateName()
        {
            try
            {
                var corporates = codal.getCorporate();
                return Ok(corporates);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetIndustry")]
        [HttpGet]
        public IActionResult GetIndustry()
        {
            try
            {
                var industries = codal.getIndustry();
                return Ok(industries);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetLevel1")]
        [HttpGet]
        public IActionResult GetLevel1(int id)
        {
            try
            {
                var level1Items = codal.getLevel1(id);
                return Ok(level1Items);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetLevel2")]
        [HttpGet]
        public IActionResult GetLevel2([FromQuery] int benefitId, [FromQuery] int level1id)
        {
            try
            {
                var level2Items = codal.getLevel2(benefitId, level1id);
                return Ok(level2Items);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetLevel3")]
        [HttpGet]
        public IActionResult GetLevel3([FromQuery] int benefitId, [FromQuery] int level1id, [FromQuery] int level2id)
        {
            try
            {
                var level3Items = codal.getLevel3(benefitId, level1id, level2id);
                return Ok(level3Items);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetStatus")]
        [HttpGet]
        public IActionResult GetStatus()
        {
            try
            {
                var statusList = codal.getstatus();
                return Ok(statusList);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetRelation")]
        [HttpGet]
        public IActionResult GetRelation()
        {
            try
            {
                var relationList = codal.getRealtion();
                return Ok(relationList);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetEmployeeType")]
        [HttpGet]
        public IActionResult GetEmployeeType()
        {
            try
            {
                var employeeTypes = codal.getEmployeeType();
                return Ok(employeeTypes);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetIntimationType")]
        [HttpGet]
        public IActionResult GetIntimationType()
        {
            try
            {
                var intimationTypes = codal.getIntimationType();
                return Ok(intimationTypes);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetIntimationFrom")]
        [HttpGet]
        public IActionResult GetIntimationFrom()
        {
            try
            {
                var intimationFroms = codal.getIntimationFrom();
                return Ok(intimationFroms);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetClaimType")]
        [HttpGet]
        public IActionResult GetClaimType()
        {
            try
            {
                var claimTypes = codal.getClaimType();
                return Ok(claimTypes);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetCaseType")]
        [HttpGet]
        public IActionResult GetCaseType()
        {
            try
            {
                var caseTypes = codal.getCaseType();
                return Ok(caseTypes);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetRequestType")]
        [HttpGet]
        public IActionResult GetRequestType()
        {
            try
            {
                var requestTypes = codal.getRequestType();
                return Ok(requestTypes);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetIntimationDetailsForClaim")]
        [HttpGet]
        public IActionResult GetIntimationDetailsForClaim()
        {
            try
            {
                var clmdtls = codal.getIntimationDetailsForClaim();
                return Ok(clmdtls);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Common/GetLoginType")]
        [HttpGet]
        public IActionResult GetLoginType()
        {
            try
            {
                var loginTypes = codal.getGetLoginType();
                return Ok(loginTypes);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

    }
}
