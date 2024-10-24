using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using SelfFunded.Models;
using System.Data;
using System.Data.SqlClient;
using SelfFunded.DAL;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Net.Http.Headers;

namespace SelfFunded.Controllers
{
    public class PaymentUploadSelfFunded : Controller
    {
        private readonly PaymentUploadSelfFundedDal _paymentUploadSelfFundedDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public PaymentUploadSelfFunded(IConfiguration configuration, CommonDal common)
        {
            _paymentUploadSelfFundedDal = new PaymentUploadSelfFundedDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");


        }
       
        [Route("api/PaymentUploadSelfFunded/Upload")]
        [HttpPost]
        public async Task<IActionResult> UploadPaymentUploadSelfFunded()
        {
            try
            {
                var httpRequest = HttpContext.Request;

                // Retrieve form data
                var file = httpRequest.Form.Files["file"];
               
                var message = httpRequest.Form["message"]; // Ensure single value
                var loginType = "123";
                // Validate file format
                if (file == null || (Path.GetExtension(file.FileName).ToUpper() != ".XLS" && Path.GetExtension(file.FileName).ToUpper() != ".XLSX"))
                {
                    return BadRequest("Invalid file format.");
                }

               
               

                // Save uploaded file to the server
                var uploadPath = Path.Combine(ConfigureFilePath, file.FileName);
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Process Excel data
                DataTable dataTable = _paymentUploadSelfFundedDal.GetDataTableFromExcel(uploadPath);
                //if (dataTable.Columns.Count > _maxColumnCount)
                //{
                //    // HttpContext.Session.SetString("check column count", "errors occurred during file upload. Please check the returned data for details.");
                //    return Ok(new { message = "Excel Column Count not match.Please Upload Proper Format" });
                //}
                var trimmedDataTable = _paymentUploadSelfFundedDal.TrimData(dataTable);
                var cleanedDataTable = _paymentUploadSelfFundedDal.DeleteBlankRows(trimmedDataTable);

                // Example: Upload cleaned data to database
                var errors = await _paymentUploadSelfFundedDal.UploadData(cleanedDataTable, file.FileName, loginType);

                // handle errors if necessary
                if (errors.Equals("This Excel is Uploaded"))
                {
                    return Ok(new { message = "File uploaded successfully." });
                }


                return Ok(new { message = "This Excel Already Uploaded" });
            }
            catch (Exception ex)
            {
                commondal.LogError("UploadEnrollment", "EnrollmentController", ex.Message, "");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }
        [Route("api/PaymentUploadSelfFunded/Delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteData(int importId)
        {
            try
            {
                int result = await _paymentUploadSelfFundedDal.deleteData(importId);
                if (result > 0)
                {
                    return Ok(new { message = "Data deleted successfully." });

                }
                else
                {
                    return NotFound("Data not found.");
                }
            }
            catch (Exception ex)
            {

                commondal.LogError("deleteData", "PaymentUploadSelfFunded", ex.Message, "");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [Route("api/PaymentUploadSelfFunded/GetPaymentLog")]
        [HttpGet]
        public IActionResult GetPaymentLog()
        {
            try
            {
                var payment = _paymentUploadSelfFundedDal.getPaymentLog();
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }
        [Route("api/PaymentUploadSelfFunded/Validate")]
        [HttpGet]
        public IActionResult ValidateImport( int importId)
        {
            try
            {
                // Assuming _enrollmentDAL.ValidateImport returns byte[] representing Excel data
                var excelData = _paymentUploadSelfFundedDal.ValidateImport(importId);

                if (excelData == null || excelData.Length == 0)
                {
                    return NotFound(new { message = "No data found for the provided importId." });
                }

                // Return the file with appropriate headers
                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "ValidationResults.xlsx"
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                // Log the error
                commondal.LogError("ValidateImport", "EnrollmentController", ex.Message, "EnrollmentDal");

                // Return a 500 Internal Server Error response
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error");
            }
        }
        [Route("api/PaymentUploadSelfFunded/ProceedToUpload")]
        [HttpGet]
        public ActionResult ProceedToUpload(int importId)
        {
            // Call the DAL method to proceed with the upload and capture the result
            var result = _paymentUploadSelfFundedDal.ProceedToUpload(importId);

            // Check if the result is null
            if (result == null)
            {
                return NotFound(new { message = "Upload not found or failed." });
            }

            // Check if the result equals "Uploaded Successfully!" with case-insensitive comparison
            if (result.Equals("Uploaded Successfully!..", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(new { message = "proceeded successfully." });
            }

            // Handle other cases if necessary
            return BadRequest(new { message = "Upload failed." });
        }
    }
}
