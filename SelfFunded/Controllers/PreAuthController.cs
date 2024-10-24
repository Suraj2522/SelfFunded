using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System;
using System.IO;

namespace YourNamespace
{
    [Route("api/PreAuth")]
    [ApiController]
    public class PreAuthController : ControllerBase
    {
        private readonly string _configureFilePath; // Define your file path here
        private readonly PreAuthDal _preAuthDal; // Define your DAL here
        private readonly CommonDal _commonDal;
        public PreAuthController(PreAuthDal preAuthDal, CommonDal commonDal, IConfiguration configuration)
        {
            _preAuthDal = preAuthDal;
            _commonDal = commonDal;
            _configureFilePath = configuration["DocumentUpload"] ?? "";
        }

        [Route("InsertPreAuth")]
        [HttpPost]
        public IActionResult InsertPreAuth()
        {
            PreAuth preauths = new PreAuth();
            string msg = "";
            try
            {
                var httpRequest = HttpContext.Request;
                preauths.preAuthID = Convert.ToInt64(httpRequest.Form["preAuthID"]);
                //preauths.preAuthNumber = httpRequest.Form["preAuthNumber"];
                preauths.insuredName = httpRequest.Form["insuredName"];
                //preauths.gender = httpRequest.Form["gender"];
                //preauths.dateOfBirth = Convert.ToDateTime(httpRequest.Form["dateOfBirth"]);
                //preauths.age = Convert.ToInt32(httpRequest.Form["age"]);
                preauths.policyNo = httpRequest.Form["policyNo"];


                //preauths.contactNo = httpRequest.Form["contactNo"];
                //preauths.cardNo = httpRequest.Form["cardNo"];
                //preauths.networkType = httpRequest.Form["networkType"];
                //preauths.providerId = Convert.ToInt32(httpRequest.Form["providerId"]);
                //preauths.roomTypeId = Convert.ToInt32(httpRequest.Form["roomTypeId"]);
                //preauths.doctorName = httpRequest.Form["doctorName"];
                //preauths.docType = httpRequest.Form["docType"];
                //preauths.complaints = httpRequest.Form["complaints"];
                //preauths.diagnosis = httpRequest.Form["diagnosis"];
                //preauths.treatment = httpRequest.Form["treatment"];
                //preauths.lengthOfStay = Convert.ToInt32(httpRequest.Form["lengthOfStay"]);
                //preauths.treatmentDate = Convert.ToDateTime(httpRequest.Form["treatmentDate"]);
                //preauths.processingCurrencyId = Convert.ToInt32(httpRequest.Form["processingCurrencyId"]);
                //preauths.finalPayableCurrencyId = Convert.ToInt32(httpRequest.Form["finalPayableCurrencyId"]);
                //preauths.createByUserId = Convert.ToInt32(httpRequest.Form["createByUserId"]);
                //preauths.createDate = Convert.ToDateTime(httpRequest.Form["createDate"]);
                //preauths.modifyByUserId = Convert.ToInt32(httpRequest.Form["modifyByUserId"]);
                //preauths.modifyDate = Convert.ToDateTime(httpRequest.Form["modifyDate"]);
                //preauths.recStatus = httpRequest.Form["recStatus"];
                preauths.insuranceCompanyId = Convert.ToInt32(httpRequest.Form["insuranceCompanyId"]);
                //preauths.ailmentId = Convert.ToInt32(httpRequest.Form["ailmentId"]);
                preauths.intimationNo = (httpRequest.Form["intimationNo"]);
                //preauths.grossAmount = Convert.ToDecimal(httpRequest.Form["grossAmount"]);
                //preauths.approvedAmount = Convert.ToDecimal(httpRequest.Form["approvedAmount"]);
                //preauths.rejectedAmount = Convert.ToDecimal(httpRequest.Form["rejectedAmount"]);
                //preauths.netAmount = Convert.ToDecimal(httpRequest.Form["netAmount"]);
                //preauths.claimStatus = Convert.ToInt32(httpRequest.Form["claimStatus"]);
                //preauths.referredToIns = Convert.ToBoolean(httpRequest.Form["referredToIns"]);
                //preauths.remarks = httpRequest.Form["remarks"];
                //preauths.certificateNo = httpRequest.Form["certificateNo"];
                //preauths.coPayType = httpRequest.Form["coPayType"];
                //preauths.coPayValue = Convert.ToDecimal(httpRequest.Form["coPayValue"]);
                //preauths.coInsurance = Convert.ToDecimal(httpRequest.Form["coInsurance"]);
                //preauths.previousPreAuthId = Convert.ToInt32(httpRequest.Form["previousPreAuthId"]);
                //preauths.cancelRemarks = httpRequest.Form["cancelRemarks"];
                //preauths.nationality = Convert.ToInt32(httpRequest.Form["nationality"]);
                //preauths.referredToIns = Convert.ToBoolean(httpRequest.Form["referredToIns"]);
                //preauths.prefix = httpRequest.Form["prefix"];
                //preauths.previousPreAuthNo = httpRequest.Form["previousPreAuthNo"];
                //preauths.stageId = Convert.ToInt32(httpRequest.Form["stageId"]);
                preauths.caseType = httpRequest.Form["caseType"];
                // preauths.dischargeDate = Convert.ToDateTime(httpRequest.Form["dischargeDate"]);
                // preauths.providerNo = Convert.ToInt32(httpRequest.Form["providerNo"]);
                // preauths.providerName = httpRequest.Form["providerName"];
                // preauths.countryCode = httpRequest.Form["countryCode"];
                // preauths.countryName = httpRequest.Form["countryName"];
                // preauths.stateCode = httpRequest.Form["stateCode"];
                // preauths.stateName = httpRequest.Form["stateName"];
                // preauths.cityCode = httpRequest.Form["cityCode"];
                // preauths.cityName = httpRequest.Form["cityName"];
                // preauths.hospitalName = httpRequest.Form["hospitalName"];
                // preauths.providerCategoryType = httpRequest.Form["providerCategoryType"];
                // preauths.providerAddress1 = httpRequest.Form["providerAddress1"];
                // preauths.providerAddress2 = httpRequest.Form["providerAddress2"];
                // preauths.providerAddressArea = httpRequest.Form["providerAddressArea"];
                // preauths.providerPincode = httpRequest.Form["providerPincode"];
                // preauths.providerAreaCode = httpRequest.Form["providerAreaCode"];
                // preauths.providerContactNo = httpRequest.Form["providerContactNo"];
                // preauths.providerFaxNo = httpRequest.Form["providerFaxNo"];
                // preauths.providerEmailId = httpRequest.Form["providerEmailId"];
                // preauths.providerWebsite = httpRequest.Form["providerWebsite"];
                // preauths.referredToInsDate = Convert.ToDateTime(httpRequest.Form["referredToInsDate"]);
                // preauths.locked = httpRequest.Form["locked"];
                // preauths.lockedBy = Convert.ToInt32(httpRequest.Form["lockedBy"]);
                // preauths.lockDate = Convert.ToDateTime(httpRequest.Form["lockDate"]);
                // preauths.databaseType = httpRequest.Form["databaseType"];
                // preauths.oldProviderNO = httpRequest.Form["oldProviderNO"];
                // preauths.oldProviderName = httpRequest.Form["oldProviderName"];
                // preauths.mainPreAuthDate = Convert.ToDateTime(httpRequest.Form["mainPreAuthDate"]);
                // preauths.overseasId = Convert.ToInt32(httpRequest.Form["overseasId"]);
                // preauths.assistantCharges = Convert.ToDecimal(httpRequest.Form["assistantCharges"]);
                // preauths.approvedBy = Convert.ToInt32(httpRequest.Form["approvedBy"]);
                // preauths.approvedDate = Convert.ToDateTime(httpRequest.Form["approvedDate"]);
                // preauths.rejectedBy = Convert.ToInt32(httpRequest.Form["rejectedBy"]);
                // preauths.rejectedDate = Convert.ToDateTime(httpRequest.Form["rejectedDate"]);
                // preauths.assistantFeesCurrencyId = Convert.ToInt32(httpRequest.Form["assistantFeesCurrencyId"]);
                // preauths.panNo = httpRequest.Form["panNo"];
                // preauths.approvedRejectedBy = Convert.ToInt32(httpRequest.Form["approvedRejectedBy"]);
                // preauths.approvedRejectedDate = Convert.ToDateTime(httpRequest.Form["approvedRejectedDate"]);
                // preauths.providerRequestId = Convert.ToInt32(httpRequest.Form["providerRequestId"]);
                // preauths.patientStatusId = Convert.ToInt32(httpRequest.Form["patientStatusId"]);
                // preauths.patientStatus = httpRequest.Form["patientStatus"];
                // preauths.timeOfAdmission = httpRequest.Form["timeOfAdmission"];
                // preauths.timeOfDischarge = httpRequest.Form["timeOfDischarge"];
                // preauths.negotiatedAmount = Convert.ToDecimal(httpRequest.Form["negotiatedAmount"]);
                // preauths.discountAmount = Convert.ToDecimal(httpRequest.Form["discountAmount"]);
                // preauths.amountWithoutDeduction = Convert.ToDecimal(httpRequest.Form["amountWithoutDeduction"]);
                // preauths.claimDate = Convert.ToDateTime(httpRequest.Form["claimDate"]);
                // preauths.claimById = Convert.ToInt32(httpRequest.Form["claimById"]);
                // preauths.dependentCode = Convert.ToInt32(httpRequest.Form["dependentCode"]);
                // preauths.savingsDownloadedDate = Convert.ToDateTime(httpRequest.Form["savingsDownloadedDate"]);
                // preauths.savingsDownloadedBy = Convert.ToInt32(httpRequest.Form["savingsDownloadedBy"]);
                // preauths.revisedAmount = Convert.ToDecimal(httpRequest.Form["revisedAmount"]);
                // preauths.payerId = Convert.ToInt32(httpRequest.Form["payerId"]);
                // preauths.payerName = httpRequest.Form["payerName"];
                // preauths.hospitalEmailId = httpRequest.Form["hospitalEmailId"];
                // preauths.logNumber = httpRequest.Form["logNumber"];
                // preauths.memberEmailId = httpRequest.Form["memberEmailId"];
                // preauths.caseReceiptDate = Convert.ToDateTime(httpRequest.Form["caseReceiptDate"]);
                // preauths.emailRespondDate = Convert.ToDateTime(httpRequest.Form["emailRespondDate"]);
                // preauths.caseRemarks = httpRequest.Form["caseRemarks"];
                // preauths.intimationId = Convert.ToInt32(httpRequest.Form["intimationId"]);
                // preauths.claimBenefitId = Convert.ToInt32(httpRequest.Form["claimBenefitId"]);
                // preauths.productId = Convert.ToInt32(httpRequest.Form["productId"]);
                // preauths.planId = Convert.ToInt32(httpRequest.Form["planId"]);
                // preauths.employeeCode = httpRequest.Form["employeeCode"];
                // preauths.memberId = Convert.ToInt32(httpRequest.Form["memberId"]);
                // preauths.enrollmentId = Convert.ToInt32(httpRequest.Form["enrollmentId"]);
                // preauths.overrideAlert = Convert.ToBoolean(httpRequest.Form["overrideAlert"]);
                // preauths.groupPolicyId = Convert.ToInt32(httpRequest.Form["groupPolicyId"]);
                // preauths.referredToProvider = Convert.ToBoolean(httpRequest.Form["referredToProvider"]);
                // preauths.phmCommentsId = Convert.ToInt32(httpRequest.Form["phmCommentsId"]);
                // preauths.phmCommentsOther = httpRequest.Form["phmCommentsOther"];
                // preauths.branchId = Convert.ToInt32(httpRequest.Form["branchId"]);
                // preauths.phsFIRNo = httpRequest.Form["phsFIRNo"];
                // preauths.ewiseCheck = httpRequest.Form["ewiseCheck"];
                // preauths.ewiseCheckRemarks = httpRequest.Form["ewiseCheckRemarks"];
                // preauths.mouDiscountCheck = httpRequest.Form["mouDiscountCheck"];
                // preauths.mouDiscountCheckRemarks = httpRequest.Form["mouDiscountCheckRemarks"];
                // preauths.hospitalType = httpRequest.Form["hospitalType"];
                // preauths.insuranceStatusId = Convert.ToInt32(httpRequest.Form["insuranceStatusId"]);
                // preauths.loginTypeId = Convert.ToInt32(httpRequest.Form["loginTypeId"]);
                // preauths.tariffDeduction = Convert.ToDecimal(httpRequest.Form["tariffDeduction"]);
                // preauths.hospitalTypeId = Convert.ToInt32(httpRequest.Form["hospitalTypeId"]);
                // preauths.preAuthLogId = Convert.ToInt32(httpRequest.Form["preAuthLogId"]);
                // preauths.loginType = httpRequest.Form["loginType"];
                // preauths.referToInsuranceYesNo = httpRequest.Form["referToInsuranceYesNo"];
                // preauths.rejectedAmount_Copy = Convert.ToDecimal(httpRequest.Form["rejectedAmount_Copy"]);
                // preauths.isDeleted = Convert.ToInt32(httpRequest.Form["isDeleted"]);
                // preauths.deletedDate = Convert.ToDateTime(httpRequest.Form["deletedDate"]);
                // preauths.policyStartDate = Convert.ToDateTime(httpRequest.Form["policyStartDate"]);
                // preauths.policyEndDate = Convert.ToDateTime(httpRequest.Form["policyEndDate"]);
                // preauths.procedureIDs = httpRequest.Form["procedureIDs"];
                // preauths.remarksIDs = httpRequest.Form["remarksIDs"];
                // preauths.fileUploadIDs = httpRequest.Form["fileUploadIDs"];
                // preauths.exclusionIDs = httpRequest.Form["exclusionIDs"];
                // preauths.tempDiagnosisIds = httpRequest.Form["tempDiagnosisIds"];
                // preauths.tempRemarksIds = httpRequest.Form["tempRemarksIds"];
                // preauths.tempNotesId = httpRequest.Form["tempNotesId"];
                // preauths.preAuthRemarksId = httpRequest.Form["preAuthRemarksId"];
                // preauths.isDeficiencyRetrieved = Convert.ToInt32(httpRequest.Form["isDeficiencyRetrieved"]);
                // preauths.medicalOpinionIds = httpRequest.Form["medicalOpinionIds"];
                // preauths.estimatedcategoryid = Convert.ToInt32(httpRequest.Form["estimatedcategoryid"]);
                // preauths.estimatedamount = Convert.ToDecimal(httpRequest.Form["estimatedamount"]);
                // preauths.negotiatedby = Convert.ToInt32(httpRequest.Form["negotiatedby"]);
                // preauths.deduction = Convert.ToDecimal(httpRequest.Form["deduction"]);
                // preauths.medicaldetails = httpRequest.Form["medicaldetails"];
                // preauths.invoicenumber = Convert.ToInt32(httpRequest.Form["invoicenumber"]);
                // preauths.invoiceDate = Convert.ToDateTime(httpRequest.Form["invoiceDate"]);
                // preauths.particulars = Convert.ToInt32(httpRequest.Form["particulars"]);
                // preauths.billRemarks = httpRequest.Form["billRemarks"];
                // preauths.billDeduction = Convert.ToDecimal(httpRequest.Form["billDeduction"]);
                // preauths.status = Convert.ToInt32(httpRequest.Form["status"]);
                //// preauths.docUpload = httpRequest.Form["docUpload"];
                // preauths.docUploadName = httpRequest.Form["docUploadName"];
                // preauths.docDate = Convert.ToDateTime(httpRequest.Form["docDate"]);
                // preauths.docRemarks = httpRequest.Form["docRemarks"];

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

                    // Optionally set file-related properties here in the preauths object
                    // preauths.FilePath = filePath;
                    // preauths.FileName = fileName;
                    // preauths.FileExtension = _ext;
                }

                // Call the DAL method to insert the data
                msg = _preAuthDal.insertPreAuth(preauths);
            }
            catch (Exception ex)
            {
                _commonDal.LogError("InsertPreAuth", "PreAuthController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

            return Ok(new { message = msg });
        }
        //[Route("GetPreAuth")]   //Usp_GetPreAuthDetailsById
        //[HttpGet]
        //public IActionResult GetPreAuth()
        //{
        //    try
        //    {
        //        var preauth = _preAuthDal.getPreAuth();
        //        return Ok(preauth);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Error occurred: " + ex.Message);
        //    }
        //}


        [Route("SearchPreAuth")]   //Usp_GetPreAuthDetailsById
        [HttpPost]
        public IActionResult GetPreAuth(int number)
        {
            try
            {
                PreAuth preauths = new PreAuth();
                var httpRequest = HttpContext.Request;
                preauths.insuranceCompanyId = Convert.ToInt32(httpRequest.Form["insuranceCompanyId"]);
                preauths.claimStatus = httpRequest.Form["claimStatus"];
                preauths.previousPreAuthNo = httpRequest.Form["preAuthNo"];


                var preauth = _preAuthDal.SearchPreAuth(preauths);
                return Ok(preauth);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

    }
}
