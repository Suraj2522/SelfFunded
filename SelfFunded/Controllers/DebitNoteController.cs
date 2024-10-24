using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Claims;

namespace SelfFunded.Controllers
{
    public class DebitNoteController : Controller
    {
        private readonly DebitNoteDal _debitNoteDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        public DebitNoteController(IConfiguration configuration, CommonDal common)
        {
            _debitNoteDal = new DebitNoteDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");

        }




        [Route("api/DebitNote/GetDebitNoteSearch")]
        [HttpPost]
        public IActionResult GetDebitNoteSearch()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                DebitNote dbtnote = new DebitNote();
                dbtnote.insuranceID = Convert.ToInt32(httpRequest.Form["insuranceCompany"]);
                dbtnote.planId = Convert.ToInt32(httpRequest.Form["plan"]);
                dbtnote.claimTypeId = Convert.ToInt32(httpRequest.Form["claimTypeId"]);
                dbtnote.fromDate = httpRequest.Form["fromDate"];
                dbtnote.toDate = httpRequest.Form["toDate"];
                dbtnote.claimNumber = httpRequest.Form["claimNumber"];


                var report = _debitNoteDal.getDebitNoteSearch(dbtnote);

                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the exception
                commondal.LogError("GetDebitNoteSearch", "DebitNoteController", ex.Message, "DebitNoteDal");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/DebitNote/GenerateDebitNote")]
        [HttpPost]
        public IActionResult GenerateDebitNote()
        {
            string msg = "";
            try
            {
                var httpRequest = HttpContext.Request;
                DebitNote debitnote = new DebitNote();
               
                //debitnote.userId=Convert.ToInt32( httpRequest.Form["UserId"]);
                debitnote.insuranceID= Convert.ToInt32(httpRequest.Form["InsuranceCompanyId"]);
                debitnote.claimTypeId= Convert.ToInt32(httpRequest.Form["ClaimTypeId"]);
                debitnote.planId= Convert.ToInt32(httpRequest.Form["Plan"]);
                debitnote.numberOfClaims= Convert.ToInt32(httpRequest.Form["NoOfClaims"]);
                //debitnote.accountId = Convert.ToInt32(httpRequest.Form["AccountId"]);
                debitnote.claimIds= httpRequest.Form["ClaimId"];
                debitnote.netAmount =Convert.ToDecimal(httpRequest.Form["NetAmount"]);
                msg = _debitNoteDal.generateDebitNote(debitnote);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("GetDebitNoteSearch", "DebitNoteController", ex.Message, "DebitNoteDal");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
