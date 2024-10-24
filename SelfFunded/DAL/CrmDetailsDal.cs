using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SelfFunded.DAL
{
    public class CrmDetailsDal
    {
        CommonDal commondal;
        private readonly string conString;

        public CrmDetailsDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

       

        public String insertCrmDetails(CrmDetails crmdtls)
        {
            int id = 0;
            SqlConnection connection = null;

            try
            {
               
                using ( connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertCrmDetails", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyId", crmdtls.policyId);
                    cmd.Parameters.AddWithValue("@CrmServicingLocation", crmdtls.crmServicingLocation);
                    cmd.Parameters.AddWithValue("@CrmVerticalHead", crmdtls.crmVerticalHead);
                    cmd.Parameters.AddWithValue("@MobileNo", crmdtls.mobileNo);
                    cmd.Parameters.AddWithValue("@EmailId", crmdtls.emailId);
                    cmd.Parameters.AddWithValue("@BrokerName", crmdtls.brokerName);

                    connection.Open();
                    id = cmd.ExecuteNonQuery();
                    connection.Close();
                }
                if (id > 0)
                {
                    return "Data saved successfully";
                }
                else
                {
                    return "error occured";
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertCrmDetails", "CrmDetailsController", ex.Message, "CRMDetailsDal");
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

        public String updateCrmDetails(int policyId, CrmDetails crmdtls)
        {
            SqlConnection connection = null;
            try
            {
                 connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_UpdateCrmDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PolicyId", crmdtls.policyId);
                cmd.Parameters.AddWithValue("@CrmServicingLocation", crmdtls.crmServicingLocation);
                cmd.Parameters.AddWithValue("@CrmVerticalHead", crmdtls.crmVerticalHead);
                cmd.Parameters.AddWithValue("@MobileNo", crmdtls.mobileNo);
                cmd.Parameters.AddWithValue("@EmailId", crmdtls.emailId);
                cmd.Parameters.AddWithValue("@BrokerName", crmdtls.brokerName);

                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                {
                    return "Data updated successfully";
                }
                else
                {
                    return "error occured";
                }

            }
            catch (Exception ex)
            {
                commondal.LogError("updateCrmDetails", "CrmDetailsController", ex.Message, "CRMDetailsDal");
                return "An error occurred while processing the request.";
            }
        }

        public List<CrmDetails> getCrmDetails()
        {
            List<CrmDetails>? crmdtls = new List<CrmDetails>();
            SqlConnection connection = null;
         

            try
            {
                connection = new SqlConnection(conString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetCrmDetails", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                connection.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    crmdtls.Add(new CrmDetails
                    {
                        srNo = Convert.ToInt32(dr["SRNO"]),
                        policyId = Convert.ToInt32(dr["PolicyId"]),
                        crmServicingLocation = dr["CrmServicingLocation"].ToString(),
                        crmVerticalHead = dr["CrmVerticalHead"].ToString(),
                        mobileNo = dr["MobileNo"].ToString(),
                        emailId = dr["EmailId"].ToString(),
                        brokerName = dr["BrokerName"].ToString(),
                    });
                }
                return crmdtls;
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertCrmDetails", "CrmDetailsController", ex.Message, "CRMDetailsDal");
                return crmdtls;
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
