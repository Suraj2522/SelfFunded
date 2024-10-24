using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SelfFunded.DAL
{
    public class ContactDetailsDal
    {
        CommonDal commondal;
        private readonly string conString;

        public ContactDetailsDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

       

        public String insertContactDetails(ContactDetails contdtls)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertContactDetails", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@PolicyId", contdtls.policyNo);
                    cmd.Parameters.AddWithValue("@MobileNo", contdtls.mobileNo);
                    cmd.Parameters.AddWithValue("@EmailId", contdtls.emailId);
                    cmd.Parameters.AddWithValue("@Designation", contdtls.designation);

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
                commondal.LogError("InsertContactDetails", "ContactDetailsController", ex.Message, "ContactDetailsDal");
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

        public String updateContactDetails(int id, ContactDetails contdtls)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_UpdateContactDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@MobileNo", contdtls.mobileNo);
                cmd.Parameters.AddWithValue("@EmailId", contdtls.emailId);
                cmd.Parameters.AddWithValue("@Designation", contdtls.designation);

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
    }
}
