using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data;
using System.Net.Http.Headers;

namespace SelfFunded.Controllers
{
    public class SavingReportController : Controller
    {
        private readonly SavingReportDal _savingReportDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public SavingReportController(IConfiguration configuration, CommonDal common)
        {
            _savingReportDal = new SavingReportDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }


        [Route("api/SavingReport/GetSavingReport")]
        [HttpPost]
        public IActionResult GetSavingReport()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                 SavingReport rpt = new SavingReport();
                rpt.insuranceCompanyId = Convert.ToInt32(httpRequest.Form["insuranceCompany"]);
                rpt.preAuthNumber = httpRequest.Form["claimNoPreAuthNo"].ToString();
                rpt.patientName = httpRequest.Form["insuredName"].ToString();
                rpt.fromDate = httpRequest.Form["fromDate"].ToString();
                rpt.toDate = httpRequest.Form["toDate"].ToString();
                // Assuming GetDataTableFromExcel method returns a DataTable
                DataTable dt = _savingReportDal.getSavingReport(rpt);

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
                    var worksheet = package.Workbook.Worksheets.Add("SavingReport");

                    // Load data table into the worksheet
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                    excelData = package.GetAsByteArray();
                }

                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "SavingReport.xlsx"
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                commondal.LogError("GetSavingReport", "SavingReportController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
