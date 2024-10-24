using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data;
using System.Reflection;
using System.Net.Http.Headers;

namespace SelfFunded.Controllers
{
    public class TDSReportController : Controller
    {
        private readonly TDSReportDal _tDSReportDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public TDSReportController(IConfiguration configuration, CommonDal common)
        {
            _tDSReportDal = new TDSReportDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }

        

        [Route("api/TDSReport/GetTDSReport")]
        [HttpPost]
        public IActionResult GetTDSReport()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                TDSReport tdsrpt = new TDSReport();
                tdsrpt.insuranceId = Convert.ToInt32(httpRequest.Form["insurance"]);
                tdsrpt.providerNo = Convert.ToInt32(httpRequest.Form["provider"]);
                tdsrpt.claimNo = httpRequest.Form["claimNo"];
                tdsrpt.fromDate = httpRequest.Form["fromDate"].ToString();
                tdsrpt.toDate = httpRequest.Form["toDate"].ToString();

                var report = _tDSReportDal.getTDSReport(tdsrpt);

                if (report == null || report.Count == 0)
                {
                    return NotFound(new { message = "No data found." });
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the exception
                commondal.LogError("GetTDSReport", "TDSReportController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
