using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SelfFunded.Controllers
{
    //[Route("api/User")]
    // [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDal _userDal;
        private readonly CommonDal commondal;
        string ConfigureFilePath;
        public UserController(UserDal userDal, CommonDal common, IConfiguration configuration)
        {
            _userDal = userDal;
            commondal = common;
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
        }


        //[Route("api/User/GetStatus")]
        //[HttpGet]

        //public List<UserMaster> GetStatus()
        //{
        //    List<UserMaster> list = new List<UserMaster>();
        //    list = _userDal.getstatus();
        //    return list;
        //}


        [Route("api/User/InsertUser")]
        [HttpPost]

        public IActionResult InsertUser()
        {
            string msg = "";
            try
            {
                var httpRequest = HttpContext.Request;
                UserMaster user = new UserMaster();

                user.userCode = Convert.ToInt32(httpRequest.Form["userName"]);
                user.firstName = httpRequest.Form["firstName"];
                user.middleName = httpRequest.Form["middleName"];
                user.lastName = httpRequest.Form["lastName"];
                user.employeeCode = httpRequest.Form["employeeNumber"];
                user.primaryContactNo = httpRequest.Form["ContactNumber"];
                user.secondaryContactNo = httpRequest.Form["SecondaryContactNumber"];
                user.userEmailId = httpRequest.Form["emailId"];
                user.userTypeId = Convert.ToInt32(httpRequest.Form["userType"]);
                user.statusId = Convert.ToInt32(httpRequest.Form["status"]);
                user.corporateId =  Convert.ToInt32(httpRequest.Form["corporateName"]);
                user.createByUserId = 123;//Convert.ToInt32(httpRequest.Form["createdBy"]);
                user.loginTypeId = Convert.ToInt32(httpRequest.Form["loginType"]);
                user.userId = 1;


                var files = httpRequest.Form.Files;
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(ConfigureFilePath, fileName);

                        if (!Directory.Exists(ConfigureFilePath))
                        {
                            Directory.CreateDirectory(ConfigureFilePath);
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        if (file.Name == "file")
                        {
                            user.logoName = fileName;
                        }
                        else if (file.Name == "signature")
                        {
                            user.userSignature = fileName;
                        }
                    }
                }

                msg = _userDal.insertUser(user);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertUser", "UserController", ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }



        //[Route("api/User/UpdateUser")]
        //    [HttpPut]
        //    public IActionResult UpdateUser(int id)
        //    {
        //        string msg = "";
        //        try
        //        {
        //            var httpRequest = HttpContext.Request;

        //            // Assuming "httpRequest" is an instance of HttpRequest
        //            UserMaster user = new UserMaster
        //            {
        //                userType = httpRequest.Form["UserType"],
        //                userName = httpRequest.Form["UserName"],
        //                firstName = httpRequest.Form["FirstName"],
        //                middleName = httpRequest.Form["MiddleName"],
        //                lastName = httpRequest.Form["LastName"],
        //                employeeNumber = httpRequest.Form["EmployeeNumber"],
        //                emailId = httpRequest.Form["EmailId"],
        //                contactNumber = httpRequest.Form["ContactNumber"],
        //                secondaryContactNumber = httpRequest.Form["SecondaryContactNumber"],
        //                loginType = httpRequest.Form["LoginType"],
        //                status = httpRequest.Form["Status"],
        //                corporateName = httpRequest.Form["CorporateName"],
        //                createdBy = httpRequest.Form["createdBy"]
        //            };

        //            string _imgname = string.Empty;
        //            var files = HttpContext.Request.Form.Files;

        //            foreach (var file in files)
        //            {
        //                if (file != null && file.Length > 0)
        //                {
        //                    var fileName = Path.GetFileName(file.FileName);
        //                    var _ext = Path.GetExtension(file.FileName);
        //                    fileName = fileName.Replace(" ", "_");

        //                    var _comPath = Path.Combine(ConfigureFilePath);
        //                    if (!Directory.Exists(_comPath))
        //                    {
        //                        Directory.CreateDirectory(_comPath);
        //                    }

        //                    var path = Path.Combine(_comPath, fileName);
        //                    using (var fileStream = new FileStream(path, FileMode.Create))
        //                    {
        //                        file.CopyTo(fileStream);
        //                    }

        //                    // Assign file name to the corresponding property based on the key
        //                    if (file.Name == "file")
        //                    {
        //                        user.photo = fileName;
        //                    }
        //                    else if (file.Name == "signature")
        //                    {
        //                        user.signatures = fileName;
        //                    }
        //                }
        //            }

        //            msg = _userDal.updateUser(id, user);
        //            return Ok(new { message = msg });
        //        }
        //        catch (Exception ex)
        //        {
        //            commondal.LogError("UpdateUser", "UserController", ex.Message, null);
        //            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
        //        }


        //    }

        [Route("api/User/GetAllUserSearch")]
        [HttpPost]
        // [HttpGet("GetAllUser")]
        public IActionResult GetAllUser()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                UserMaster user = new UserMaster();


                user.userName = httpRequest.Form["userName"];
                user.claimStatus = httpRequest.Form["claimStatus"];

                List<UserMaster> users = new List<UserMaster>();
                users = _userDal.getAllUsers(user);
                return Ok(users);
            }
            catch (Exception ex)
            {
                commondal.LogError("GetallUser", "UserController", ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/User/DeleteUser")]
        [HttpPut]
        public IActionResult DeleteUser(int id, [FromBody] UserMaster item)
        {
            try
            {
                string msg = "";
                msg = _userDal.DeleteUser(id, item);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("DeleteUser", "UserController", ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/User/GetUserType")]
        [HttpGet]
        //[HttpGet("GetUserType")]
        public List<UserMaster> GetUserType()
        {
            return _userDal.getusertype();
        }
    }

}