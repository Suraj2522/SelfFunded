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
    public class MasterReportController : Controller
    {
        private readonly MasterReportDal _masterReportDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public MasterReportController(IConfiguration configuration, CommonDal common)
        {
            _masterReportDal = new MasterReportDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }
        [Route("api/Master/GetMasterReport")]
        [HttpPost]
        public IActionResult GetMasterReport()
        {
            try
            {

                var httpRequest = HttpContext.Request;
                MasterReport rpt = new MasterReport();
                rpt.insuranceCompanyId = string.IsNullOrEmpty(httpRequest.Form["insuranceCompany"]) ? 0 : Convert.ToInt32(httpRequest.Form["insuranceCompany"]);
                rpt.groupPolicyId = string.IsNullOrEmpty(httpRequest.Form["groupPolicy"]) ? 0 : Convert.ToInt32(httpRequest.Form["groupPolicy"]);
                rpt.preAuthNumber = httpRequest.Form["claimNoPreAuthNo"].ToString();
                rpt.diagnosisId = string.IsNullOrEmpty(httpRequest.Form["diagnosis"]) ? 0 : Convert.ToInt32(httpRequest.Form["diagnosis"]);
                rpt.insuredName = httpRequest.Form["insuredName"].ToString();
                rpt.policyNo = httpRequest.Form["policyCardNo"].ToString();
                rpt.fromDate = httpRequest.Form["fromDate"].ToString();
                rpt.toDate = httpRequest.Form["toDate"].ToString();
                rpt.type = httpRequest.Form["reportType"].ToString();
                rpt.claimStatus = httpRequest.Form["claimStatus"].ToString();
                rpt.providerId = string.IsNullOrEmpty(httpRequest.Form["provider"]) ? 0 : Convert.ToInt32(httpRequest.Form["provider"]);
                rpt.ailmentId = string.IsNullOrEmpty(httpRequest.Form["ailments"]) ? 0 : Convert.ToInt32(httpRequest.Form["ailments"]);
                rpt.insuranceType= httpRequest.Form["insuranceType"].ToString();

                DataTable dt = _masterReportDal.getMasterReport(rpt);

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
                    var worksheet = package.Workbook.Worksheets.Add("MasterReport");

                    // Load data table into the worksheet
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                    excelData= package.GetAsByteArray(); 
                }

                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "MasterReport.xlsx"
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                commondal.LogError("GetMasterReport", "MasterReportController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }


    }
}

