using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SelfFunded.DAL
{
    public class PolicyPemiumInfoDal
    {
        CommonDal commondal;
        private readonly string conString;

        public PolicyPemiumInfoDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

        public String insertPolicyPremiumInfo(PolicyPremiumInfo polpreminf)
        {
            SqlConnection connection = null;

            try
            {
                int id = 0;
                using ( connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertPolicyPremiumInfo", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyId", polpreminf.policyId);
                    cmd.Parameters.AddWithValue("@PremiumType", polpreminf.premiumType);
                    cmd.Parameters.AddWithValue("@PolicyPremium", polpreminf.policyPremium);
                    cmd.Parameters.AddWithValue("@Addition", polpreminf.addition);
                    cmd.Parameters.AddWithValue("@Deletion", polpreminf.deletion);
                    cmd.Parameters.AddWithValue("@NilEndorsement", polpreminf.nilEndorsement);
                    cmd.Parameters.AddWithValue("@Refund", polpreminf.refund);
                    cmd.Parameters.AddWithValue("@Extra", polpreminf.extra);
                    cmd.Parameters.AddWithValue("@NetPremium", polpreminf.netPremium);                    

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
                commondal.LogError("InsertPolicyPremiumInfo", "PolicyPemiumInfoController", ex.Message, "PolicyPemiumInfoDal");
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

        public String updatePolicyPremiumInfo(int pTId, PolicyPremiumInfo polpreminf)
        {
            SqlConnection connection = null;

            try
            {
                 connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_UpdatePolicyPremiumInfo", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PolicyId", pTId);
                cmd.Parameters.AddWithValue("@PremiumType", polpreminf.premiumType);
                cmd.Parameters.AddWithValue("@PolicyPremium", polpreminf.policyPremium);
                cmd.Parameters.AddWithValue("@Addition", polpreminf.addition);
                cmd.Parameters.AddWithValue("@Deletion", polpreminf.deletion);
                cmd.Parameters.AddWithValue("@NilEndorsement", polpreminf.nilEndorsement);
                cmd.Parameters.AddWithValue("@Refund", polpreminf.refund);
                cmd.Parameters.AddWithValue("@Extra", polpreminf.extra);
                cmd.Parameters.AddWithValue("@NetPremium", polpreminf.netPremium);

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
                commondal.LogError("InsertPolicyMaster", "PolicyPemiumInfoController", ex.Message, "PolicyPemiumInfoDal");
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

        public List<PolicyPremiumInfo> getAllPolicyPremiumInfo()
        {
            List<PolicyPremiumInfo>? polpreminf = new List<PolicyPremiumInfo>();
            SqlConnection connection = null;

            try
            {
                 connection = new SqlConnection(conString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetAllPolicyPremiumInfo", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                connection.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    polpreminf.Add(new PolicyPremiumInfo
                    {
                        srNo = Convert.ToInt32(dr["SRNO"]),
                        policyId = Convert.ToInt32(dr["PolicyId"]),
                        premiumType = Convert.ToInt32(dr["PremiumType"]),
                        policyPremium = dr["PolicyPremium"].ToString(),
                        addition = dr["Addition"].ToString(),
                        deletion = dr["Deletion"].ToString(),
                        nilEndorsement = dr["NilEndorsement"].ToString(),
                        refund = dr["Refund"].ToString(),
                        extra = dr["Extra"].ToString(),
                        netPremium = dr["NetPremium"].ToString(),                        
                    });
                }
                return polpreminf;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllPolicyDetails", "PolicyPemiumInfoController", ex.Message, "PolicyPemiumInfoDal");
                return polpreminf;
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
