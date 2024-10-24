using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data;
using System.Net.Http.Headers;

namespace SelfFunded.Controllers
{
    public class ClientMISReportController : Controller
    {
        private readonly ClientMISReportDal _clientMISReportDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public ClientMISReportController(IConfiguration configuration, CommonDal common)
        {
            _clientMISReportDal = new ClientMISReportDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }


        [Route("api/ClientMISReport/GetClientMISReport")]
        [HttpPost]
        public IActionResult GetClientMISReport()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                ClientMISReport rpt = new ClientMISReport();

                rpt.insuranceCompanyId = string.IsNullOrEmpty(httpRequest.Form["insuranceCompany"]) ? 0 : Convert.ToInt32(httpRequest.Form["insuranceCompany"]);
                rpt.fromDate = httpRequest.Form["fromDate"].ToString();
                rpt.toDate = httpRequest.Form["toDate"].ToString();
                // Assuming GetDataTableFromExcel method returns a DataTable
                DataTable dt = _clientMISReportDal.getClientMISReport(rpt);

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
                    var worksheet = package.Workbook.Worksheets.Add("ClientMISReport");

                    // Load data table into the worksheet
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                    excelData = package.GetAsByteArray();
                }

                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "ClientMISReport.xlsx"
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                commondal.LogError("GetClientMISReport", "ClientMISReportController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
