using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using SelfFunded.Models;
using System.Data;
using Dapper;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Data.SqlClient;
using SelfFunded.DAL;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;


namespace SelfFunded.Controllers
{
    public class PhmWeeklyReportController : Controller
    {
        private readonly PhmWeeklyReportDal _phmWeeklyReportDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public PhmWeeklyReportController(IConfiguration configuration, CommonDal common)
        {
            _phmWeeklyReportDal = new PhmWeeklyReportDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }


        [Route("api/PhmWeeklyReport/GetPhmWeeklyReport")]
        [HttpPost]
        public IActionResult GetPhmWeeklyReport()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                PhmWeeklyReport rpt = new PhmWeeklyReport();
                 rpt.insuranceId = Convert.ToInt32(httpRequest.Form["insuranceCompany"]);
                rpt.fromDate = httpRequest.Form["fromDate"].ToString();
                rpt.toDate = httpRequest.Form["toDate"].ToString();
                //Assuming GetDataTableFromExcel method returns a DataTable
                DataTable dt = _phmWeeklyReportDal.getPhmWeeklyReport(rpt);

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
                    var worksheet = package.Workbook.Worksheets.Add("PHMWeeklyReport");

                    // Load data table into the worksheet
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                    excelData = package.GetAsByteArray();
                }

                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "PHMWeeklyReport.xlsx"
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                commondal.LogError("GetPhmWeeklyReport", "PhmWeeklyReportController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
