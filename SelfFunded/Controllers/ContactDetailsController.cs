using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;

namespace SelfFunded.Controllers
{
    public class ContactDetailsController : Controller
    {
        private readonly ContactDetailsDal _contactDetailsDal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;
        public ContactDetailsController(ContactDetailsDal contactDetailsDal, CommonDal common, IConfiguration configuration)
        {
            _contactDetailsDal = contactDetailsDal;
            commondal = common;
        }

       

        [Route("api/ContactDetails/InsertContactDetails")]
        [HttpPost]
        public IActionResult InsertContactDetails([FromBody] ContactDetails contdtls)
        {
            string msg = "";
            try
            {
                msg = _contactDetailsDal.insertContactDetails(contdtls);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertContactDetails", "ContactDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/ContactDetails/UpdateContactDetails/{id}")]
        [HttpPut]
        public IActionResult UpdateContactDetails(int id, [FromBody] ContactDetails contdtls)
        {
            string msg = "";
            try
            {
                msg = _contactDetailsDal.updateContactDetails(id, contdtls);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                    commondal.LogError("UpdateContactDetails", "ContactDetailsController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
