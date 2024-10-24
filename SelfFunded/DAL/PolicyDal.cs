using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace SelfFunded.DAL
{
    public class PolicyDal
    {
        CommonDal commondal;
        private readonly string conString;

        public PolicyDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

        public String insertPolicy(PolicyMaster policy)
        {
            SqlConnection connection = null;

            try
            {
                int id = 0;
                using (connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertPolicyDetails", connection);
                    cmd.CommandType = CommandType.StoredProcedure;                   
                    cmd.Parameters.AddWithValue("@Insurance",policy.insurance);
                    cmd.Parameters.AddWithValue("@UWOffice",policy.uWOffice);
                    cmd.Parameters.AddWithValue("@PolicyNo", policy.policyNo);
                    cmd.Parameters.AddWithValue("@ValidFrom", policy.validFrom);
                    cmd.Parameters.AddWithValue("@ValidTo", policy.validTo);
                    cmd.Parameters.AddWithValue("@CorporateName", policy.corporateName);
                    cmd.Parameters.AddWithValue("@GroupCode", policy.groupCode);
                    cmd.Parameters.AddWithValue("@FrequencyofEndorsement", policy.frequencyofEndorsement);
                    cmd.Parameters.AddWithValue("@TypeofPolicy", policy.typeofPolicy);
                    cmd.Parameters.AddWithValue("@Floater", policy.floater);
                    cmd.Parameters.AddWithValue("@TypeofIndustries", policy.typeofIndustries);
                    cmd.Parameters.AddWithValue("@Address", policy.address);
                    cmd.Parameters.AddWithValue("@State", policy.state);
                    cmd.Parameters.AddWithValue("@City", policy.city);
                    cmd.Parameters.AddWithValue("@PinCode", policy.pinCode);
                    cmd.Parameters.AddWithValue("@ServiceModel", policy.serviceModel);
                    cmd.Parameters.AddWithValue("@Remarks", policy.remarks);

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
                    commondal.LogError("InsertPolicyMaster", "PolicyController", ex.Message, "PolicyDal");
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

        public String updatePolicy(int id,PolicyMaster policy)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_UpdatePolicyDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PolicyId", id);
                cmd.Parameters.AddWithValue("@Insurance", policy.insurance);
                cmd.Parameters.AddWithValue("@UWOffice", policy.uWOffice);
                cmd.Parameters.AddWithValue("@PolicyNo", policy.policyNo);
                cmd.Parameters.AddWithValue("@ValidFrom", policy.validFrom);
                cmd.Parameters.AddWithValue("@ValidTo", policy.validTo);
                cmd.Parameters.AddWithValue("@CorporateName", policy.corporateName);
                cmd.Parameters.AddWithValue("@GroupCode", policy.groupCode);
                cmd.Parameters.AddWithValue("@FrequencyofEndorsement", policy.frequencyofEndorsement);
                cmd.Parameters.AddWithValue("@TypeofPolicy", policy.typeofPolicy);
                cmd.Parameters.AddWithValue("@Floater", policy.floater);
                cmd.Parameters.AddWithValue("@TypeofIndustries", policy.typeofIndustries);
                cmd.Parameters.AddWithValue("@Address", policy.address);
                cmd.Parameters.AddWithValue("@State", policy.state);
                cmd.Parameters.AddWithValue("@City", policy.city);
                cmd.Parameters.AddWithValue("@PinCode", policy.pinCode);
                cmd.Parameters.AddWithValue("@ServiceModel", policy.serviceModel);
                cmd.Parameters.AddWithValue("@Remarks", policy.remarks);
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
                commondal.LogError("InsertPolicyMaster", "PolicyController", ex.Message, "PolicyDal");
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

        public List<PolicyMaster> getAllPolicyDetails()
        {
            List<PolicyMaster>? policy = new List<PolicyMaster>();
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(conString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetAllPolicyDetails",connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                connection.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    policy.Add(new PolicyMaster
                    {


                        srNo = Convert.ToInt32(dr["SRNO"]),
                        policyId = Convert.ToInt32(dr["PolicyId"]),
                        policyNo = Convert.ToInt32(dr["PolicyNo"]),
                        insurance = dr["Insurance"].ToString(),
                        insuranceId = Convert.ToInt32(dr["InsuranceCompanyId"]),
                        uWOffice = dr["UWOffice"].ToString(),
                        corporateName = dr["CorporateName"].ToString(),
                        corporateId = Convert.ToInt32(dr["corporateId"]),
                        frequencyofEndorsement = dr["FrequencyofEndorsement"].ToString(),
                        typeofPolicy = dr["TypeofPolicy"].ToString(),
                        floater = dr["Floater"].ToString(),
                        typeofIndustries = dr["TypeofIndustries"].ToString(),
                        industryId = Convert.ToInt32(dr["IndustryId"]),
                        address = dr["Address"].ToString(),
                        groupCode = dr["GroupCode"].ToString(),
                        state = dr["State"].ToString(),
                        stateID = Convert.ToInt32(dr["StateID"]),
                        city = dr["City"].ToString(),
                        cityID = Convert.ToInt32(dr["CityID"]),
                       // pinCode = Convert.ToInt32(dr["PinCode"]),                       
                        serviceModel = dr["ServiceModel"].ToString(),
                        remarks = dr["Remarks"].ToString(),
                        validTo = DateTime.Parse(dr["ValidTo"].ToString()),
                        validFrom = DateTime.Parse(dr["ValidFrom"].ToString()),
                        premiumInfo = new PolicyPremiumInfo
                        {
                            premiumType = Convert.ToInt32(dr["PremiumType"]),
                            policyPremium = dr["PolicyPremium"].ToString(),
                            addition = dr["Addition"].ToString(),
                            deletion = dr["Deletion"].ToString(),
                            nilEndorsement = dr["NilEndorsement"].ToString(),
                            refund = dr["Refund"].ToString(),
                            extra = dr["Extra"].ToString(),
                            netPremium = dr["NetPremium"].ToString()
                        },

                        liveDetails = new PolicyLiveDetails
                        {
                            atPolicyInception = dr["AtPolicyInception"].ToString(),
                            addition = dr["Addition"].ToString(),
                            deletion = dr["Deletion"].ToString(),
                            liveActive = dr["LiveActive"].ToString()
                        },

                        impPolicyInfo= new ImpPolicyInfo
                        {
                            corporateFloat = Convert.ToInt32(dr["CorporateFloat"]),
                            fastTrackCorp = Convert.ToInt32(dr["FastTrackCorp"]),
                            cSICorporateFloat = dr["CSICorporateFloat"].ToString(),
                            cSICorporateFloatUtilized = dr["CSICorporateFloatUtilized"].ToString(),
                            cSICorporateFloatBalance = dr["CSICorporateFloatBalance"].ToString()
                       },
                        crmDetails = new CrmDetails
                        {
                            crmServicingLocation = dr["CrmServicingLocation"].ToString(),
                            crmVerticalHead = dr["CrmVerticalHead"].ToString(),
                            mobileNo = dr["MobileNo"].ToString(),
                            emailId = dr["EmailId"].ToString(),
                            brokerName = dr["BrokerId"].ToString(),
                        },
                        coinsurance =new Coinsurance
                        {
                            coinsurance = dr["Coinsurance"].ToString(),
                            coinsuranceratio = dr["Coinsuranceratio"].ToString(),
                            businessReceivePersonName = dr["Businessreceivedpersonname"].ToString(),
                            rennewalpolicy = dr["Rennewalpolicy"].ToString()
                        },
                        contactInfo=new ContactDetails 
                        {
                            emailId = dr["EmailId"].ToString(),
                            mobileNo = dr["MobileNo"].ToString(),
                            designation = dr["Designation"].ToString()
                        }
                    });

                }
                return policy;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetAllPolicyDetails", "PolicyController", ex.Message, "PolicyDal");
                return policy;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public String deletePolicy(PolicyMaster policy)
        {
            SqlConnection connection = null;

            try
            {
                 connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_DeletePolicyDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PolicyId", policy.policyId);
                
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
                commondal.LogError("DeletePolicy", "PolicyController", ex.Message, "policyDal");
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
