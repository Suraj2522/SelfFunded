using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data;
using System.Net.Http.Headers;

namespace SelfFunded.Controllers
{
    public class ClaimSettlementReportController : Controller
    {
        private readonly ClaimSettlementReportDal _claimSettlementReportDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public ClaimSettlementReportController(IConfiguration configuration, CommonDal common)
        {
            _claimSettlementReportDal = new ClaimSettlementReportDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }


        [Route("api/ClaimSettlementReport/GetClaimSettlementReport")]
        [HttpPost]
        public IActionResult GetClaimSettlementReport()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                ClaimSettlementReport rpt = new ClaimSettlementReport();
                rpt.insuranceID = Convert.ToInt32(httpRequest.Form["insuranceCompany"]);
                rpt.insuredName = httpRequest.Form["insuredName"].ToString();
                rpt.claimNO = httpRequest.Form["claimNo"].ToString();
                rpt.providerNo = httpRequest.Form["provider"].ToString();
                rpt.fromDate = httpRequest.Form["fromDate"].ToString();
                rpt.toDate = httpRequest.Form["toDate"].ToString();
                // Assuming GetDataTableFromExcel method returns a DataTable
                DataTable dt = _claimSettlementReportDal.getClaimSettlementReport(rpt);

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
                    var worksheet = package.Workbook.Worksheets.Add("ClaimSettlementReport");

                    // Load data table into the worksheet
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                    excelData = package.GetAsByteArray();
                }

                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "ClaimSettlementReport.xlsx"
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                commondal.LogError("GetClaimSettlementReport", "ClaimSettlementReportController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
