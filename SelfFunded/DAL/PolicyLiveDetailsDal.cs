using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SelfFunded.DAL
{
    public class PolicyLiveDetailsDal
    {
        CommonDal commondal;
        private readonly string conString;

        public PolicyLiveDetailsDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

      

        public String insertPolicyLiveDetails(PolicyLiveDetails pollivedtls)
        {
            SqlConnection connection = null;

            try
            {
                int id = 0;
                using ( connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertPolicyLiveDtls", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyId", pollivedtls.policyId);
                    cmd.Parameters.AddWithValue("@AtPolicyInception", pollivedtls.atPolicyInception);                    
                    cmd.Parameters.AddWithValue("@Addition", pollivedtls.addition);
                    cmd.Parameters.AddWithValue("@Deletion", pollivedtls.deletion);
                    cmd.Parameters.AddWithValue("@LiveActive", pollivedtls.liveActive);                   

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
                commondal.LogError("InsertPolicyLiveDetails", "PolicyLiveDetailsController", ex.Message, "PolicyLiveDetailsDal");
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

        public String updatePolicyLiveDetails(int policyid, PolicyLiveDetails pollivedtls)
        {
            SqlConnection connection = null;

            try
            {
                 connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_UpdatePolicyLiveDtls", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Policyid", policyid);
                cmd.Parameters.AddWithValue("@AtPolicyInception", pollivedtls.atPolicyInception);                
                cmd.Parameters.AddWithValue("@Addition", pollivedtls.addition);
                cmd.Parameters.AddWithValue("@Deletion", pollivedtls.deletion);
                cmd.Parameters.AddWithValue("@LiveActive", pollivedtls.liveActive);                

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
                commondal.LogError("UpdatePolicyLiveDetails", "PolicyLiveDetailsController", ex.Message, "PolicyLiveDetailsDal");
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

        public List<PolicyLiveDetails> getAllPolicyLiveDetails()
        {
            List<PolicyLiveDetails>? pollivedtls = new List<PolicyLiveDetails>();
            SqlConnection connection = null;

            try
            {
                 connection = new SqlConnection(conString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetAllPolicyLiveDtls", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                connection.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    pollivedtls.Add(new PolicyLiveDetails
                    {
                        srNo = Convert.ToInt32(dr["SRNO"]),
                        policyId = Convert.ToInt32(dr["PolicyId"]),
                        atPolicyInception = dr["AtPolicyInception"].ToString(),                       
                        addition = dr["Addition"].ToString(),
                        deletion = dr["Deletion"].ToString(),
                        liveActive = dr["LiveActive"].ToString(),                       
                    });
                }
                return pollivedtls;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllPolicyLiveDetails", "PolicyLiveDetailsController", ex.Message, "PolicyLiveDetailsDal");
                return pollivedtls;
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
