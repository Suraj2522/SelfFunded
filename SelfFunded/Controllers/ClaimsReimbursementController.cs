using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;

namespace SelfFunded.Controllers
{
    public class ClaimsReimbursementController : Controller
    {
        private readonly ClaimReimbursementDal _claimReimbursementDal;
        private readonly CommonDal commondal;
        private readonly string _configureFilePath; public ClaimsReimbursementController(ClaimReimbursementDal claimReimbursementDal, CommonDal commonDal, IConfiguration configuration)
        {
            _claimReimbursementDal = claimReimbursementDal;
            commondal = commonDal;
            _configureFilePath = configuration["DocumentUpload"] ?? "";
        }



        [Route("api/ClaimsReimbursement/InsertReimbursement")]
        [HttpPost]
        public IActionResult InsertReimbursement()
        {
            Claims claim = new Claims();
            string msg = "";
            try
            {
                var httpRequest = HttpContext.Request;
                //claim.accountHolderName = httpRequest.Form["accountHolderName"];
                claim.claimId = Convert.ToInt32(httpRequest.Form["claimId"]);
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
                claim.recStatus = httpRequest.Form["status"];
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
                msg = _claimReimbursementDal.insertReimbursement(claim);
            }


            catch (Exception ex)
            {
                commondal.LogError("InsertReimbursement", "ClaimsReimbursementController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

            return Ok(new { message = msg });
        }

        [Route("api/ClaimsReimbursement/GetReimbursementType")]
        [HttpGet]
        public IActionResult GetReimbursementType()
        {
            try
            {
                var reimbursementTypes = _claimReimbursementDal.getReimbursementType();
                return Ok(reimbursementTypes);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }
    }
}
