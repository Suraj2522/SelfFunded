using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SelfFunded.DAL
{
    public class PlanDetailsDal
    {
        CommonDal commondal;
        private readonly string conString;

        public PlanDetailsDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }


        public String insertPlanDetails(PlanDetails plandtls)
        {
            SqlConnection connection = null;

            try
            {
                using ( connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertPlanDetails", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyId", plandtls.policyNo);
                    cmd.Parameters.AddWithValue("@PlanCodeExternal", plandtls.planCodeExternal);
                    cmd.Parameters.AddWithValue("@PlanDescription", plandtls.planDescription);
                    cmd.Parameters.AddWithValue("@CopayBuyBack", plandtls.copayBuyBack);

                    connection.Open();
                    int id = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (id > 0)
                    {
                        return "Data saved successfully";
                    }
                    else
                    {
                        return "error occured";
                    }
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertPlanDetails", "PlanDetailsController", ex.Message, "PlanDetailsDal");
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

        public String updatePlanDetails(int planId, PlanDetails plandtls)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_UpdatePlanDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlanId", planId);
                cmd.Parameters.AddWithValue("@PlanCodeExternal", plandtls.planCodeExternal);
                cmd.Parameters.AddWithValue("@PlanDescription", plandtls.planDescription);
                cmd.Parameters.AddWithValue("@CopayBuyBack", plandtls.copayBuyBack);

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
                commondal.LogError("UpdatePlanDetails", "PlanDetailsController", ex.Message, "PlanDetailsDal");
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

        public List<PlanDetails> getPlanDetails()
        {
            List<PlanDetails>? plandtls = new List<PlanDetails>();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(conString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetPlanDetails", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                connection.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    plandtls.Add(new PlanDetails
                    {
                        srNo = Convert.ToInt32(dr["SRNO"]),
                        planId = Convert.ToInt32(dr["PlanId"]),
                        policyId = Convert.ToInt32(dr["PolicyId"]),
                        policyNo = Convert.ToInt32(dr["PolicyNo"]),
                        planCodeExternal = dr["PlanCodeExternal"].ToString(),
                        planDescription = dr["PlanDescription"].ToString(),
                        copayBuyBack = dr["CopayBuyBack"].ToString(),                      
                    });
                }
                return plandtls;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetPlanDetails", "PlanDetailsController", ex.Message, "PlanDetailsDal");
                return plandtls;
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
