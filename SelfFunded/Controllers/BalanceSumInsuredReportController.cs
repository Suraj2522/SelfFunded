using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data;
using System.Net.Http.Headers;

namespace SelfFunded.Controllers
{
    public class BalanceSumInsuredReportController : Controller
    {
        private readonly BalanceSumInsuredReportDal _balanceSumInsuredReportDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public BalanceSumInsuredReportController(IConfiguration configuration, CommonDal common)
        {
            _balanceSumInsuredReportDal = new BalanceSumInsuredReportDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }


        [Route("api/BalanceSumInsuredReport/GetBalanceSumInsuredReport")]
        [HttpPost]
        public IActionResult GetBalanceSumInsuredReport()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                BalanceSumInsuredReport balsirpt = new BalanceSumInsuredReport();
                balsirpt.insuranceCompanyId =  Convert.ToInt32(httpRequest.Form["insuranceCompany"]);
                balsirpt.groupPolicyId= Convert.ToInt32(httpRequest.Form["groupPolicy"]);
                balsirpt.fromDate = httpRequest.Form["fromDate"].ToString();
                balsirpt.toDate = httpRequest.Form["toDate"].ToString();
                DataTable dt = _balanceSumInsuredReportDal.getBalanceSumInsuredReport(balsirpt);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return NotFound(new { message = "No data found " });
                }

                // Convert DataTable to Excel file (as a byte array)
                byte[] excelData;


                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Generate Excel file
                using (var package = new OfficeOpenXml.ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("BalanceSumInsuredReport");

                    // Load data table into the worksheet
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                    excelData = package.GetAsByteArray();
                }

                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "BalanceSumInsuredReport.xlsx"
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                commondal.LogError("GetBalanceSumInsuredReport", "BalanceSumInsuredReportController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
