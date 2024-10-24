using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SelfFunded.DAL
{
    public class BrokerDal
    {
        CommonDal commondal;
        private readonly string conString;

        public BrokerDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

        public string deleteBroker(BrokerMaster broker)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_DeleteBrokerDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BrokerId", broker.brokerId);
                cmd.Parameters.AddWithValue("@isDeleted", broker.isDeleted);
                cmd.Parameters.AddWithValue("@isActive", broker.isActive);
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return "Data deleted successfully";
                }
                else
                {
                    return "Error occurred";
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("deleteBroker", "BrokerController", ex.Message, "BrokerDal");
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

        public List<BrokerMaster> getAllBrokerDetails()
        {
            List<BrokerMaster> broker = new List<BrokerMaster>();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(conString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetAllBrokerDetails", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    broker.Add(new BrokerMaster
                    {
                        srNo = Convert.ToInt32(dr["SRNO"]),
                        brokerId = Convert.ToInt32(dr["BrokerId"]),
                        brokerName = dr["BrokerName"].ToString(),
                        emailId = dr["EmailId"].ToString(),
                        mobileNo = dr["MobileNo"].ToString(),
                        businessEmailId = dr["BusinessEmailId"].ToString(),
                        officePhone = dr["OfficePhone"].ToString(),
                        fax = dr["Fax"].ToString(),
                        address1 = dr["Address1"].ToString(),
                        address2 = dr["Address2"].ToString(),
                        country = dr["Country"].ToString(),
                        city = dr["City"].ToString(),
                        cityID = Convert.ToInt32(dr["CityID"]),
                        countryID = Convert.ToInt32(dr["CountryID"]),
                        stateID = Convert.ToInt32(dr["StateID"]),
                        state = dr["State"].ToString(),
                        website = dr["Website"].ToString(),
                        brokerLogo = dr["BrokerLogo"].ToString(),
                    });
                }
                return broker;
            }
            catch (Exception ex)
            {
                commondal.LogError("getAllBrokerDetails", "BrokerController", ex.Message, "BrokerDal");
                return broker;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public string insertBroker(BrokerMaster broker)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertBrokerDetails", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BrokerName", broker.brokerName);
                    cmd.Parameters.AddWithValue("@EmailId", broker.emailId);
                    cmd.Parameters.AddWithValue("@BusinessEmailId", broker.businessEmailId);
                    cmd.Parameters.AddWithValue("@MobileNo", broker.mobileNo);
                    cmd.Parameters.AddWithValue("@OfficePhone", broker.officePhone);
                    cmd.Parameters.AddWithValue("@Fax", broker.fax);
                    cmd.Parameters.AddWithValue("@Address1", broker.address1);
                    cmd.Parameters.AddWithValue("@Address2", broker.address2);
                    cmd.Parameters.AddWithValue("@Country", broker.country);
                    cmd.Parameters.AddWithValue("@City", broker.city);
                    cmd.Parameters.AddWithValue("@State", broker.state);
                    cmd.Parameters.AddWithValue("@Website", broker.website);
                    cmd.Parameters.AddWithValue("@BrokerLogo", broker.brokerLogo);
                    cmd.Parameters.AddWithValue("@CreatedBy", broker.createdBy);
                    connection.Open();
                    int id = cmd.ExecuteNonQuery();
                    if (id > 0)
                    {
                        return "Data saved successfully";
                    }
                    else
                    {
                        return "Error occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("insertBroker", "BrokerController", ex.Message, "BrokerDal");
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

        public string updateBroker(int id, BrokerMaster broker)
        {
            SqlConnection connection = null;
            try
            {


                connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_UpdateBrokerDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BrokerId", id);
                cmd.Parameters.AddWithValue("@BrokerName", broker.brokerName);
                cmd.Parameters.AddWithValue("@EmailId", broker.emailId);
                cmd.Parameters.AddWithValue("@BusinessEmailId", broker.businessEmailId);
                cmd.Parameters.AddWithValue("@MobileNo", broker.mobileNo);
                cmd.Parameters.AddWithValue("@OfficePhone", broker.officePhone);
                cmd.Parameters.AddWithValue("@Fax", broker.fax);
                cmd.Parameters.AddWithValue("@Address1", broker.address1);
                cmd.Parameters.AddWithValue("@Address2", broker.address2);
                cmd.Parameters.AddWithValue("@Country", broker.country);
                cmd.Parameters.AddWithValue("@City", broker.city);
                cmd.Parameters.AddWithValue("@State", broker.state);
                cmd.Parameters.AddWithValue("@Website", broker.website);
                cmd.Parameters.AddWithValue("@BrokerLogo", broker.brokerLogo);
                cmd.Parameters.AddWithValue("@CreatedBy", broker.createdBy);
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return "Data updated successfully";
                }
                else
                {
                    return "Error occurred";
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("updateBroker", "BrokerController", ex.Message, "BrokerDal");
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
