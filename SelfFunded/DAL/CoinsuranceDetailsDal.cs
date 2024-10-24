using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Security.AccessControl;

namespace SelfFunded.DAL
{
    public class CoinsuranceDetailsDal
    {
        CommonDal commondal;
        private readonly string conString;

        public CoinsuranceDetailsDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

       
        public String insertCoinsuranceDetails(Coinsurance coins)
        {
            SqlConnection connection=null;
            try
            {
                using ( connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertCoinsuranceDetails", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyId", coins.policyId);
                    cmd.Parameters.AddWithValue("@Coinsurance", coins.coinsurance);
                    cmd.Parameters.AddWithValue("@Coinsuranceratio", coins.coinsuranceratio);
                    cmd.Parameters.AddWithValue("@Businessreceivedpersonname", coins.businessReceivePersonName);
                    cmd.Parameters.AddWithValue("@RennewalPolicy", coins.rennewalpolicy);

                    connection.Open();
                    int id = cmd.ExecuteNonQuery();
                  
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
                commondal.LogError("InsertCoinsuranceDetailsetails", "CoinsuranceDetailsController", ex.Message, "CoinsuranceDetailsDal");
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

        public String updateCoinsuranceDetails(int coinsuranceId, Coinsurance coins)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_UpdateCoinsuranceDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CoinsuranceId", coinsuranceId);
                cmd.Parameters.AddWithValue("@Coinsurance", coins.coinsurance);
                cmd.Parameters.AddWithValue("@Coinsuranceratio", coins.coinsuranceratio);
                cmd.Parameters.AddWithValue("@Businessreceivedpersonname", coins.businessReceivePersonName);
                cmd.Parameters.AddWithValue("@Rennewalpolicy", coins.rennewalpolicy);

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
                commondal.LogError("UpdateCoinsuranceDetailsetails", "CoinsuranceDetailsController", ex.Message, "CoinsuranceDetailsDal");
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

        public List<Coinsurance> getCoinsuranceDetails()
        {
            SqlConnection connection = null;
            List<Coinsurance>? coins = new List<Coinsurance>();
            try
            {
                connection = new SqlConnection(conString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetCoinsuranceDetails", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                connection.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    coins.Add(new Coinsurance
                    {
                        srNo = Convert.ToInt32(dr["SRNO"]),
                        policyId = Convert.ToInt32(dr["PolicyId"]),
                        coinsurance = dr["Coinsurance"].ToString(),
                        coinsuranceratio = dr["Coinsuranceratio"].ToString(),
                        businessReceivePersonName = dr["Businessreceivedpersonname"].ToString(),
                        rennewalpolicy = dr["Rennewalpolicy"].ToString()
                    });
                }
                return coins;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetCoinsuranceDetailsetails", "CoinsuranceDetailsController", ex.Message, "CoinsuranceDetailsDal");
                return coins;
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
