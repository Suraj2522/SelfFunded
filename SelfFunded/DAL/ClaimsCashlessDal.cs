using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace SelfFunded.DAL
{
    public class ClaimsCashlessDal
    {
        CommonDal commondal;
        private readonly string conString;

        public ClaimsCashlessDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

        

        public String insertCashlessClaimsDetails(Claims claim)
        {
            long result = 0;
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(conString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("IUClaimCashlessDetails", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // Parameters for stored procedure
                    command.Parameters.AddWithValue("@ClaimId", claim.claimId);
                    command.Parameters.AddWithValue("@PreAuthID", claim.preAuthID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@InsuredName", claim.insuredName);
                    command.Parameters.AddWithValue("@Gender", claim.gender);
                    command.Parameters.AddWithValue("@DateOfBirth", claim.dateOfBirth.HasValue ? claim.dateOfBirth.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@Age", claim.age);
                    command.Parameters.AddWithValue("@PolicyNo", claim.policyNo);
                    command.Parameters.AddWithValue("@ContactNo", claim.contactNo);
                    command.Parameters.AddWithValue("@CardNo", claim.cardNo);
                    command.Parameters.AddWithValue("@NetworkType", claim.networkType);
                    command.Parameters.AddWithValue("@ProviderId", claim.providerId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RoomTypeId", claim.roomTypeId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DoctorName", claim.doctorName);
                    command.Parameters.AddWithValue("@Complaints", claim.complaints);
                    command.Parameters.AddWithValue("@Diagnosis", claim.diagnosis);
                    command.Parameters.AddWithValue("@Treatment", claim.treatment);
                    command.Parameters.AddWithValue("@LengthOfStay", claim.lengthOfStay);
                    command.Parameters.AddWithValue("@TreatmentDate", claim.treatmentDate.HasValue ? claim.treatmentDate.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@FinalPayableCurrencyId", claim.finalPayableCurrencyId);
                    command.Parameters.AddWithValue("@InsuranceCompanyId", claim.insuranceCompanyId);
                    command.Parameters.AddWithValue("@AilmentId", claim.ailmentId);
                    command.Parameters.AddWithValue("@IntimationNo", claim.intimationNo);
                    command.Parameters.AddWithValue("@GrossAmount", claim.grossAmount);
                    command.Parameters.AddWithValue("@ApprovedAmount", claim.approvedAmount);
                    command.Parameters.AddWithValue("@RejectedAmount", claim.rejectedAmount);
                    command.Parameters.AddWithValue("@NetAmount", claim.netAmount);
                    command.Parameters.AddWithValue("@ClaimStatus", claim.claimStatus);
                    command.Parameters.AddWithValue("@Remarks", claim.remarks);
                    command.Parameters.AddWithValue("@CertificateNo", claim.certificateNo);
                    command.Parameters.AddWithValue("@CoPayType", claim.coPayType);
                    command.Parameters.AddWithValue("@CoPayValue", claim.coPayValue);
                    command.Parameters.AddWithValue("@CoInsurance", claim.coInsurance);
                    // command.Parameters.AddWithValue("@ClaimNo", claim.claimNo);
                    command.Parameters.AddWithValue("@Nationality", claim.nationality);
                    command.Parameters.AddWithValue("@Prefix", claim.prefix);
                    command.Parameters.AddWithValue("@CaseType", claim.caseType);
                    command.Parameters.AddWithValue("@DischargeDate", claim.dischargeDate.HasValue ? claim.dischargeDate.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@ProviderNo", claim.providerNo);
                    command.Parameters.AddWithValue("@ProviderName", claim.providerName);
                    command.Parameters.AddWithValue("@CountryCode", claim.countryCode);
                    command.Parameters.AddWithValue("@CountryName", claim.countryName);
                    command.Parameters.AddWithValue("@StateCode", claim.stateCode);
                    command.Parameters.AddWithValue("@StateName", claim.stateName);
                    command.Parameters.AddWithValue("@CityCode", claim.cityCode);
                    command.Parameters.AddWithValue("@CityName", claim.cityName);
                    command.Parameters.AddWithValue("@ProviderAmountCurrency", claim.providerAmountCurrency);
                    command.Parameters.AddWithValue("@PHMFeesCurrency", claim.phmFeesCurrency);
                    command.Parameters.AddWithValue("@DatabaseType", claim.databaseType);
                    command.Parameters.AddWithValue("@HospitalName", claim.hospitalName);
                    command.Parameters.AddWithValue("@ProviderCategoryType", claim.providerCategoryType);
                    command.Parameters.AddWithValue("@ProviderAddress1", claim.providerAddress1);
                    command.Parameters.AddWithValue("@ProviderAddress2", claim.providerAddress2);
                    command.Parameters.AddWithValue("@ProviderAddressArea", claim.providerAddressArea);
                    command.Parameters.AddWithValue("@ProviderPincode", claim.providerPincode);
                    command.Parameters.AddWithValue("@ProviderAreaCode", claim.providerAreaCode);
                    command.Parameters.AddWithValue("@ProviderContactNo", claim.providerContactNo);
                    command.Parameters.AddWithValue("@ProviderFaxNo", claim.providerFaxNo);
                    command.Parameters.AddWithValue("@ProviderEmailId", claim.providerEmailId);
                    command.Parameters.AddWithValue("@ProviderWebsite", claim.providerWebsite);
                    command.Parameters.AddWithValue("@OverseasId", claim.overseasId);
                    command.Parameters.AddWithValue("@Charges", claim.charges);
                    command.Parameters.AddWithValue("@HospitalAmount", claim.hospitalAmount);
                    command.Parameters.AddWithValue("@HospitalCurrency", claim.hospitalCurrency);
                    command.Parameters.AddWithValue("@AssistantCurrency", claim.assistantFeesCurrencyId);
                    command.Parameters.AddWithValue("@IsPHMFeesApplicable", claim.isPhmFeesApplicable);
                    command.Parameters.AddWithValue("@NegotiatedAmount", claim.negotiatedAmount);
                    command.Parameters.AddWithValue("@DiscountAmount", claim.discountAmount);
                    command.Parameters.AddWithValue("@AmountWithoutDeduction", claim.amountWithoutDeduction);
                    command.Parameters.AddWithValue("@DependentCode", claim.dependentCode);
                    command.Parameters.AddWithValue("@RevisedAmount", claim.revisedAmount);
                    command.Parameters.AddWithValue("@PayerId", claim.payerId);
                    command.Parameters.AddWithValue("@PayerName", claim.payerName);
                    command.Parameters.AddWithValue("@IntimationId", claim.intimationId);
                    command.Parameters.AddWithValue("@ProductId", claim.productId);
                    command.Parameters.AddWithValue("@PlanId", claim.planId);
                    command.Parameters.AddWithValue("@ClaimBenefitId", claim.claimBenefitId);
                    command.Parameters.AddWithValue("@UniqueId", claim.uniqueId);
                    command.Parameters.AddWithValue("@LogNumber", claim.logNumber);
                    command.Parameters.AddWithValue("@MemberEmailId", claim.memberEmailId);
                    command.Parameters.AddWithValue("@CaseReceiptDate", claim.caseReceiptDate.HasValue ? claim.caseReceiptDate.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@EmailRespondDate", claim.emailRespondDate.HasValue ? claim.emailRespondDate.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@PrimaryMember", claim.primaryMember);
                    command.Parameters.AddWithValue("@RelationId", claim.relationId);
                    command.Parameters.AddWithValue("@EmployeeTypeId", claim.employeeTypeId);
                    command.Parameters.AddWithValue("@AlternateContactNo", claim.alternateContactNo);
                    command.Parameters.AddWithValue("@AuditorRemarks", claim.auditorRemarks);
                    command.Parameters.AddWithValue("@RequestType", claim.requestType);
                    command.Parameters.AddWithValue("@RequestTypeOther", claim.requestTypeOther);
                    command.Parameters.AddWithValue("@HospitalVerification", claim.hospitalVerification);
                    command.Parameters.AddWithValue("@DateOfDeath", claim.dateOfDeath.HasValue ? claim.dateOfDeath.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@CauseOfDeath", claim.causeOfDeath);
                    command.Parameters.AddWithValue("@EmployeeCode", claim.employeeCode);
                    command.Parameters.AddWithValue("@EnrollmentId", claim.enrollmentId);
                    command.Parameters.AddWithValue("@MemberId", claim.memberId);
                    command.Parameters.AddWithValue("@OverrideAlert", claim.overrideAlert);
                    command.Parameters.AddWithValue("@GroupPolicyId", claim.groupPolicyId);
                    command.Parameters.AddWithValue("@PHMCommentsId", claim.phmCommentsId);
                    command.Parameters.AddWithValue("@PHMCommentsOther", claim.phmCommentsOther);
                    command.Parameters.AddWithValue("@BranchId", claim.branchId);
                    command.Parameters.AddWithValue("@HospitalTypeId", claim.hospitalTypeId);
                    command.Parameters.AddWithValue("@TariffDeduction", claim.tariffDeduction);
                    command.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    result = Convert.ToInt64(command.Parameters["@Result"].Value);
                    if (result > 0)
                    {
                        return "Data add successfully";
                    }
                    else
                    {
                        return "Error occurred while saving data.";
                    }
                }

            }
            catch (Exception ex)
            {
                 commondal.LogError("InsertPreAuthDetails", "PreAuthController", ex.Message, "PreAuthDal");
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

        public List<Dictionary<string,object>> getCashlessType()
        {
            List<Dictionary<string, object>> cashlessTypes = new List<Dictionary<string, object>>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetCashlessType]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<string, object> rodict =new Dictionary<string, object>();   
                    foreach(DataColumn column in dt.Columns)
                    {
                        rodict[column.ColumnName] = dr[column];
                    }
                    cashlessTypes.Add(rodict);
                }
                
                return cashlessTypes;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetCashlessType", "ClaimsController", ex.Message, "ClaimsDal.getCashlessType");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

           
        }
        public List<Dictionary<string, object>> getClaims( Claims clm)
        {
            List<Dictionary<string,object> >clmdtls = new List<Dictionary<string,object>>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[Usp_ClaimSearch]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 600;
            da.SelectCommand.Parameters.AddWithValue("@InsuranceId", clm.insuranceCompanyId);
            da.SelectCommand.Parameters.AddWithValue("@InsuredName", clm.insuredName);
            da.SelectCommand.Parameters.AddWithValue("@PolicyCardNo", clm.policyNo);
            da.SelectCommand.Parameters.AddWithValue("@FromDate", clm.fromDate);
            da.SelectCommand.Parameters.AddWithValue("@ToDate", clm.toDate);
            da.SelectCommand.Parameters.AddWithValue("@StageId", clm.stageId);
            da.SelectCommand.Parameters.AddWithValue("@ClaimNo", clm.claimNo);
            da.SelectCommand.Parameters.AddWithValue("@CaseType", clm.caseType);
            da.SelectCommand.Parameters.AddWithValue("@PrimaryMember", clm.primaryMember);
            da.SelectCommand.Parameters.AddWithValue("@UniqueId", clm.uniqueId);
            da.SelectCommand.Parameters.AddWithValue("@ContactNo", clm.contactNo);
            da.SelectCommand.Parameters.AddWithValue("@InsuranceType", clm.insuranceType);
            da.SelectCommand.Parameters.AddWithValue("@LoginTypeId", clm.loginTypeId);
            da.SelectCommand.Parameters.AddWithValue("@InsuranceIDs", clm.insuranceIDs);
            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);


                foreach(DataRow dr  in dt.Rows)
                {
                  Dictionary<string,object> dic = new Dictionary<string,object>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        dic.Add(column.ColumnName, dr[column.ColumnName]);

                    }
                    clmdtls.Add(dic);
                }
                return clmdtls;
}
            catch (Exception ex)
            {
                commondal.LogError("GetCashlessType", "ClaimsController", ex.Message, "ClaimsDal.getCashlessType");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

          
        }

    }
}
