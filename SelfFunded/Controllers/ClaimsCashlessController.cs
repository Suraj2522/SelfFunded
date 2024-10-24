using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System;
using System.IO;

namespace SelfFunded.Controllers
{
    public class ClaimsCashlessController : Controller
    {
        private readonly ClaimsCashlessDal _claimsDal;
        private readonly CommonDal _commonDal;
        private readonly string _configureFilePath;
       

        public ClaimsCashlessController(ClaimsCashlessDal claimsDal, CommonDal commonDal, IConfiguration configuration)
        {
            _claimsDal = claimsDal;
            _commonDal = commonDal;
            _configureFilePath = configuration["DocumentUpload"] ?? "";

        }

        [Route("api/Claims/InsertCashlessClaims")]
        [HttpPost]
        public IActionResult InsertClaims()
        {
            Claims claim = new Claims();
            string msg = "";
            try
            {
                var httpRequest = HttpContext.Request;
                claim.claimId = Convert.ToInt32(httpRequest.Form["claimId"]);
                //claim.accountHolderName = httpRequest.Form["accountHolderName"];
                claim.accountNo = httpRequest.Form["accountNumber"];
//claim.accountType = httpRequest.Form["accountType"];
                claim.age = httpRequest.Form["age"];
                claim.alternateContactNo = httpRequest.Form["alternateContactNo"];
           //     claim.approvedAmount = Convert.ToDecimal(httpRequest.Form["approvedAmount"]);
                claim.auditorRemarks = httpRequest.Form["auditorRemarks"];
             //   claim.bankBranch = httpRequest.Form["bankBranch"];
                claim.bankName = httpRequest.Form["bankName"];
                claim.caseType = httpRequest.Form["caseType"];
             //   claim.category = httpRequest.Form["category"];
                claim.causeOfDeath = httpRequest.Form["causeOfDeath"];
                //claim.claimDate = string.IsNullOrEmpty(httpRequest.Form["claimDate"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["claimDate"]);
                claim.claimNo = httpRequest.Form["claimNo"];
              //  claim.claimProcessDate = string.IsNullOrEmpty(httpRequest.Form["claimProcessDate"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["claimProcessDate"]);
                claim.complaints = httpRequest.Form["complaints"];
              //  claim.confirmAccountNumber = httpRequest.Form["confirmAccountNumber"];
                //claim.dateOfDeath = string.IsNullOrEmpty(httpRequest.Form["dateOfDeath"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["dateOfDeath"]);
                //claim.deductionAmount = string.IsNullOrEmpty(httpRequest.Form["deduction"]) ? (decimal?)null : Convert.ToDecimal(httpRequest.Form["deduction"]);

                claim.diagnosis = httpRequest.Form["diagnosis"];
                //claim.dischargeDate = string.IsNullOrEmpty(httpRequest.Form["dischargeDate"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["dischargeDate"]);
                //claim.discountAmount = string.IsNullOrEmpty(httpRequest.Form["discount"]) ? (decimal?)null : Convert.ToDecimal(httpRequest.Form["discount"]);
              //  claim.dateOfBirth = Convert.ToDateTime(httpRequest.Form["dob"]);
                claim.doctorName = httpRequest.Form["doctorName"];

                claim.employeeCode = httpRequest.Form["employeeCode"];
               // claim.employeeTypeId = Convert.ToInt32(httpRequest.Form["employeeType"]);
                //claim.estimatedAmount = string.IsNullOrEmpty(httpRequest.Form["estimatedAmount"]) ? (decimal?)null : Convert.ToDecimal(httpRequest.Form["estimatedAmount"]);
                claim.gender = httpRequest.Form["gender"];
                //claim.grossAmount = Convert.ToDecimal(httpRequest.Form["grossAmount"]);
                //claim.groupPolicyId = Convert.ToInt32(httpRequest.Form["groupPolicy"]);
                claim.hospitalType = httpRequest.Form["hospitalType"];
                claim.hospitalVerification = httpRequest.Form["hospitalVerification"];
            //    claim.ifscNumber = httpRequest.Form["ifscNumber"];
                claim.insuranceCompanyId = Convert.ToInt32(httpRequest.Form["insurance"]);
                claim.insuredName = httpRequest.Form["insuredName"];
                claim.intimationId = Convert.ToInt32(httpRequest.Form["intimationId"]);
                //claim.invoiceDate = string.IsNullOrEmpty(httpRequest.Form["invoiceDate"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["invoiceDate"]);
                //claim.invoiceNumber = Convert.ToInt32(httpRequest.Form["invoiceNumber"]);
               // claim.lengthOfStay = Convert.ToDecimal(httpRequest.Form["lengthOfStay"]);
              //  claim.medicalDetails = httpRequest.Form["medicalDetails"];
                //claim.nationality = Convert.ToInt32(httpRequest.Form["nationality"]);
                //claim.netAmount = string.IsNullOrEmpty(httpRequest.Form["netAmount"]) ? (decimal?)null : Convert.ToDecimal(httpRequest.Form["netAmount"]);
                //claim.approvedAmount = Convert.ToDecimal(httpRequest.Form["netApprovedAmount"]);
                //claim.networkType = Convert.ToBoolean(httpRequest.Form["networkType"]);

                claim.contactNo = httpRequest.Form["personalContact"];
                //claim.planId = Convert.ToInt32(httpRequest.Form["plan"]);
                claim.cardNo = httpRequest.Form["policyCardNo"];
                //claim.policyEndDate = string.IsNullOrEmpty(httpRequest.Form["policyEndDate"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["policyEndDate"]);
                //claim.policyStartDate = string.IsNullOrEmpty(httpRequest.Form["policyStartDate"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["policyStartDate"]);
                claim.primaryMember = httpRequest.Form["primaryMember"];
                //claim.productId = Convert.ToInt32(httpRequest.Form["product"]);
                //claim.providerNo = Convert.ToInt32(httpRequest.Form["providerNo"]);
                //claim.relationId = Convert.ToInt32(httpRequest.Form["relation"]);
                claim.auditorRemarks = httpRequest.Form["remark"];
                claim.remarks = httpRequest.Form["remarks"];
                claim.requestType = httpRequest.Form["requestType"];
                //claim.revisedAmount = string.IsNullOrEmpty(httpRequest.Form["revisedAmount"]) ? (decimal?)null : Convert.ToDecimal(httpRequest.Form["revisedAmount"]);
                //claim.roomTypeId = string.IsNullOrEmpty(httpRequest.Form["roomType"]) ? (int?)null : Convert.ToInt32(httpRequest.Form["roomType"]);
              //  claim.selectedAilment = httpRequest.Form["selectedAilment"];
                claim.coPayType = httpRequest.Form["selectedCoPay"];
                claim.claimStatus = (httpRequest.Form["status"]);
                //claim.tariffDeduction = Convert.ToDecimal(httpRequest.Form["tariffDeduction"]);
                claim.treatment = httpRequest.Form["treatment"];
                //claim.treatmentDate = string.IsNullOrEmpty(httpRequest.Form["treatmentDate"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["treatmentDate"]);

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

                    // Optionally set file-related properties here in the claim object
                    // claim.FilePath = filePath;
                    // claim.FileName = fileName;
                    // claim.FileExtension = _ext;
                }

                // Call the DAL method to insert the data
                msg = _claimsDal.insertCashlessClaimsDetails(claim);
            }
            catch (Exception ex)
            {
                _commonDal.LogError("InsertCashlessClaims", "ClaimsCashlessController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

            return Ok(new { message = msg });
        }

        [Route("api/Claims/GetCashlessType")]
        [HttpGet]
        public IActionResult GetCashlessType()
        {
            try
            {
                var cashlessTypes = _claimsDal.getCashlessType();
                return Ok(cashlessTypes);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }
        [Route("api/Claims/GetClaims")]
        [HttpPost]
       public IActionResult GetClaims()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                Claims clm = new Claims();
                clm.insuranceCompanyId = Convert.ToInt32(httpRequest.Form["insuranceCompanyId"]);
                clm.insuredName = httpRequest.Form["insuredName"];
                clm.policyNo = httpRequest.Form["policyNo"];
                clm.fromDate = httpRequest.Form["fromDate"];
                clm.toDate = httpRequest.Form["toDate"];
                clm.stageId = Convert.ToInt32(httpRequest.Form["stageId"]);
                clm.claimNo = httpRequest.Form["claimNo"];
                clm.caseType = httpRequest.Form["caseType"];
                clm.primaryMember = httpRequest.Form["primaryMember"];
                clm.uniqueId = httpRequest.Form["uniqueId"];
                clm.contactNo = httpRequest.Form["contactNo"];
                clm.insuranceType = httpRequest.Form["insuranceType"];
                clm.loginTypeId = Convert.ToInt32(httpRequest.Form["loginTypeId"]);
                clm.insuranceIDs = httpRequest.Form["insuranceIDs"];
                var clmdtls = _claimsDal.getClaims(clm);
                return Ok(clmdtls);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }
    }
}
