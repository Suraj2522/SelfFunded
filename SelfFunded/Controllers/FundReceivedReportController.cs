using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.AccessControl;

namespace SelfFunded.Controllers
{
    public class FundReceivedReportController : Controller
    {
        private readonly FundReceivedReportDal _fundReceivedReportDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public FundReceivedReportController(IConfiguration configuration, CommonDal common)
        {
            _fundReceivedReportDal = new FundReceivedReportDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }


       

    [Route("api/FundReceivedReport/GetFundReceivedReportSearch")]
        [HttpPost]
        public IActionResult GetFundReceivedReportSearch()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                FundReceivedReport fundrpt = new FundReceivedReport();
                fundrpt.insuranceID = Convert.ToInt32(httpRequest.Form["insurance"]);
                fundrpt.debitID = Convert.ToInt32(httpRequest.Form["inovoiceNo"]);
                fundrpt.claimNO = httpRequest.Form["claimNo"];

                var report = _fundReceivedReportDal.getALlFundReceivedReport(fundrpt);

                if (report == null || report.Count==0)
                {
                    return NotFound(new { message = "No data found." });
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the exception
                commondal.LogError("GetFundReceivedReport", "FundReceivedReportController", ex.Message, "FundReceivedReportDal");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
       }
    }
}
