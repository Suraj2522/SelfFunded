using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SelfFunded.Controllers
{
    public class BrokerController : Controller
    {
        private readonly BrokerDal _brokerDal;
        CommonDal commondal;
        string ConfigureFilePath;

        public BrokerController(BrokerDal brokerDal, CommonDal common, IConfiguration configuration)
        {
            _brokerDal = brokerDal;
            commondal = common;
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
        }

        [Route("api/Broker/InsertBrokerMaster")]
        [HttpPost]
        public IActionResult InsertBrokerMaster()
        {
            string msg = "";
          
            try
            {
                var httpRequest = HttpContext.Request;

                BrokerMaster brokerMaster = new BrokerMaster
                {
                    brokerName = httpRequest.Form["brokerName"].ToString(),
                    emailId = httpRequest.Form["emailId"].ToString(),
                    businessEmailId = httpRequest.Form["businessEmailId"].ToString(),
                    mobileNo = httpRequest.Form["mobileNo"],
                    officePhone = httpRequest.Form["officePhone"],
                    fax = httpRequest.Form["Fax"],
                    address1 = httpRequest.Form["address1"],
                    address2 = httpRequest.Form["address2"],
                    country = httpRequest.Form["country"],
                    city = httpRequest.Form["city"],
                    state = httpRequest.Form["state"].ToString(),
                    website = httpRequest.Form["website"].ToString(),
                    createdBy = httpRequest.Form["createdBy"].ToString(),
                };

                string _imgname = string.Empty;
                var pic = HttpContext.Request.Form.Files["file"];

                if (pic != null && pic.Length > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);
                    fileName = fileName.Replace(" ", "_");
                    brokerMaster.brokerLogo = fileName;
                    var _comPath = Path.Combine(ConfigureFilePath);
                    if (!Directory.Exists(_comPath))
                    {
                        Directory.CreateDirectory(_comPath);
                    }
                    _comPath = Path.Combine(_comPath, fileName);
                    var path = _comPath;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        pic.CopyTo(fileStream);
                    }
                }

                msg = _brokerDal.insertBroker(brokerMaster);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertBrokerMaster", "BrokerController", ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
            
        }

        [Route("api/Broker/UpdateBrokerMaster")]
        [HttpPut]
        public IActionResult UpdateBrokerMaster(int id)
        {
            string msg = "";
            SqlConnection connection = null;
            try
            {
                var httpRequest = HttpContext.Request;

                BrokerMaster brokerMaster = new BrokerMaster
                {
                    brokerName = httpRequest.Form["brokerName"],
                    emailId = httpRequest.Form["emailId"],
                    businessEmailId = httpRequest.Form["businessEmailId"],
                    mobileNo = httpRequest.Form["mobileNo"],
                    officePhone = httpRequest.Form["officePhone"],
                    fax = httpRequest.Form["fax"],
                    address1 = httpRequest.Form["address1"],
                    address2 = httpRequest.Form["address2"],
                    country = httpRequest.Form["country"],
                    city = httpRequest.Form["city"],
                    state = httpRequest.Form["state"],
                    website = httpRequest.Form["website"],
                    createdBy = httpRequest.Form["createdBy"].ToString()
                };

                string _imgname = string.Empty;
                var pic = HttpContext.Request.Form.Files["file"];

                if (pic != null && pic.Length > 0)
                {
                    var fileName = Path.GetFileName(pic.FileName);
                    var _ext = Path.GetExtension(pic.FileName);
                    fileName = fileName.Replace(" ", "_");
                    brokerMaster.brokerLogo = fileName;
                    var _comPath = Path.Combine(ConfigureFilePath);
                    if (!Directory.Exists(_comPath))
                    {
                        Directory.CreateDirectory(_comPath);
                    }
                    _comPath = Path.Combine(_comPath, fileName);
                    var path = _comPath;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        pic.CopyTo(fileStream);
                    }
                }

                msg = _brokerDal.updateBroker(id, brokerMaster);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("UpdateBrokerMaster", "BrokerController", ex.Message, "");
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

        [Route("api/Broker/GetAllBrokerDetails")]
        [HttpGet]
        public IActionResult GetAllBroker()
        {
            SqlConnection connection = null;
            try
            {
                List<BrokerMaster> brk = new List<BrokerMaster>();
                brk = _brokerDal.getAllBrokerDetails();
                return Ok(brk);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetallBrokerMaster", "BrokerController", ex.Message, "");
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

        [Route("api/Broker/deleteBroker")]
        [HttpPut]
        public IActionResult DeleteBroker(int id, [FromBody] BrokerMaster item)
        {
            SqlConnection connection = null;
            try
            {
                string msg = _brokerDal.deleteBroker(item);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("DeleteBroker", "BrokerController", ex.Message, "");
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
    }
}
