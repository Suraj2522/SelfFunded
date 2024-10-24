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
using Microsoft.AspNetCore.Mvc;

namespace SelfFunded.Controllers
{
    public class HospitalOutstandingReportController : Controller
    {
        private readonly HospitalOutstandiingReportDal _hospitalOutstandiingReportDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public HospitalOutstandingReportController(IConfiguration configuration, CommonDal common)
        {
            _hospitalOutstandiingReportDal = new HospitalOutstandiingReportDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }


        [Route("api/HospitalOutstandingReport/GetHospitalOutstandingReport")]
        [HttpPost]
        public IActionResult GetHospitalOutstandingReport()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                HospitalOutstandingReport tdsrpt = new HospitalOutstandingReport();
                tdsrpt.insuranceID = Convert.ToInt32(httpRequest.Form["insuranceCompany"]);
                tdsrpt.invoiceNo = httpRequest.Form["invoiceno"];
                tdsrpt.outwardNo= httpRequest.Form["outwardno"];
                tdsrpt.claimNO = httpRequest.Form["claimno"];
                tdsrpt.fromDate = httpRequest.Form["fromDate"].ToString();
                tdsrpt.toDate = httpRequest.Form["toDate"].ToString();
                // Assuming GetDataTableFromExcel method returns a DataTable
                DataTable dt = _hospitalOutstandiingReportDal.getHospitalOutstandingReport(tdsrpt);

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
                    var worksheet = package.Workbook.Worksheets.Add("HospitalOutstandingReport");

                    // Load data table into the worksheet
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                    excelData = package.GetAsByteArray();
                }

                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "HospitalOutstandingReport.xlsx"
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                commondal.LogError("GetHospitalOutstandingReport", "HospitalOutstandingReportController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
