using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data.SqlClient;
using System.Security.AccessControl;
using System.Security.Claims;

namespace SelfFunded.Controllers
{
    public class HospitalClaimDetailsController : Controller
    {
        private readonly HospitalClaimDetailsDal _hospitalClaimDetailsDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public HospitalClaimDetailsController(IConfiguration configuration, CommonDal common)
        {
            _hospitalClaimDetailsDal = new HospitalClaimDetailsDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }

        //[Route("api/HospitalClaimDetails/GetHospitalClaimDetails")]
        //[HttpPost]
        //public IActionResult GetHospitalClaimDetails()
        //{
        //    try
        //    {
        //        var httpRequest = HttpContext.Request;
        //        HospitalClaimDetails hospdtls = new HospitalClaimDetails();
        //        hospdtls.insuranceId = Convert.ToInt32(httpRequest.Form["InsuranceId"]);
        //        hospdtls.claimNumber = httpRequest.Form["ClaimNumber"];
        //        hospdtls.insuredName = httpRequest.Form["InsuredName"];
        //        hospdtls.fromDate = httpRequest.Form["fromDate"];
        //        hospdtls.toDate = httpRequest.Form["toDate"];
        //        hospdtls.orderByCol = httpRequest.Form["OrderByCol"];
        //        hospdtls.corporateId = Convert.ToInt32(httpRequest.Form["CorporateId"]);
        //        hospdtls.claimType = httpRequest.Form["ClaimType"];


        //        var report = _hospitalClaimDetailsDal.getHospitalClaimDetails(hospdtls);

        //        return Ok(report);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        commondal.LogError("GetHospitalClaimDetails", "HospitalClaimDetailsController", ex.Message, "");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
        //    }
        //}

        [Route("api/HospitalClaimDetails/GetHospitalClaimDetails")]
        [HttpPost]
        public IActionResult GetHospitalClaimDetails()
        {

            var httpRequest = HttpContext.Request;
            HospitalClaimDetails hospitalClaimDetails = new HospitalClaimDetails();
           
            hospitalClaimDetails.insuranceId= Convert.ToInt32(httpRequest.Form["insuranceCompany"]);
            hospitalClaimDetails.claimNumber = httpRequest.Form["claimNumber"];
            hospitalClaimDetails.insuredName = httpRequest.Form["insuredName"];
            //hospitalClaimDetails.orderByCol = httpRequest.Form[""];
            hospitalClaimDetails.fromDate = httpRequest.Form["fromDate"];
            hospitalClaimDetails.toDate = httpRequest.Form["toDate"];
            hospitalClaimDetails.corporateId = Convert.ToInt32(httpRequest.Form["corporate"]);
            hospitalClaimDetails.claimType = httpRequest.Form["claimTypeId"];
            hospitalClaimDetails.bankName = httpRequest.Form["bankName"];

            try
            {
                var report = _hospitalClaimDetailsDal.getHospitalClaimDetails(hospitalClaimDetails);
                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the exception
                commondal.LogError("GetHospitalClaimDetails", "HospitalClaimDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/HospitalClaimDetails/UpdateHospitalClaimDetails")]
        [HttpPost]
        public IActionResult UpdateHospitalClaimDetails()
        {
            string msg = "";


            try
            {
                var httpRequest = HttpContext.Request;
                HospitalClaimDetails hospitalClaimDetails = new HospitalClaimDetails();
                hospitalClaimDetails.claimType = httpRequest.Form["ClaimId"];
                hospitalClaimDetails.tdsValueBeforeLimit = Convert.ToDecimal(httpRequest.Form["TDSValueBeforeLimit"]);
                hospitalClaimDetails.tdsValueAfterLimit = Convert.ToDecimal(httpRequest.Form["TDSValueAfterLimit"]);
                // hospitalClaimDetails.userId =Convert.ToInt32( httpRequest.Form["ClaimId"]);

                msg = _hospitalClaimDetailsDal.updateDebitDetails(hospitalClaimDetails);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("UpdateHospitalClaimDetails", "HospitalClaimDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

        }
        [Route("api/HospitalClaimDetails/SaveTDSDetails")]
        [HttpPost]
        public IActionResult SaveTDSDetails([FromBody] List<Dictionary<string, object>> hospdtlsList)
        {
            string msg = "";

            try
            {
                foreach (var hospdtls in hospdtlsList)
                {
                    var claimId = hospdtls.ContainsKey("ClaimId") ? hospdtls["ClaimId"].ToString() : "";
                    var userId = 111;//hospdtls.ContainsKey("") ? hospdtls["userId"].ToString() : "";
                    var chequeInTheNameOf = hospdtls.ContainsKey("Cheque_in_the_name_of") ? hospdtls["Cheque_in_the_name_of"].ToString() : "";
                    var benfBankAccNo = hospdtls.ContainsKey("Benf_bank_acc_no") ? hospdtls["Benf_bank_acc_no"].ToString() : "";
                    var ifscCode = hospdtls.ContainsKey("Ifsc_code") ? hospdtls["Ifsc_code"].ToString() : "";
                    var bankName = hospdtls.ContainsKey("Bank_name") ? hospdtls["Bank_name"].ToString() : "";
                    var accountType = hospdtls.ContainsKey("Account_type") ? hospdtls["Account_type"].ToString() : "";



                    HospitalClaimDetails details = new HospitalClaimDetails();
                    details.claimId = Convert.ToInt32(claimId);
                    details.userId = userId;
                    details.chequeInTheNameOf = chequeInTheNameOf;
                    details.benfBankAccNo = benfBankAccNo;
                    details.ifscCode = ifscCode;
                    details.bankName = bankName;
                    details.accountType = accountType;
                    msg = _hospitalClaimDetailsDal.saveTDSDetails(details);
                    if (msg != "Data updated successfully")
                    {
                        return BadRequest(new { message = msg });
                    }
                }

                return Ok(new { message = "Data updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the error and return a server error response
                commondal.LogError("SaveTDSDetails", "HospitalClaimDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
