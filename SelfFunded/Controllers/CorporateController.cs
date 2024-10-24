using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Security.AccessControl;

namespace SelfFunded.Controllers
{

    public class CorporateController : ControllerBase
    {
        private readonly CorporateDal _cdal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;



        public CorporateController(CorporateDal cdal, CommonDal common, IConfiguration configuration)
        {
            _cdal = cdal;
            commondal = common;
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
        }
      

        [Route("api/Corporate/InsertCorporateMaster")]
        [HttpPost]
        public IActionResult InsertCorporateMaster()
        {
            string msg = "";
            try
            {
                var httpRequest = HttpContext.Request;
                CorporateMaster corporate = new CorporateMaster
                {
                    corporateId = Convert.ToInt32(httpRequest.Form["CorporateId"]),
                    industryId = Convert.ToInt32(httpRequest.Form["Industry"]),
                    insuranceCompanyId = Convert.ToInt32(httpRequest.Form["InsuranceName"]),
                    corporateName = httpRequest.Form["Corporate"],
                    alias = httpRequest.Form["Alias"],
                    numberOfEmployees = Convert.ToInt32(httpRequest.Form["NumberOfEmployees"]),
                    emailID = httpRequest.Form["EmailID"],
                    businessEmailID = httpRequest.Form["BusinessEmailID"],
                    contactNo = httpRequest.Form["MobileNo"],
                    officePhone = httpRequest.Form["OfficePhone"],
                    fax = httpRequest.Form["Fax"],
                    address1 = httpRequest.Form["Address1"],
                    address2 = httpRequest.Form["Address2"],
                    countryID = Convert.ToInt32(httpRequest.Form["Country"]),
                    cityID = Convert.ToInt32(httpRequest.Form["City"]),
                    stateID = Convert.ToInt32(httpRequest.Form["State"]),
                    website = httpRequest.Form["Website"],
                    createByUserId = 1,// httpRequest.Form["createdBy"],
                    isSelfFunded = httpRequest.Form["isSelfFunded"]
                };

                // Handle file upload
                var pic = httpRequest.Form.Files["CorporateLogo"];
                if (pic.Length > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);
                    fileName = fileName.Replace(" ", "_");

                    var _comPath = Path.Combine(ConfigureFilePath);
                    if (!Directory.Exists(_comPath))
                    {
                        Directory.CreateDirectory(_comPath);
                    }

                    var path = Path.Combine(_comPath, fileName);
                    pic.CopyTo(new FileStream(path, FileMode.Create));

                    corporate.corporateLogoName = fileName;
                }

                msg = _cdal.InsertCorporateMaster(corporate);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertCorporateMaster", "CorporateController", ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

        }

        //[Route("api/Corporate/UpdateCorporate")]
        //[HttpPut]
        //public IActionResult UpdateCorporate(int id)
        //{
        //    string msg = "";
        //    try
        //    {
        //        var httpRequest = HttpContext.Request;

        //        CorporateMaster corporate = new CorporateMaster
        //        {
        //            industry = httpRequest.Form["Industry"],
        //            insuranceName = httpRequest.Form["InsuranceName"],
        //            corporateName = httpRequest.Form["Corporate"],
        //            alias = httpRequest.Form["Alias"],
        //            numberOfEmployees = Convert.ToInt32(httpRequest.Form["NumberOfEmployees"]),
        //            emailID = httpRequest.Form["EmailID"],
        //            businessEmailID = httpRequest.Form["BusinessEmailID"],
        //            contactNo = httpRequest.Form["MobileNo"],
        //            officePhone = httpRequest.Form["OfficePhone"],
        //            fax = httpRequest.Form["Fax"],
        //            address1 = httpRequest.Form["Address1"],
        //            address2 = httpRequest.Form["Address2"],
        //            country = httpRequest.Form["Country"],
        //            city = httpRequest.Form["City"],
        //            state = httpRequest.Form["State"],
        //            website = httpRequest.Form["Website"],
        //            createByUserId = httpRequest.Form["createdBy"]
        //        };

        //        // Handle file upload
        //        var pic = httpRequest.Form.Files["CorporateLogo"];
        //        if (pic.Length > 0)
        //        {
        //            var fileName = Path.GetFileName(pic.FileName);
        //            var _ext = Path.GetExtension(pic.FileName);
        //            fileName = fileName.Replace(" ", "_");

        //            var _comPath = Path.Combine(ConfigureFilePath);
        //            if (!Directory.Exists(_comPath))
        //            {
        //                Directory.CreateDirectory(_comPath);
        //            }

        //            var path = Path.Combine(_comPath, fileName);
        //            pic.CopyTo(new FileStream(path, FileMode.Create));

        //            corporate.corporateLogoName = fileName;
        //        }

        //        msg = _cdal.UpdateCorporate(id, corporate);
        //        return Ok(new { message = msg });

        //    }
        //    catch (Exception ex)
        //    {
        //        commondal.LogError("UpdateCorporateMaster", "CorporateController", ex.Message, null);
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
        //    }

        //}
        [Route("api/Corporate/GetAllCorporate")]
        [HttpGet]
        public IActionResult GetAllCorporate()
        {
            try
            {
                List<CorporateMaster> brk = new List<CorporateMaster>();
                brk = _cdal.GetAllCorporateMasters();
                return Ok(brk);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllCorporateMaster", "CorporateController", ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/Corporate/DeleteCorporate")]
        [HttpPut]
        public IActionResult DeleteCorporate(int id, [FromBody] CorporateMaster master)
        {
            try
            {
                string msg = _cdal.DeleteCorporate(id, master);

                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("DeleteCorporateMaster", "CorporateController", ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

        }
    }
}
