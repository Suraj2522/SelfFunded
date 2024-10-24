using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SelfFunded.Controllers
{
    public class IntimationSheetInboundController : Controller
    {
        private readonly IntimationSheetInboundDal _intimationSheetInboundDal;
        private readonly CommonDal _commondal;
        private readonly string _configureFilePath;

        // Define the list of expected keys (if needed for validation)
        private readonly string[] expectedKeys = {
            "alternateContactNo", "caseType", "claimTypeId", "contactNo", "diagnosis", "employeeTypeId",
            "hospitalAddress1", "hospitalContactNo", "hospitalName", "insuranceCompanyId", "insuredName",
            "intimationFrom", "intimationType", "personContactNo", "personEmailId", "policyEndDate", "policyNo",
            "policyStartDate", "primaryMember", "relationId", "remarks", "requestType", "status", "treatment", "treatmentDate"
        };

        public IntimationSheetInboundController(IntimationSheetInboundDal intimationSheetInboundDal, CommonDal commonDal, IConfiguration configuration)
        {
            _intimationSheetInboundDal = intimationSheetInboundDal;
            _commondal = commonDal;
            _configureFilePath = configuration["DocumentUpload"] ?? "";
        }

        [Route("api/IntimationSheetInbound/InsertIntimationSheetInbound")]
        [HttpPost]
        public IActionResult InsertIntimationSheetInbound()
        {
            IntimationSheetInbound intimation = new IntimationSheetInbound();
            string msg = "";
            try
            {
                var httpRequest = HttpContext.Request;
                intimation.intimationId = Convert.ToInt64(httpRequest.Form["intimationId"]);
                intimation.alternateContactNo = httpRequest.Form["alternateContactNo"];
                intimation.caseType =httpRequest.Form["caseType"];
                intimation.claimTypeId = Convert.ToInt32(httpRequest.Form["claimTypeId"]);
                intimation.contactNo = httpRequest.Form["contactNo"];
                intimation.diagnosis = httpRequest.Form["diagnosis"];
          //      intimation.employeeTypeId = Convert.ToInt32(httpRequest.Form["employeeTypeId"]);
                intimation.hospitalAddress1 = httpRequest.Form["hospitalAddress1"];
                intimation.hospitalContactNo = httpRequest.Form["hospitalContactNo"];
                intimation.hospitalName = httpRequest.Form["hospitalName"];
               

                
                intimation.insuranceCompanyId =Convert.ToInt32( httpRequest.Form["insuranceCompanyId"]);
                intimation.insuredName = httpRequest.Form["insuredName"];
                intimation.intimationFrom = httpRequest.Form["intimationFrom"];
                intimation.intimationType = httpRequest.Form["intimationType"];
                intimation.personContactNo = httpRequest.Form["personContactNo"];
                intimation.personEmailId = httpRequest.Form["personEmailId"];
                intimation.policyEndDate = Convert.ToDateTime(httpRequest.Form["policyEndDate"]);
                intimation.policyNo = httpRequest.Form["policyNo"];
                intimation.policyStartDate = Convert.ToDateTime(httpRequest.Form["policyStartDate"]);
                intimation.primaryMember = httpRequest.Form["primaryMember"];
            //    intimation.relationId = Convert.ToInt32(httpRequest.Form["relationId"]);
                intimation.remarks = httpRequest.Form["remarks"];
                intimation.requestType = httpRequest.Form["requestType"];
                intimation.recStatus = httpRequest.Form["status"];
                intimation.treatment = httpRequest.Form["treatment"];
                intimation.treatmentDate = Convert.ToDateTime(httpRequest.Form["treatmentDate"]);

                // Handle file upload
                var docUpload = httpRequest.Form.Files["docUpload"];
                if (docUpload != null && docUpload.Length > 0)
                {
                    var fileName = Path.GetFileName(docUpload.FileName);
                    var _ext = Path.GetExtension(docUpload.FileName);
                    fileName = fileName.Replace(" ", "_");
                    var filePath = Path.Combine(_configureFilePath, fileName);

                    if (!Directory.Exists(_configureFilePath))
                    {
                        Directory.CreateDirectory(_configureFilePath);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        docUpload.CopyTo(fileStream);
                    }

                    // Optionally set file-related properties here in the intimation object
                    // intimation.FilePath = filePath;
                    // intimation.FileName = fileName;
                    // intimation.FileExtension = _ext;
                }

                // Call the DAL method to insert the data
                msg = _intimationSheetInboundDal.insertIntimationSheetInbound(intimation);
            }
            catch (Exception ex)
            {
                _commondal.LogError("InsertIntimationSheetInbound", "IntimationSheetInboundController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

            return Ok(new { message = msg });
        }
        [Route("api/IntimationSheetInbound/SearchIntimationDetails")]
        [HttpPost]
        public IActionResult SearchIntimationDetails()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                IntimationSheetInbound ISI = new IntimationSheetInbound();
                ISI.insuranceCompanyId = Convert.ToInt32(httpRequest.Form["insuranceCompanyId"]);
                ISI.caseType = httpRequest.Form["caseType"];
                ISI.insuredName = httpRequest.Form["insuredName"];
                ISI.intimationNo = httpRequest.Form["intimationNo"];
                ISI.fromDate = DateTime.ParseExact(httpRequest.Form["fromDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                ISI.toDate = DateTime.ParseExact(httpRequest.Form["toDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                ISI.orderByCol = httpRequest.Form["orderByCol"];
                ISI.loginTypeId = httpRequest.Form["loginTypeId"];
                ISI.insuranceIDs = httpRequest.Form["insuranceIDs"];

                List<IntimationSheetInbound>  clmdtls =_intimationSheetInboundDal.getIntimationDetails(ISI);
                return Ok(clmdtls);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

      

    }
}

