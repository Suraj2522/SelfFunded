using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace SelfFunded.DAL
{
    public class ClaimReimbursementDal
    {
        CommonDal commondal;
        private readonly string conString;

        public ClaimReimbursementDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

        public String insertReimbursement(Claims clm)
        {
            long result = 0;
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(conString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("IUClaimReimbursement", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // Parameters for stored procedure
                    command.Parameters.AddWithValue("@ClaimId", clm.claimId);
                    command.Parameters.AddWithValue("@PreAuthID", clm.preAuthID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@InsuredName", clm.insuredName);
                    command.Parameters.AddWithValue("@Gender", clm.gender);
                    command.Parameters.AddWithValue("@DateOfBirth", clm.dateOfBirth.HasValue ? clm.dateOfBirth.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@Age", clm.age);
                    command.Parameters.AddWithValue("@PolicyNo", 0);
                    command.Parameters.AddWithValue("@ContactNo", clm.contactNo);
                    command.Parameters.AddWithValue("@CardNo", clm.cardNo);
                    command.Parameters.AddWithValue("@NetworkType", clm.networkType);
                    command.Parameters.AddWithValue("@ProviderId", clm.providerId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@RoomTypeId", clm.roomTypeId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DoctorName", clm.doctorName);
                    command.Parameters.AddWithValue("@Complaints", clm.complaints);
                    command.Parameters.AddWithValue("@Diagnosis", clm.diagnosis);
                    command.Parameters.AddWithValue("@Treatment", clm.treatment);
                    command.Parameters.AddWithValue("@LengthOfStay", clm.lengthOfStay);
                    command.Parameters.AddWithValue("@TreatmentDate", clm.treatmentDate.HasValue ? clm.treatmentDate.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@FinalPayableCurrencyId", clm.finalPayableCurrencyId);
                    command.Parameters.AddWithValue("@InsuranceCompanyId", clm.insuranceCompanyId);
                    command.Parameters.AddWithValue("@AilmentId", clm.ailmentId);
                    command.Parameters.AddWithValue("@IntimationNo", clm.intimationNo);
                    command.Parameters.AddWithValue("@GrossAmount", clm.grossAmount);
                    command.Parameters.AddWithValue("@ApprovedAmount", clm.approvedAmount);
                    command.Parameters.AddWithValue("@RejectedAmount", clm.rejectedAmount);
                    command.Parameters.AddWithValue("@NetAmount", clm.netAmount);
                    command.Parameters.AddWithValue("@ClaimStatus", clm.claimStatus);
                    command.Parameters.AddWithValue("@Remarks", clm.remarks);
                    command.Parameters.AddWithValue("@CertificateNo", clm.certificateNo);
                    command.Parameters.AddWithValue("@CoPayType", clm.coPayType);
                    command.Parameters.AddWithValue("@CoPayValue", clm.coPayValue);
                    command.Parameters.AddWithValue("@CoInsurance", clm.coInsurance);
                    // command.Parameters.AddWithValue("@ClaimNo", clm.claimNo);
                    command.Parameters.AddWithValue("@Nationality", clm.nationality);
                    command.Parameters.AddWithValue("@Prefix", clm.prefix);
                    command.Parameters.AddWithValue("@CaseType", clm.caseType);
                    command.Parameters.AddWithValue("@DischargeDate", clm.dischargeDate.HasValue ? clm.dischargeDate.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@ProviderNo", clm.providerNo);
                    command.Parameters.AddWithValue("@ProviderName", clm.providerName);
                    command.Parameters.AddWithValue("@CountryCode", clm.countryCode);
                    command.Parameters.AddWithValue("@CountryName", clm.countryName);
                    command.Parameters.AddWithValue("@StateCode", clm.stateCode);
                    command.Parameters.AddWithValue("@StateName", clm.stateName);
                    command.Parameters.AddWithValue("@CityCode", clm.cityCode);
                    command.Parameters.AddWithValue("@CityName", clm.cityName);
                    command.Parameters.AddWithValue("@ProviderAmountCurrency", clm.providerAmountCurrency);
                    command.Parameters.AddWithValue("@PHMFeesCurrency", clm.phmFeesCurrency);
                    command.Parameters.AddWithValue("@DatabaseType", clm.databaseType);
                    command.Parameters.AddWithValue("@HospitalName", clm.hospitalName);
                    command.Parameters.AddWithValue("@ProviderCategoryType", clm.providerCategoryType);
                    command.Parameters.AddWithValue("@ProviderAddress1", clm.providerAddress1);
                    command.Parameters.AddWithValue("@ProviderAddress2", clm.providerAddress2);
                    command.Parameters.AddWithValue("@ProviderAddressArea", clm.providerAddressArea);
                    command.Parameters.AddWithValue("@ProviderPincode", clm.providerPincode);
                    command.Parameters.AddWithValue("@ProviderAreaCode", clm.providerAreaCode);
                    command.Parameters.AddWithValue("@ProviderContactNo", clm.providerContactNo);
                    command.Parameters.AddWithValue("@ProviderFaxNo", clm.providerFaxNo);
                    command.Parameters.AddWithValue("@ProviderEmailId", clm.providerEmailId);
                    command.Parameters.AddWithValue("@ProviderWebsite", clm.providerWebsite);
                    command.Parameters.AddWithValue("@OverseasId", clm.overseasId);
                    command.Parameters.AddWithValue("@Charges", clm.charges);
                    command.Parameters.AddWithValue("@HospitalAmount", clm.hospitalAmount);
                    command.Parameters.AddWithValue("@HospitalCurrency", clm.hospitalCurrency);
                    command.Parameters.AddWithValue("@AssistantCurrency", clm.assistantFeesCurrencyId);
                    command.Parameters.AddWithValue("@IsPHMFeesApplicable", clm.isPhmFeesApplicable);
                    command.Parameters.AddWithValue("@NegotiatedAmount", clm.negotiatedAmount);
                    command.Parameters.AddWithValue("@DiscountAmount", clm.discountAmount);
                    command.Parameters.AddWithValue("@AmountWithoutDeduction", clm.amountWithoutDeduction);
                    command.Parameters.AddWithValue("@DependentCode", clm.dependentCode);
                    command.Parameters.AddWithValue("@RevisedAmount", clm.revisedAmount);
                    command.Parameters.AddWithValue("@PayerId", clm.payerId);
                    command.Parameters.AddWithValue("@PayerName", clm.payerName);
                    command.Parameters.AddWithValue("@IntimationId", clm.intimationId);
                    command.Parameters.AddWithValue("@ProductId", clm.productId);
                    command.Parameters.AddWithValue("@PlanId", clm.planId);
                    command.Parameters.AddWithValue("@ClaimBenefitId", clm.claimBenefitId);
                    command.Parameters.AddWithValue("@UniqueId", clm.uniqueId);
                    command.Parameters.AddWithValue("@LogNumber", clm.logNumber);
                    command.Parameters.AddWithValue("@MemberEmailId", clm.memberEmailId);
                    command.Parameters.AddWithValue("@CaseReceiptDate", clm.caseReceiptDate.HasValue ? clm.caseReceiptDate.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@EmailRespondDate", clm.emailRespondDate.HasValue ? clm.emailRespondDate.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@PrimaryMember", clm.primaryMember);
                    command.Parameters.AddWithValue("@RelationId", clm.relationId);
                    command.Parameters.AddWithValue("@EmployeeTypeId", clm.employeeTypeId);
                    command.Parameters.AddWithValue("@AlternateContactNo", clm.alternateContactNo);
                    command.Parameters.AddWithValue("@AuditorRemarks", clm.auditorRemarks);
                    command.Parameters.AddWithValue("@RequestType", clm.requestType);
                    command.Parameters.AddWithValue("@RequestTypeOther", clm.requestTypeOther);
                    command.Parameters.AddWithValue("@HospitalVerification", clm.hospitalVerification);
                    command.Parameters.AddWithValue("@DateOfDeath", clm.dateOfDeath.HasValue ? clm.dateOfDeath.Value.ToString("dd.MM.yyyy") : "");
                    command.Parameters.AddWithValue("@CauseOfDeath", clm.causeOfDeath);
                    command.Parameters.AddWithValue("@EmployeeCode", clm.employeeCode);
                    command.Parameters.AddWithValue("@EnrollmentId", clm.enrollmentId);
                    command.Parameters.AddWithValue("@MemberId", clm.memberId);
                    command.Parameters.AddWithValue("@OverrideAlert", clm.overrideAlert);
                    command.Parameters.AddWithValue("@GroupPolicyId", clm.groupPolicyId);
                    command.Parameters.AddWithValue("@PHMCommentsId", clm.phmCommentsId);                    
                    command.Parameters.AddWithValue("@PHMCommentsOther", clm.phmCommentsOther);
                    command.Parameters.AddWithValue("@BranchId", clm.branchId);                    
                    command.Parameters.AddWithValue("@HospitalTypeId", clm.hospitalTypeId);
                    command.Parameters.AddWithValue("@TariffDeduction", clm.tariffDeduction);
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
                commondal.LogError("InsertReimbursement", "ClaimsReimbursementController", ex.Message, "ClaimReimbursementDal");
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

        public List<Dictionary<string,object>> getReimbursementType()
            {
            List<Dictionary<string, object>> reimbursementTypes =new  List<Dictionary<string, object>>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetReimbursementType]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 600;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach(DataRow dr in  dt.Rows) 
                { 
                    Dictionary<string,object> rowdict = new Dictionary<string,object>();
                    foreach (DataColumn column in dt.Columns)
                    {

                        rowdict[column.ColumnName] = dr[column];

                    }

                    reimbursementTypes.Add(rowdict);
                }
                
            }
            catch (Exception ex)
            {
                commondal.LogError("GetReimbursementType", "ClaimsController", ex.Message, "ClaimsDal.getReimbursementType");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return reimbursementTypes;
        }
    }
}
