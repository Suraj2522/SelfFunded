using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SelfFunded.DAL
{
    public class ImpPolicyInfoDal
    {
        CommonDal commondal;
        private readonly string conString;

        public ImpPolicyInfoDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

     

        public String insertImpPolicyInfo( ImpPolicyInfo impPolinfo)
        {
            SqlConnection connection = null;

            try
            {
                int id = 0;
                using ( connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertImpPolicyInfo", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyId", impPolinfo.policyId);
                    cmd.Parameters.AddWithValue("@CorporateFloat", impPolinfo.corporateFloat);
                    cmd.Parameters.AddWithValue("@FastTrackCorp", impPolinfo.fastTrackCorp);
                    cmd.Parameters.AddWithValue("@CSICorporateFloat", impPolinfo.cSICorporateFloat);
                    cmd.Parameters.AddWithValue("@CSICorporateFloatUtilized", impPolinfo.cSICorporateFloatUtilized);
                    cmd.Parameters.AddWithValue("@CSICorporateFloatBalance", impPolinfo.cSICorporateFloatBalance);

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
                commondal.LogError("InsertImpPolicyInfo", "ImpPolicyInfoController", ex.Message, "ImpPolicyInfoDal");
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

        public String updateImpPolicyInfo(int iD,ImpPolicyInfo impPolinfo)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("[SP_UpdateImpPolicyInfo]", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PolicyId", iD);
                cmd.Parameters.AddWithValue("@CorporateFloat", impPolinfo.corporateFloat);
                cmd.Parameters.AddWithValue("@FastTrackCorp", impPolinfo.fastTrackCorp);
                cmd.Parameters.AddWithValue("@CSICorporateFloat", impPolinfo.cSICorporateFloat);
                cmd.Parameters.AddWithValue("@CSICorporateFloatUtilized", impPolinfo.cSICorporateFloatUtilized);
                cmd.Parameters.AddWithValue("@CSICorporateFloatBalance", impPolinfo.cSICorporateFloatBalance);

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
                commondal.LogError("UpdateImpPolicyInfo", "PolicyImpPolicyInfoController", ex.Message, "ImpPolicyInfoDal");
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

        public List<ImpPolicyInfo> getImpPolicyInfo()
        {
            List<ImpPolicyInfo>? impPolinfo = new List<ImpPolicyInfo>();
            SqlConnection connection = null;

            try
            {
                 connection = new SqlConnection(conString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetImpPolicyInfo", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                connection.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    impPolinfo.Add(new ImpPolicyInfo
                    {
                        srNo = Convert.ToInt32(dr["SRNO"]),
                        policyId = Convert.ToInt32(dr["PolicyId"]),
                        corporateFloat = Convert.ToInt32(dr["CorporateFloat"]),
                        fastTrackCorp = Convert.ToInt32(dr["FastTrackCorp"]),
                        cSICorporateFloat = dr["CSICorporateFloat"].ToString(),
                        cSICorporateFloatUtilized = dr["CSICorporateFloatUtilized"].ToString(),
                        cSICorporateFloatBalance = dr["CSICorporateFloatBalance"].ToString(),
                    });
                }
                return impPolinfo;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllPolicyLiveDetails", "PolicyLiveDetailsController", ex.Message, "PolicyLiveDetailsDal");
                return impPolinfo;
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
