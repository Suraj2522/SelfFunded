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


public class EnrollmentController : ControllerBase
{
    private readonly EnrollmentDal _enrollmentDAL;
    string ConfigureFilePath;
    CommonDal commondal;
    private readonly int _maxColumnCount;
    public EnrollmentController(IConfiguration configuration, CommonDal common)
    {
        _enrollmentDAL = new EnrollmentDal(configuration, common);
        ConfigureFilePath = configuration["DocumentUpload"] ?? "";
        _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");


    }

    [Route("api/Enrollment/Upload")]
    [HttpPost]
    public async Task<IActionResult> UploadEnrollment()
    {
        try
        {
            var httpRequest = HttpContext.Request;

            // Retrieve form data
            var file = httpRequest.Form.Files["file"];
            int groupPolicyId = Convert.ToInt32(httpRequest.Form["groupPolicyId"]);
            int insuranceCompanyId = Convert.ToInt32(httpRequest.Form["insurance"]);
            var typeOfEnrollment = httpRequest.Form["typeOfEnrollment"]; // Ensure single value
            var loginType = "123";
            // Validate file format
            if (file == null || (Path.GetExtension(file.FileName).ToUpper() != ".XLS" && Path.GetExtension(file.FileName).ToUpper() != ".XLSX"))
            {
                return BadRequest("Invalid file format.");
            }

            // Check validity based on group policy (if needed)
            string strMessage = await _enrollmentDAL.CheckEnrollmentUploadValidity(1);
            if (!string.IsNullOrEmpty(strMessage))
            {
                return BadRequest(strMessage);
            }

            // Save uploaded file to the server
            var uploadPath = Path.Combine(ConfigureFilePath, file.FileName);
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Process Excel data
            DataTable dataTable = _enrollmentDAL.GetDataTableFromExcel(uploadPath);
            if (dataTable.Columns.Count > _maxColumnCount)
            {
                // HttpContext.Session.SetString("check column count", "errors occurred during file upload. Please check the returned data for details.");
                return Ok(new { message ="Excel Column Count not match.Please Upload Proper Format" });
            }
            var trimmedDataTable = _enrollmentDAL.TrimData(dataTable);
            var cleanedDataTable = _enrollmentDAL.DeleteBlankRows(trimmedDataTable);

            // Example: Upload cleaned data to database
            var errors = await _enrollmentDAL.UploadData(cleanedDataTable, file.FileName, insuranceCompanyId, groupPolicyId, loginType, typeOfEnrollment);

            // handle errors if necessary
            if (errors.Rows.Count > 0)
            {
                HttpContext.Session.SetString("existsdata", "errors occurred during file upload. Please check the returned data for details.");
                return BadRequest("Errors occurred during file upload. Please check the returned data for details.");
            }


            return Ok(new { message = "File uploaded successfully." });
        }
        catch (Exception ex)
        {
            commondal.LogError("UploadEnrollment", "EnrollmentController", ex.Message, "");
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [Route("api/Enrollment/SearchEnrollment")]
    [HttpPost]
    public IActionResult SearchEnrollment()
    {
        SqlConnection connection = null;
        try
        {
            var httpRequest = HttpContext.Request;

            int? ParseInt(string key)
            {
                return httpRequest.Form.ContainsKey(key) && !string.IsNullOrEmpty(httpRequest.Form[key])
                    ? (int?)Convert.ToInt32(httpRequest.Form[key])
                    : null;
            }

            decimal? ParseDecimal(string key)
            {
                return httpRequest.Form.ContainsKey(key) && !string.IsNullOrEmpty(httpRequest.Form[key])
                    ? (decimal?)Convert.ToDecimal(httpRequest.Form[key])
                    : null;
            }

            DateTime? ParseDateTime(string key)
            {
                return httpRequest.Form.ContainsKey(key) && !string.IsNullOrEmpty(httpRequest.Form[key])
                    ? (DateTime?)DateTime.Parse(httpRequest.Form[key])
                    : null;
            }

            var enrollment = new Enrollment
            {
                insuranceCompanyId = ParseInt("insured_company"),
                product = httpRequest.Form["products"],
                planNo = httpRequest.Form["plan"],
                insuredName = httpRequest.Form["insured_name"],
                policyHolder = httpRequest.Form["policy_holder"],
                policyNo = httpRequest.Form["policy_no"],
                recStatus = httpRequest.Form["status"],
                employeeCode = httpRequest.Form["employee_code"],
                member_Id = httpRequest.Form["member_id"],
                validFrom = ParseDateTime("from_date"),
                validTo = ParseDateTime("to_date"),
                policyType = httpRequest.Form["policy_type"],
                sumInsured = ParseDecimal("sumInsured_type"),
                enrollmentId = ParseInt("id")
            };

            List<Enrollment> enrollments = _enrollmentDAL.searchEnrollment(enrollment);
            return Ok(enrollments);
        }
        catch (Exception ex)
        {
            commondal.LogError("SearchEnrollment", "EnrollmentController", ex.Message, "");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
        }
        finally
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
//public IActionResult SearchEnrollment()
//{
//    SqlConnection connection = null;
//    try
//    {
//        var httpRequest = HttpContext.Request;

//        // Retrieve form data
//        int? insuranceCompanyId = httpRequest.Form.ContainsKey("insured_company") ?
//            (int?)Convert.ToInt32(httpRequest.Form["insured_company"]) : null;
//        string product = httpRequest.Form["products"];
//        string planNo = httpRequest.Form["plan"];
//        string insuredName = httpRequest.Form["insured_name"];
//        string policyHolder = httpRequest.Form["policy_holder"];
//        string policyNo = httpRequest.Form["policy_no"];
//        string status = httpRequest.Form["status"];
//        string employeeCode = httpRequest.Form["employee_code"];
//        string member_Id = httpRequest.Form["member_id"];
//        DateTime? validFrom = httpRequest.Form.ContainsKey("from_date") && !string.IsNullOrEmpty(httpRequest.Form["from_date"]) ?
//                (DateTime?)DateTime.Parse(httpRequest.Form["from_date"]) : null;
//        DateTime? validTo = httpRequest.Form.ContainsKey("to_date") && !string.IsNullOrEmpty(httpRequest.Form["to_date"]) ?
//            (DateTime?)DateTime.Parse(httpRequest.Form["to_date"]) : null; string policyType = httpRequest.Form["policy_type"];
//        decimal? sumInsured = httpRequest.Form.ContainsKey("sumInsured_type") ?
//            (decimal?)Convert.ToDecimal(httpRequest.Form["sumInsured_type"]) : null;
//        int? enrollmentId = httpRequest.Form.ContainsKey("id") ?
//            (int?)Convert.ToInt32(httpRequest.Form["id"]) : null;

//        // Populate the Enrollment object
//        var enrollment = new Enrollment
//        {
//            insuranceCompanyId = insuranceCompanyId,
//            product = product,
//            planNo = planNo,
//            insuredName = insuredName,
//            policyHolder = policyHolder,
//            policyNo = policyNo,
//            recStatus = status,
//            employeeCode = employeeCode,
//            member_Id = member_Id,
//            validFrom = validFrom,
//            validTo = validTo,
//            policyType = policyType,
//            sumInsured = sumInsured,
//           enrollmentId = enrollmentId
//        };

//        List<Enrollment> enrollments = _enrollmentDAL.searchEnrollment(enrollment);
//        return Ok(enrollments);
//    }
//    catch (Exception ex)
//    {
//        commondal.LogError("SearchEnrollment", "EnrollmentController", ex.Message, "");
//        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
//    }
//    finally
//    {
//        if (connection != null && connection.State == System.Data.ConnectionState.Open)
//        {
//            connection.Close();
//        }

//    }
//}

[Route("api/Enrollment/GetEnrollment")]
    [HttpGet]

    public IActionResult GetEnrollment()
    {
        SqlConnection connection = null;
        try
        {
            List<Enrollment> enrollment = new List<Enrollment>();
            enrollment = _enrollmentDAL.getAllEnrollment();
            return Ok(enrollment);
        }
        catch (Exception ex)
        {
            commondal.LogError("GetEnrollment", "EnrollmentController", ex.Message, "");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
        }
        finally
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    [Route("api/Enrollment/Validate")]
    [HttpGet]
    public IActionResult ValidateImport(int importId)
    {
        try
        {
            // Assuming _enrollmentDAL.ValidateImport returns byte[] representing Excel data
            var excelData = _enrollmentDAL.ValidateImport(importId);

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



    [Route("api/Enrollment/ProceedToUpload")]
    [HttpGet]
    public ActionResult ProceedToUpload(int importId)
    {
        // Call the DAL method to proceed with the upload and capture the result
        var result = _enrollmentDAL.ProceedToUpload(importId);

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



    [Route("api/Enrollment/Delete")]
    [HttpGet]
    public async Task<IActionResult> DeleteImportedData(int importId)
    {
        try
        {
            int result = await _enrollmentDAL.deleteImportedData(importId);
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

            commondal.LogError("DeleteImportedData", "EnrollmentController", ex.Message, "");
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
       [Route("api/Enrollment/GetPolicyDate")]
    [HttpGet]
 
    public IActionResult GetPolicyDate(int insuranceId)
    {
        try
        {
            List<Enrollment> list = _enrollmentDAL.getPolicyDate(insuranceId);
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest("Error occurred while retrieving PolicyDate: " + ex.Message);
        }
    }
}