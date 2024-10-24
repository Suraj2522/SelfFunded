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
    public class InsuranceMasterDal
    {
        private readonly string conString;
        CommonDal commondal;
        public InsuranceMasterDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }


        public String insertInsurance(InsuranceMaster insurance)
        {
            SqlConnection connection = null;

            try
            {
                using (connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("Usp_IUInsuranceMaster", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command
                    cmd.Parameters.AddWithValue("@InsuranceId", insurance.insuranceCompanyId); // This must be supplied
                    cmd.Parameters.AddWithValue("@InsuranceCompany", insurance.insuranceCompany ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@InsuranceCompanyCode", insurance.insuranceCompanyCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IRDA_INSCode", insurance.irdaInsCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@InboundOutbound", insurance.inboundOutbound ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@RegistrationNo", insurance.registrationNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@RegistrationDate", insurance.registrationDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address1", insurance.address1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address2", insurance.address2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CountryID", insurance.country ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@StateId", insurance.state ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CityId", insurance.city ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PinCode", insurance.pinCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContactNo", insurance.contactNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@OfficePhone", insurance.officePhone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Fax", insurance.fax ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EmailId", insurance.emailId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BusinessEmailId", insurance.businessEmailId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Website", insurance.website ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ConcernedPerson", insurance.concernedPerson ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReinsurerGICLiability", insurance.reinsurerGICLiability ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@InsuranceLiability", insurance.insuranceLiability ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsPayee", insurance.isPayee ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AccountId", insurance.accountId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CurrencyId", insurance.currencyId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MinAmount", insurance.minAmount ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaxAmount", insurance.maxAmount ?? (object)DBNull.Value);
                  //  cmd.Parameters.AddWithValue("@RecStatus", insurance.recStatus ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreateByUserId", insurance.createByUserId ?? (object)DBNull.Value);
                  //  cmd.Parameters.AddWithValue("@IsSMSFacility", insurance.isSMSFacility ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsEmailFacility", insurance.isEmailFacility ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsHeadOffice", insurance.isHeadOffice ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HeadOfficeId", insurance.headOfficeId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsAutoGenerateMemberId", insurance.isAutoGenerateMemberId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Unit", insurance.unit ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Location", insurance.location ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ProviderNo", insurance.providerNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@InsuranceLogo", insurance.insuranceLogo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PanNo", insurance.panNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@GSTNo", insurance.gstNo ?? (object)DBNull.Value);

                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (result > 0)
                    {
                        return "Data saved successfully";
                    }
                    else
                    {
                        return "An error occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertInsuranceMaster", "InsuranceController", ex.Message, "InsuranceMasterDal");
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


        public List<InsuranceMaster> getAllInsuranceDetails()
        {
            List<InsuranceMaster> insurance = new List<InsuranceMaster>();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(conString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetAllInsuranceDetails", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                connection.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    insurance.Add(new InsuranceMaster
                    {
                        srNo = dr["SRNO"] != DBNull.Value ? Convert.ToInt32(dr["SRNO"]) : 0,
                        insuranceCompanyId = dr["InsuranceCompanyId"] != DBNull.Value ? Convert.ToInt32(dr["InsuranceCompanyId"]) : 0,
                        insuranceCompany = dr["InsuranceCompany"] != DBNull.Value ? dr["InsuranceCompany"].ToString() : string.Empty,
                        registrationNo = dr["RegistrationNo"] != DBNull.Value ? dr["RegistrationNo"].ToString() : string.Empty,
                        registrationDate = dr["RegistrationDate"] != DBNull.Value ? DateTime.Parse(dr["RegistrationDate"].ToString()) : DateTime.MinValue,
                        emailId = dr["EmailId"] != DBNull.Value ? dr["EmailId"].ToString() : string.Empty,
                        contactNo = dr["ContactNo"] != DBNull.Value ? dr["ContactNo"].ToString() : string.Empty,
                        businessEmailId = dr["BusinessEmailId"] != DBNull.Value ? dr["BusinessEmailId"].ToString() : string.Empty,
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
                        insuranceLogo = dr["InsuranceLogo"] != DBNull.Value ? dr["InsuranceLogo"].ToString() : string.Empty

                    });
                }
                return insurance;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllInsuranceMaster", "InsuranceController", ex.Message, "InuranceMasterDal");
                return insurance;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        //public String updateInsurance(int id, InsuranceMaster insurance)
        //{
        //    SqlConnection connection = null;

        //    try
        //    {
        //        connection = new SqlConnection(conString);
        //        SqlCommand cmd = new SqlCommand("SP_UpdateInsuranceDetails", connection);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@InsuranceId", id);
        //        cmd.Parameters.AddWithValue("@InsuranceName ", insurance.insuranceName);
        //        //  cmd.Parameters.AddWithValue("@LocalName ", insurance.LocalName);
        //        cmd.Parameters.AddWithValue("@Alias", insurance.alias);
        //        cmd.Parameters.AddWithValue("@RegistrationNo", insurance.registrationNo);
        //        cmd.Parameters.AddWithValue("@RegistrationDate", insurance.registrationDate);
        //        cmd.Parameters.AddWithValue("@EmailId", insurance.emailId);
        //        cmd.Parameters.AddWithValue("@BusinessEmailId", insurance.businessEmailId);
        //        cmd.Parameters.AddWithValue("@MobileNo", insurance.mobileNo);
        //        cmd.Parameters.AddWithValue("@OfficePhone ", insurance.officePhone);
        //        cmd.Parameters.AddWithValue("@Fax", insurance.fax);
        //        cmd.Parameters.AddWithValue("@Address1", insurance.address1);
        //        cmd.Parameters.AddWithValue("@Address2", insurance.address2);
        //        cmd.Parameters.AddWithValue("@Country", insurance.country);
        //        cmd.Parameters.AddWithValue("@City", insurance.city);
        //        cmd.Parameters.AddWithValue("@State", insurance.state);
        //        cmd.Parameters.AddWithValue("@Website", insurance.website);
        //        cmd.Parameters.AddWithValue("@InsuranceLogo", insurance.insuranceLogo);
        //        cmd.Parameters.AddWithValue("@CreatedBy", insurance.createdBy);
        //        connection.Open();
        //        int i = cmd.ExecuteNonQuery();
        //        connection.Close();
        //        if (i > 0)
        //        {
        //            return "Data updated successfully";
        //        }
        //        else
        //        {
        //            return "error occured";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        commondal.LogError("UpdateInsuranceMaster", "InsuranceController", ex.Message, "InuranceMasterDal");

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

        public String deleteInsurance(int id, InsuranceMaster insurance)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_DeleteInsuranceDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsuranceCompanyId", id);
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                {
                    return "Data deleted successfully";
                }
                else
                {
                    return "error occured";
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("DeleteInsuranceMaster", "InsuranceController", ex.Message, "InuranceMasterDal");
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