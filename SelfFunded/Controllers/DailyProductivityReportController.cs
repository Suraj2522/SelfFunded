using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data;
using System.Reflection;
using System.Net.Http.Headers;

using Microsoft.AspNetCore.Mvc;

namespace SelfFunded.Controllers
{
    public class DailyProductivityReportController : Controller
    {
        private readonly DailyProductivityReportDal _dailyProductivityReportDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public DailyProductivityReportController(IConfiguration configuration, CommonDal common)
        {
            _dailyProductivityReportDal = new DailyProductivityReportDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }


        [Route("api/DailyProductivityReport/GetDailyProductivityReport")]
        [HttpPost]
        public IActionResult GetDailyProductivityReport()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                DailyProductivityReport dailyrpt = new DailyProductivityReport();
                dailyrpt.insuranceId = Convert.ToInt32(httpRequest.Form["insurance"]);
                // dailyrpt.userCode = Convert.ToInt32(httpRequest.Form["userCode"]);
                dailyrpt.fromDate = httpRequest.Form["fromDate"].ToString();
                dailyrpt.toDate = httpRequest.Form["toDate"].ToString();

                var report = _dailyProductivityReportDal.getDailyProductivityReport(dailyrpt);

                if (report == null || report.Count == 0)
                {
                    return NotFound(new { message = "No data found." });
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the exception
                commondal.LogError("GetDailyProductivityReport", "DailyProductivityReportController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
