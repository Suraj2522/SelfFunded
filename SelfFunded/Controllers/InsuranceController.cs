using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;

namespace SelfFunded.Controllers
{

    public class InsuranceController : Controller
    {
        private readonly InsuranceMasterDal insurance_dal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;

        public InsuranceController(InsuranceMasterDal dal,CommonDal common, IConfiguration configuration )
        {
            insurance_dal = dal;
            commondal = common;
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
        }
       

        [Route("api/Insurance/InsertInsuranceMaster")]
        [HttpPost]
        public IActionResult InsertInsuranceMaster()
        {
            String msg = "";
            HttpResponseMessage response = new HttpResponseMessage();
            InsuranceMaster insuranceMaster = new InsuranceMaster();
            try
            {
                var httpRequest = HttpContext.Request;
                insuranceMaster.insuranceCompanyId =Convert.ToInt32( httpRequest.Form["insuranceCompanyId"]);
                insuranceMaster.insuranceCompany = httpRequest.Form["InsuranceName"];
                insuranceMaster.registrationNo = httpRequest.Form["RegistrationNo"];
                insuranceMaster.registrationDate = Convert.ToDateTime(httpRequest.Form["RegistrationDate"]);
                insuranceMaster.emailId = httpRequest.Form["EmailId"];
                insuranceMaster.businessEmailId = httpRequest.Form["BusinessEmailId"];
                insuranceMaster.contactNo = httpRequest.Form["MobileNo"];
                insuranceMaster.officePhone = httpRequest.Form["OfficePhone"];
                insuranceMaster.fax = httpRequest.Form["Fax"];
                insuranceMaster.address1 = httpRequest.Form["Address1"];
                insuranceMaster.address2 = httpRequest.Form["Address2"];
                insuranceMaster.country = httpRequest.Form["Country"];
                insuranceMaster.city = httpRequest.Form["City"];
                insuranceMaster.state = httpRequest.Form["State"];
                insuranceMaster.website = httpRequest.Form["Website"];
                insuranceMaster.createByUserId = 1;//httpRequest.Form["createdBy"];


                string _imgname = string.Empty;
                var pic = HttpContext.Request.Form.Files["file"];
                if (pic != null && pic.Length > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);
                    fileName = fileName.Replace(" ", "_");
                    insuranceMaster.insuranceLogo = fileName;
                    var _comPath = Path.Combine(ConfigureFilePath);
                    if (!Directory.Exists(_comPath))
                    {
                        Directory.CreateDirectory(_comPath);
                    }
                    _comPath = _comPath + fileName;
                    var path = _comPath;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        pic.CopyTo(fileStream);
                    }
                }
                msg = insurance_dal.insertInsurance(insuranceMaster);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertInsuranceMaster", "InsuranceController", ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");

            }

        }



        [Route("api/Insurance/GetAllInsuranceDetails")]
        [HttpGet]
        public IActionResult GetAllInsurance()
        {
            List<InsuranceMaster> ins = new List<InsuranceMaster>();
            try
            {
                ins = insurance_dal.getAllInsuranceDetails();
                return Ok(ins);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllInsurance", "InsuranceController", ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");

            }

        }

        //  [Route("api/Insurance/UpdateInsurance")]
        //[HttpPut]
        //public IActionResult UpdateInsurance(int id)
        //{
        //    String msg = "";
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    InsuranceMaster insuranceMaster = new InsuranceMaster();
        //    try
        //    {
        //        var httpRequest = HttpContext.Request;

        //        insuranceMaster.insuranceName = httpRequest.Form["InsuranceName"];
        //        insuranceMaster.localName = httpRequest.Form["LocalName"];
        //        insuranceMaster.alias = httpRequest.Form["Alias"];
        //        insuranceMaster.registrationNo = httpRequest.Form["RegistrationNo"];
        //        insuranceMaster.registrationDate = Convert.ToDateTime(httpRequest.Form["RegistrationDate"]);
        //        insuranceMaster.emailId = httpRequest.Form["EmailId"];
        //        insuranceMaster.businessEmailId = httpRequest.Form["BusinessEmailId"];
        //        insuranceMaster.mobileNo = httpRequest.Form["MobileNo"];
        //        insuranceMaster.officePhone = httpRequest.Form["OfficePhone"];
        //        insuranceMaster.fax = httpRequest.Form["Fax"];
        //        insuranceMaster.address1 = httpRequest.Form["Address1"];
        //        insuranceMaster.address2 = httpRequest.Form["Address2"];
        //        insuranceMaster.country = httpRequest.Form["Country"];
        //        insuranceMaster.city = httpRequest.Form["City"];
        //        insuranceMaster.state = httpRequest.Form["State"];
        //        insuranceMaster.website = httpRequest.Form["Website"];
        //        insuranceMaster.createdBy = httpRequest.Form["createdBy"];

        //        string _imgname = string.Empty;
        //        var pic = HttpContext.Request.Form.Files["file"];

        //        if (pic != null && pic.Length > 0)
        //        {
        //            var fileName = Path.GetFileName(pic.FileName);
        //            var _ext = Path.GetExtension(pic.FileName);
        //            fileName = fileName.Replace(" ", "_");
        //            insuranceMaster.insuranceLogo = fileName;
        //            var _comPath = Path.Combine(ConfigureFilePath);
        //            if (!Directory.Exists(_comPath))
        //            {
        //                Directory.CreateDirectory(_comPath);
        //            }
        //            _comPath = _comPath + fileName;
        //            var path = _comPath;
        //            using (var fileStream = new FileStream(path, FileMode.Create))
        //            {
        //                pic.CopyTo(fileStream);
        //            }
        //        }
        //        msg = insurance_dal.updateInsurance(id, insuranceMaster);
        //        return Ok(new { message = msg });
        //    }
        //    catch (Exception ex)
        //    {
        //        commondal.LogError("GetallBrokerMaster", "BrokerController", ex.Message, "");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");

        //    }
        //}

        [Route("api/Insurance/DeleteInsurance")]
        [HttpPut]
        public IActionResult DeleteInsurance(int id, InsuranceMaster insurance)
        {
            try
            {
                String msg = "";
                {
                    msg = insurance_dal.deleteInsurance(id, insurance);
                }
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("DeleteInsurance", "InsuranceController", ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");


            }
        }
    }
}
