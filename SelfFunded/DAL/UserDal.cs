using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using SelfFunded.Models;
using SelfFunded.DAL;
using System.Drawing;
using System.Security.Policy;

namespace SelfFunded.DAL
{
    public class UserDal
    {
        EncryDecry ed = new EncryDecry();
        RandomPass pass = new RandomPass();
       
        private readonly string conString;
        CommonDal commondal;
        public UserDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

        public string insertUser(UserMaster user)
        {
            SqlConnection connection = null;
            try
            {
                string password = pass.GenerateRandomPassword();
                using (connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("sp_CreateUser", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserCode", user.userCode);
                    cmd.Parameters.AddWithValue("@Password", ed.Encrypt(password));
                    cmd.Parameters.AddWithValue("@FirstName", user.firstName);
                    cmd.Parameters.AddWithValue("@MiddleName", user.middleName);
                    cmd.Parameters.AddWithValue("@LastName", user.lastName);
                    cmd.Parameters.AddWithValue("@EmployeeCode", user.employeeCode);
                    cmd.Parameters.AddWithValue("@PrimaryContactNo", user.primaryContactNo);
                    cmd.Parameters.AddWithValue("@SecondaryContactNo", user.secondaryContactNo);
                    cmd.Parameters.AddWithValue("@EmailId", user.userEmailId);
                    cmd.Parameters.AddWithValue("@UserTypeId", user.userTypeId);
                    cmd.Parameters.AddWithValue("@StatusId", user.statusId);
                    cmd.Parameters.AddWithValue("@CorporateId", user.corporateId);
                    cmd.Parameters.AddWithValue("@LogoName", user.logoName);
                    cmd.Parameters.AddWithValue("@UserId", user.userId);
                    cmd.Parameters.AddWithValue("@UserSignature", user.userSignature);
                    cmd.Parameters.AddWithValue("@LoginTypeId", user.loginTypeId);

                    connection.Open();
                    int id = cmd.ExecuteNonQuery();
                    connection.Close();

                    return id > 0 ? "Data saved successfully" : "Error occurred";
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertUser", "UserDal", ex.Message, "UserDal");
                return "An error occurred while processing the request.";
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        public List<UserMaster> getAllUsers(UserMaster user)
        {
            List<UserMaster> users = new List<UserMaster>();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(conString);
                SqlDataAdapter da = new SqlDataAdapter("[GetUserLoginDetails]", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;
                da.SelectCommand.Parameters.AddWithValue ("@Usersearch",user.userName);
                da.SelectCommand.Parameters.AddWithValue("@RecStatus", user.claimStatus);
                da.SelectCommand.Parameters.AddWithValue("@LoginTypeId", user.loginTypeId=5);
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                connection.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    users.Add(new UserMaster
                    {
                        srNo = dr["SRNO"] != DBNull.Value ? Convert.ToInt32(dr["SRNO"]) : 0,
                        userId = dr["UserId"] != DBNull.Value ? Convert.ToInt32(dr["UserId"]) : 0,
                        userCode = dr["UserCode"] != DBNull.Value ? Convert.ToInt32(dr["UserCode"]) : 0,
                        fullName = dr["FullName"] != DBNull.Value ? dr["FullName"].ToString() : string.Empty,
                       // userType = dr["UserCode"] != DBNull.Value ? dr["UserCode"].ToString() : string.Empty,
                        //roleId = dr["roleId"] != DBNull.Value ? Convert.ToInt32(dr["roleId"]) : 0,
                        firstName = dr["FirstName"] != DBNull.Value ? dr["FirstName"].ToString() : string.Empty,
                        middleName = dr["MiddleName"] != DBNull.Value ? dr["MiddleName"].ToString() : string.Empty,
                        lastName = dr["LastName"] != DBNull.Value ? dr["LastName"].ToString() : string.Empty,
                        employeeCode = dr["EmployeeCode"] != DBNull.Value ? dr["EmployeeCode"].ToString() : string.Empty,
                       // userEmailId = dr["UserEmailId"] != DBNull.Value ? dr["UserEmailId"].ToString() : string.Empty,
                        //primaryContactNo = dr["PrimaryContactNo"] != DBNull.Value ? dr["PrimaryContactNo"].ToString() : string.Empty,
                        //secondaryContactNo = dr["SecondaryContactNo"] != DBNull.Value ? dr["SecondaryContactNo"].ToString() : string.Empty,
                        //loginTypeId = dr["LoginTypeId"] != DBNull.Value ? Convert.ToInt32(dr["LoginTypeId"]) : 0,
                        //status = dr["Status"] != DBNull.Value ? dr["Status"].ToString() : string.Empty,
                        //statusId = dr["StatusID"] != DBNull.Value ? Convert.ToInt32(dr["StatusID"]) : 0,
                        //corporateName = dr["CorporateName"] != DBNull.Value ? dr["CorporateName"].ToString() : string.Empty,
                    //    corporateId = dr["CorporateId"] != DBNull.Value ? Convert.ToInt32(dr["CorporateId"]) : 0,
                    //    logoName = dr["LogoName"] != DBNull.Value ? dr["LogoName"].ToString() : string.Empty,
                    //    userSignature = dr["UserSignature"] != DBNull.Value ? dr["UserSignature"].ToString() : string.Empty
                    //
                    });
                }
                return users;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllUser", "UserController", ex.Message, "UserDal");
                return users;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        //public String updateUser(int id,UserMaster user)
        //{
        //    SqlConnection connection = null;

        //    try
        //    {
        //         connection = new SqlConnection(conString);
        //        SqlCommand cmd = new SqlCommand("SP_UpdateUser", connection);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@UserId", id);
        //        cmd.Parameters.AddWithValue("@UserName", user.userName);
        //        cmd.Parameters.AddWithValue("@UserType", user.userType);
        //        cmd.Parameters.AddWithValue("@FirstName", user.firstName);
        //        cmd.Parameters.AddWithValue("@MiddleName", user.middleName);
        //        cmd.Parameters.AddWithValue("@LastName", user.lastName);
        //        cmd.Parameters.AddWithValue("@EmployeeNumber", user.employeeNumber);
        //        cmd.Parameters.AddWithValue("@EmailId", user.emailId);
        //        cmd.Parameters.AddWithValue("@ContactNumber", user.contactNumber);
        //        // cmd.Parameters.AddWithValue("@CorporateName", user.CorporateId);
        //        cmd.Parameters.AddWithValue("@SecondaryContactNumber", user.secondaryContactNumber);
        //        cmd.Parameters.AddWithValue("@LoginType", user.loginType);
        //        cmd.Parameters.AddWithValue("@Status", user.status);
        //        cmd.Parameters.AddWithValue("@CorporateName", user.corporateName);
        //        cmd.Parameters.AddWithValue("@Photo", user.photo);
        //        cmd.Parameters.AddWithValue("@Signatures", user.signatures);
        //        cmd.Parameters.AddWithValue("@CreatedBY", user.createdBy);

        //        connection.Open();
        //        int i = cmd.ExecuteNonQuery();
        //        connection.Close();
        //        if (i > 0)
        //        {
        //            return "Data updated successfully";
        //        }
        //        else
        //        {
        //            return "eror occured";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        commondal.LogError("UpdateUser", "UserController", ex.Message,"UserDal");
        //        return "An error occurred while processing the request.";
        //    }
        //    finally
        //    {
        //        if (connection != null && connection.State == ConnectionState.Open)
        //        {
        //            connection.Close();
        //        }
        //    }
        //}


        public string DeleteUser(int id, UserMaster item)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_DeleteUser", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserCode", id);


                connection.Open();
                int i = cmd.ExecuteNonQuery();

                if (i > 0)
                {
                    return "Data deleted successfully";
                }
                else
                {
                    return "No rows affected"; // Adjust this message as per your application logic
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("DeleteUser", "UserDal", ex.Message, "");
                return "An error occurred while processing the request.";
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }



        //public List<UserMaster> getstatus()
        //{
        //    List<UserMaster> user = new List<UserMaster>();
        //    SqlConnection connection = new SqlConnection(conString);
        //    SqlDataAdapter da = new SqlDataAdapter("[GetStatus]", connection);
        //    da.SelectCommand.CommandType = CommandType.StoredProcedure;
        //    DataTable dt = new DataTable();
        //    connection.Open();
        //    da.Fill(dt);
        //    connection.Close();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        user.Add(new UserMaster
        //        {
        //            status = dr["Name"].ToString(),
        //            statusID = Convert.ToInt32(dr["StatusID"]),
        //        });
        //    }
        //    return user;
        //}

        public List<UserMaster> getusertype()
        {
            List<UserMaster> user = new List<UserMaster>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[GetUserType]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            connection.Open();
            da.Fill(dt);
            connection.Close();
            foreach (DataRow dr in dt.Rows)
            {
                user.Add(new UserMaster
                {
                    roleType = dr["RoleType"].ToString(),
                    roleId = Convert.ToInt32(dr["RoleId"]),
                });
            }
            return user;
        }

    }
}