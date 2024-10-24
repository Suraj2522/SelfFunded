using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System;

namespace SelfFunded.DAL
{
    public class BenefitConfigDal
    {
        private readonly CommonDal commondal;
        private readonly string conString;

        public BenefitConfigDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

        public string InsertBenefitConfig(BenefitConfig benconfig)
        {
            int rowsAffected = 0;
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(conString);
                SqlCommand cmd = new SqlCommand("SP_InsertBenefitConfig", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Add parameters
                cmd.Parameters.AddWithValue("@PolicyId", benconfig.policyId);
                cmd.Parameters.AddWithValue("@PlanId", benconfig.planId);
                cmd.Parameters.AddWithValue("@Benefit", benconfig.benefit);
                cmd.Parameters.AddWithValue("@Level1", benconfig.level1);
                cmd.Parameters.AddWithValue("@Level2", benconfig.level2);
                cmd.Parameters.AddWithValue("@Level3", benconfig.level3);
                cmd.Parameters.AddWithValue("@Limit", benconfig.limit);
                cmd.Parameters.AddWithValue("@ApplicablePercentage", benconfig.applicablePercentage);
                cmd.Parameters.AddWithValue("@ApplicableOn", benconfig.applicableOn);
                cmd.Parameters.AddWithValue("@FixedAmount", benconfig.fixedAmount);
                cmd.Parameters.AddWithValue("@Operator", benconfig._operator);
                cmd.Parameters.AddWithValue("@SessionAllowed", benconfig.sessionAllowed);
                cmd.Parameters.AddWithValue("@BenefitGender", benconfig.benefitGender);
                cmd.Parameters.AddWithValue("@MinAge", benconfig.minAge);
                cmd.Parameters.AddWithValue("@MaxAge", benconfig.maxAge);
                cmd.Parameters.AddWithValue("@IncrementalApplicable", benconfig.incrementalApplicable);
                cmd.Parameters.AddWithValue("@CoPayApplicable", benconfig.coPayApplicable);
                cmd.Parameters.AddWithValue("@CoPayApplicableOn", benconfig.coPayApplicableOn);
                cmd.Parameters.AddWithValue("@CoPayPercentage", benconfig.coPayPercentage);
                cmd.Parameters.AddWithValue("@CoPayFixedAmount", benconfig.coPayFixedAmount);
                cmd.Parameters.AddWithValue("@CoPayOperator", benconfig.coPayOperator);
                cmd.Parameters.AddWithValue("@ClaimType", benconfig.claimType);
                cmd.Parameters.AddWithValue("@WaitingApplicable", benconfig.waitingApplicable);
                cmd.Parameters.AddWithValue("@Waiting", benconfig.waiting);
                cmd.Parameters.AddWithValue("@ApplicableFromDate", benconfig.applicableFromDate);
                cmd.Parameters.AddWithValue("@PayableFrom", benconfig.payableFrom);
                cmd.Parameters.AddWithValue("@GroupItemwise", benconfig.groupItemwise);
                cmd.Parameters.AddWithValue("@ProviderType", benconfig.providerType);
                cmd.Parameters.AddWithValue("@AnnualLimit", benconfig.annualLimit);
                cmd.Parameters.AddWithValue("@AnnualLimitValuesInDays", benconfig.annualLimitValuesInDays);
                cmd.Parameters.AddWithValue("@ExclusionDaysFromDOA", benconfig.exclusionDaysFromDOA);
                cmd.Parameters.AddWithValue("@MaximumPayableDays", benconfig.maximumPayableDays);
                cmd.Parameters.AddWithValue("@MinimumHospitalizationDays", benconfig.minimumHospitalizationDays);
                cmd.Parameters.AddWithValue("@NatureOfStay", benconfig.natureOfStay);
                cmd.Parameters.AddWithValue("@Relation", benconfig.relation);
                cmd.Parameters.AddWithValue("@SearchFor", benconfig.searchFor);
                cmd.Parameters.AddWithValue("@CreatedBy", benconfig.createdBy);

                connection.Open();
                rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return "Data saved successfully";
                }
                else
                {
                    return "Error occurred";
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("InsertBenefitConfig", "BenefitConfigController", ex.Message, "BenefitConfigDal");
                return "An error occurred while processing the request.";
            }
            finally
            {
                // Ensure connection is properly closed
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}
