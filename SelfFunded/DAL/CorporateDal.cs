using SelfFunded.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Linq.Expressions;
using System.Security.AccessControl;

namespace SelfFunded.DAL
{
    public class CorporateDal
    {

        EncryDecry ed = new EncryDecry();
        RandomPass pass = new RandomPass();

        private readonly string conString;
        CommonDal commondal;

        public CorporateDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

        public string InsertCorporateMaster(CorporateMaster corporate)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("Usp_IUCorporateMaster", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CorporateId", corporate.corporateId);

                    cmd.Parameters.AddWithValue("@IndustryId", corporate.industryId);
                    cmd.Parameters.AddWithValue("@InsuranceId", corporate.insuranceCompanyId);
                    cmd.Parameters.AddWithValue("@Corporate", corporate.corporateName);
                 //   cmd.Parameters.AddWithValue("@Alias", corporate.alias);
                    cmd.Parameters.AddWithValue("@NumberOfEmployees", corporate.numberOfEmployees);
                    cmd.Parameters.AddWithValue("@EmailID", corporate.emailID);
                    cmd.Parameters.AddWithValue("@BusinessEmailID", corporate.businessEmailID);
                    cmd.Parameters.AddWithValue("@MobileNo", corporate.contactNo);
                    cmd.Parameters.AddWithValue("@OfficePhone", corporate.officePhone);
                    cmd.Parameters.AddWithValue("@Fax", corporate.fax);
                    cmd.Parameters.AddWithValue("@Address1", corporate.address1);
                    cmd.Parameters.AddWithValue("@Address2", corporate.address2);
                    cmd.Parameters.AddWithValue("@Country", corporate.countryID);
                    cmd.Parameters.AddWithValue("@City", corporate.cityID);
                    cmd.Parameters.AddWithValue("@State", corporate.stateID);
                    cmd.Parameters.AddWithValue("@Website", corporate.website);
                    cmd.Parameters.AddWithValue("@CreatedBy", corporate.createByUserId);
                    cmd.Parameters.AddWithValue("@IsSelfFunded", corporate.isSelfFunded);
                    cmd.Parameters.AddWithValue("@CorporateLogoName", corporate.corporateLogoName); // Adjust if necessary

                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    connection.Close();

                    return result > 0 ? "Data saved successfully" : "Error occurred";
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertCorporateMaster", "CorporateController", ex.Message, "CorporateDal");
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


        public List<CorporateMaster> GetAllCorporateMasters()
        {
            List<CorporateMaster> corporateMasters = new List<CorporateMaster>();
            SqlConnection connection=null;
            try
            {
                using ( connection = new SqlConnection(conString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SP_GetAllCorporateMaster", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    connection.Open();
                    da.Fill(dt);
                    connection.Close();
                    foreach (DataRow dr in dt.Rows)
                    {
                        corporateMasters.Add(new CorporateMaster
                        {
                            srNo = dr["SRNO"] != DBNull.Value ? Convert.ToInt32(dr["SRNO"]) : 0,
                            corporateId = dr["CorporateId"] != DBNull.Value ? Convert.ToInt32(dr["CorporateId"].ToString()) : 0,
                            industry = dr["Industry"] != DBNull.Value ? dr["Industry"].ToString() : string.Empty,
                            industryId = dr["IndustryId"] != DBNull.Value ? Convert.ToInt32(dr["IndustryId"]) : 0,
                            insuranceName = dr["InsuranceName"] != DBNull.Value ? dr["InsuranceName"].ToString() : string.Empty,
                            insuranceCompanyId = dr["InsuranceId"] != DBNull.Value ? Convert.ToInt32(dr["InsuranceId"]) : 0,
                            corporateName = dr["CorporateName"] != DBNull.Value ? dr["CorporateName"].ToString() : string.Empty,
                            // localName = dr["LocalName"] != DBNull.Value ? dr["LocalName"].ToString() : string.Empty,
                            // alias = dr["Alias"] != DBNull.Value ? dr["Alias"].ToString() : string.Empty,
                            numberOfEmployees = dr["NumberOfEmployees"] != DBNull.Value ? Convert.ToInt32(dr["NumberOfEmployees"]) : 0,
                            emailID = dr["EmailID"] != DBNull.Value ? dr["EmailID"].ToString() : string.Empty,
                            businessEmailID = dr["BusinessEmailID"] != DBNull.Value ? dr["BusinessEmailID"].ToString() : string.Empty,
                            contactNo = dr["ContactNo"] != DBNull.Value ? dr["ContactNo"].ToString() : string.Empty,
                            officePhone = dr["OfficePhone"] != DBNull.Value ? dr["OfficePhone"].ToString() : string.Empty,
                            fax = dr["Fax"] != DBNull.Value ? dr["Fax"].ToString() : string.Empty,
                            address1 = dr["Address1"] != DBNull.Value ? dr["Address1"].ToString() : string.Empty,
                            address2 = dr["Address2"] != DBNull.Value ? dr["Address2"].ToString() : string.Empty,
                            country = dr["Country"] != DBNull.Value ? dr["Country"].ToString() : string.Empty,
                            countryID = dr["CountryID"] != DBNull.Value ? Convert.ToInt32(dr["CountryID"]) : 0,
                            city = dr["City"] != DBNull.Value ? dr["City"].ToString() : string.Empty,
                            cityID = dr["CityID"] != DBNull.Value ? Convert.ToInt32(dr["CityID"]) : 0,
                            state = dr["State"] != DBNull.Value ? dr["State"].ToString() : string.Empty,
                            stateID = dr["StateID"] != DBNull.Value ? Convert.ToInt32(dr["StateID"]) : 0,
                            website = dr["Website"] != DBNull.Value ? dr["Website"].ToString() : string.Empty,
                            corporateLogoName = dr["CorporateLogoName"] != DBNull.Value ? dr["CorporateLogoName"].ToString() : string.Empty

                        });
                    }
                }
                return corporateMasters;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllCorporateMasters", "CorporateController", ex.Message, "CorporateDal");
                return corporateMasters;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        //public string UpdateCorporate(int id, CorporateMaster corporate)
        //{
        //    SqlConnection connection = null;

        //    try
        //    {
        //        using ( connection = new SqlConnection(conString))
        //        {
        //            SqlCommand cmd = new SqlCommand("SP_UpdateCorporateMaster", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@CorporateId", id);
        //            cmd.Parameters.AddWithValue("@IndustryId", corporate.industry);
        //            cmd.Parameters.AddWithValue("@InsuranceId", corporate.insuranceName);
        //            cmd.Parameters.AddWithValue("@Corporate", corporate.corporateName);
        //            // cmd.Parameters.AddWithValue("@LocalName", corporate.LocalName);
        //            //  cmd.Parameters.AddWithValue("@Alias", corporate.Alias);
        //            cmd.Parameters.AddWithValue("@NumberOfEmployees", corporate.numberOfEmployees);
        //            cmd.Parameters.AddWithValue("@EmailID", corporate.emailID);
        //            cmd.Parameters.AddWithValue("@BusinessEmailID", corporate.businessEmailID);
        //            cmd.Parameters.AddWithValue("@MobileNo", corporate.contactNo);
        //            cmd.Parameters.AddWithValue("@OfficePhone", corporate.officePhone);
        //            cmd.Parameters.AddWithValue("@Fax", corporate.fax);
        //            cmd.Parameters.AddWithValue("@Address1", corporate.address1);
        //            cmd.Parameters.AddWithValue("@Address2", corporate.address2);
        //            cmd.Parameters.AddWithValue("@Country", corporate.country);
        //            cmd.Parameters.AddWithValue("@City", corporate.city);
        //            cmd.Parameters.AddWithValue("@State", corporate.state);
        //            cmd.Parameters.AddWithValue("@Website", corporate.website);
        //            cmd.Parameters.AddWithValue("@CreatedBY", corporate.createByUserId);

        //            // cmd.Parameters.AddWithValue("@FeeType", corporate.FeeType);
        //            // cmd.Parameters.AddWithValue("@TPAFees", corporate.TPAFees);
        //            cmd.Parameters.AddWithValue("@CorporateLogoName", corporate.corporateLogoName);
        //            connection.Open();
        //            int i = cmd.ExecuteNonQuery();
        //            connection.Close();
        //            if (i > 0)
        //            {
        //                return "Data updated successfully";
        //            }
        //            else
        //            {
        //                return "error occured";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        commondal.LogError("UpdateCorporate", "CorporateController", ex.Message, "CorporateDal");

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

        public string DeleteCorporate(int id, CorporateMaster master)
        {
            SqlConnection connection = null;

            try
            {
                using (connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("SP_DeleteCorporate", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CorporateId", id);
                    connection.Open();
                    int i = cmd.ExecuteNonQuery();
                    connection.Close();
                    if (i > 0)
                    {
                        return "Data deleted successfully";
                    }
                    else
                    {
                        return "Error occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("DeleteCorporate", "CorporateController", ex.Message, "CorporateDal");

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
    }
}