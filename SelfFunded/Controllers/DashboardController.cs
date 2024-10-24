using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace SelfFunded.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardDal _dashboardDAL;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly CommonDal codal;


        public DashboardController(IConfiguration configuration, CommonDal common)
        {
            _dashboardDAL = new DashboardDal(configuration, common);
            commondal = common;
        }

        [Route("api/Dashboard/GetClaimCountByStatus")]
        [HttpGet]
        public IActionResult GetClaimCountByStatus()
        {
            try
            {
                var status = _dashboardDAL.getClaimCountByStatus();
                return Ok(status);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetClaimCountByStatus", "DashboardController", ex.Message, null);

                return BadRequest("Error occurred: " + ex.Message);
            }
        }
        [Route("api/Dashboard/GetStatusCountByClaimType")]
        [HttpGet]
        public IActionResult GetStatusCountByClaimType()
        {
            try
            {
                var clmtype = _dashboardDAL.getStatusCountByClaimType();
                return Ok(clmtype);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetStatusCountByClaimType", "DashboardController", ex.Message, null);
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Dashboard/GetClaimCountByRecStatus")]
        [HttpGet]
        public IActionResult GetClaimCountByRecStatus()
        {
            try
            {
                var recstatus = _dashboardDAL.getClaimCountByRecStatus();
                return Ok(recstatus);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetClaimCountByRecStatus", "DashboardController", ex.Message, null);
                return BadRequest("Error occurred: " + ex.Message);
            }
        }
        [Route("api/Dashboard/GetPreAuthCountByStatus")]
        [HttpGet]
        public IActionResult GetPreAuthCountByStatus()
        {
            try
            {
                var status = _dashboardDAL.getPreAuthCountByStatus();
                return Ok(status);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetPreAuthCountByStatus", "DashboardController", ex.Message, null);
                return BadRequest("Error occurred: " + ex.Message);
            }
        }


        [Route("api/Dashboard/GetPreAuthCountCaseType")]
        [HttpGet]
        public IActionResult GetPreAuthCountCaseType()
        {
            try
            {
                var casetype = _dashboardDAL.getPreAuthCountCaseType();
                return Ok(casetype);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetPreAuthCountCaseType", "DashboardController", ex.Message, null);
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Dashboard/GetPreAuthCountRecStatus")]
        [HttpGet]
        public IActionResult GetPreAuthCountRecStatus()
        {
            try
            {
                var recstatus = _dashboardDAL.getPreAuthCountRecStatus();
                return Ok(recstatus);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetPreAuthCountRecStatus", "DashboardController", ex.Message, null);
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Dashboard/GetEnrollmentAgeData")]
        [HttpGet]
        public IActionResult GetEnrollmentAgeData()
        {
            try
            {
                var age = _dashboardDAL.getEnrollmentAgeData();
                return Ok(age);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetEnrollmentAgeData", "DashboardController", ex.Message, null);
                return BadRequest("Error occurred: " + ex.Message);
            }
        }
        [Route("api/Dashboard/GetCountEnrollmentType")]
        [HttpGet]
        public IActionResult GetCountEnrollmentType()
        {
            try
            {
                var type = _dashboardDAL.getCountEnrollmentType();
                return Ok(type);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetCountEnrollmentType", "DashboardController", ex.Message, null);
                return BadRequest("Error occurred: " + ex.Message);
            }
        }
        [Route("api/Dashboard/GetEnrollmentRelationWiseData")]
        [HttpGet]
        public IActionResult GetEnrollmentRelationWiseData()
        {
            try
            {
                var relation = _dashboardDAL.getEnrollmentRelationWiseData();
                return Ok(relation);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetEnrollmentRelationWiseData", "DashboardController", ex.Message, null);
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Dashboard/GetIntimationCountByYear")]
        [HttpGet]
        public IActionResult GetIntimationCountByYear()
        {
            try
            {
                var year = _dashboardDAL.getIntimationCountByYear();
                return Ok(year);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetIntimationCountByYear", "DashboardController", ex.Message, null);
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Dashboard/GetClaimCountByClaimType")]
        [HttpGet]
        public IActionResult GetClaimCountByClaimType()
        {
            try
            {
                var clmtype = _dashboardDAL.getClaimCountByClaimType();
                return Ok(clmtype);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetClaimCountByClaimType", "DashboardController", ex.Message, null);
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [Route("api/Dashboard/GetIntimationCountByInsuranceCompany")]
        [HttpGet]
        public IActionResult GetIntimationCountByInsuranceCompany()
        {
            try
            {
                var insurance = _dashboardDAL.getIntimationCountByInsuranceCompany();
                return Ok(insurance);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetIntimationCountByInsuranceCompany", "DashboardController", ex.Message, null);
                return BadRequest("Error occurred: " + ex.Message);
            }
        }
    }

}
