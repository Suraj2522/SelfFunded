using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;

namespace SelfFunded.DAL
{
    public class PreAuthDal
    {
        CommonDal commondal;
        private readonly string conString;

        public PreAuthDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

        private void LogError(string methodName, string controllerName, string errorMessage, string dalName)
        {
            ErrorLog errorLog = new ErrorLog
            {
                error = errorMessage,
                methodName = methodName,
                controllerName = controllerName,
                dalName = string.IsNullOrEmpty(dalName) ? null : dalName
            };
            commondal.insertErrorLog(errorLog);
        }

        public String insertPreAuth(PreAuth preauth)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand("IUPreAuthDetails", connection);
                    cmd.CommandTimeout = 120;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PreAuthID", preauth.preAuthID);
                    cmd.Parameters.AddWithValue("@InsuredName", preauth.insuredName);
                    cmd.Parameters.AddWithValue("@Gender", preauth.gender);
                    cmd.Parameters.AddWithValue("@DateOfBirth", preauth.dateOfBirth);
                    cmd.Parameters.AddWithValue("@Age", preauth.age);
                    cmd.Parameters.AddWithValue("@PolicyNo", preauth.policyNo);
                    cmd.Parameters.AddWithValue("@ContactNo", preauth.contactNo);
                    cmd.Parameters.AddWithValue("@CardNo", preauth.cardNo);
                    cmd.Parameters.AddWithValue("@NetworkType", preauth.networkType);
                    cmd.Parameters.AddWithValue("@ProviderId", preauth.providerId);
                    cmd.Parameters.AddWithValue("@RoomTypeId", preauth.roomTypeId);
                    cmd.Parameters.AddWithValue("@DoctorName", preauth.doctorName);
                    cmd.Parameters.AddWithValue("@Complaints", preauth.complaints);
                    cmd.Parameters.AddWithValue("@Diagnosis", preauth.diagnosis);
                    cmd.Parameters.AddWithValue("@Treatment", preauth.treatment);
                    cmd.Parameters.AddWithValue("@LengthOfStay", preauth.lengthOfStay);
                    cmd.Parameters.AddWithValue("@TreatmentDate", preauth.treatmentDate);
                    cmd.Parameters.AddWithValue("@ProcessingCurrencyId", preauth.processingCurrencyId);
                    cmd.Parameters.AddWithValue("@FinalPayableCurrencyId", preauth.finalPayableCurrencyId);
                    cmd.Parameters.AddWithValue("@InsuranceCompanyId", preauth.insuranceCompanyId);
                    cmd.Parameters.AddWithValue("@AilmentId", preauth.ailmentId);
                    cmd.Parameters.AddWithValue("@IntimationNo", preauth.intimationNo);
                    //cmd.Parameters.AddWithValue("@ProcedureIDs", preauth.procedureIDs);
                    //cmd.Parameters.AddWithValue("@RemarksIDs", preauth.remarksIDs);
                    //cmd.Parameters.AddWithValue("@FileUploadIDs", preauth.fileUploadIDs);
                    cmd.Parameters.AddWithValue("@UserID", preauth.createByUserId);
                    cmd.Parameters.AddWithValue("@GrossAmount", preauth.grossAmount);
                    cmd.Parameters.AddWithValue("@ApprovedAmount", preauth.approvedAmount);
                    cmd.Parameters.AddWithValue("@RejectedAmount", preauth.rejectedAmount);
                    cmd.Parameters.AddWithValue("@NetAmount", preauth.netAmount);
                    cmd.Parameters.AddWithValue("@ClaimStatus", preauth.claimStatus);
                    cmd.Parameters.AddWithValue("@RefferedToIns", preauth.refferedToIns);
             //       cmd.Parameters.AddWithValue("@ExclusionIDs", preauth.exclusionIDs);
//                            cmd.Parameters.AddWithValue("@Remarks", preauth.remarks);
                    cmd.Parameters.AddWithValue("@CertificateNo", preauth.certificateNo);
                    cmd.Parameters.AddWithValue("@CoPayType", preauth.coPayType);
                    cmd.Parameters.AddWithValue("@CoPayValue", preauth.coPayValue);
                    cmd.Parameters.AddWithValue("@CoInsurance", preauth.coInsurance);
                    //cmd.Parameters.AddWithValue("@TempDiagnosisIds", preauth.tempDiagnosisIds);
                    //cmd.Parameters.AddWithValue("@TempRemarksIds", preauth.tempRemarksIds);
                    cmd.Parameters.AddWithValue("@Nationality", preauth.nationality);
                    cmd.Parameters.AddWithValue("@Prefix", preauth.prefix);
                    cmd.Parameters.AddWithValue("@CaseType", preauth.caseType);
                    cmd.Parameters.AddWithValue("@DischargeDate", preauth.dischargeDate);
                    cmd.Parameters.AddWithValue("@ProviderNo", preauth.providerNo);
                    cmd.Parameters.AddWithValue("@ProviderName", preauth.providerName);
                    cmd.Parameters.AddWithValue("@CountryCode", preauth.countryCode);
                    cmd.Parameters.AddWithValue("@CountryName", preauth.countryName);
                    cmd.Parameters.AddWithValue("@StateCode", preauth.stateCode);
                    cmd.Parameters.AddWithValue("@StateName", preauth.stateName);
                    cmd.Parameters.AddWithValue("@CityCode", preauth.cityCode);
                    cmd.Parameters.AddWithValue("@CityName", preauth.cityName);
                    cmd.Parameters.AddWithValue("@HospitalName", preauth.hospitalName);
                    cmd.Parameters.AddWithValue("@ProviderCategoryType", preauth.providerCategoryType);
                    cmd.Parameters.AddWithValue("@ProviderAddress1", preauth.providerAddress1);
                    cmd.Parameters.AddWithValue("@ProviderAddress2", preauth.providerAddress2);
                    cmd.Parameters.AddWithValue("@ProviderAddressArea", preauth.providerAddressArea);
                    cmd.Parameters.AddWithValue("@ProviderPincode", preauth.providerPincode);
                    cmd.Parameters.AddWithValue("@ProviderAreaCode", preauth.providerAreaCode);
                    cmd.Parameters.AddWithValue("@ProviderContactNo", preauth.providerContactNo);
                    cmd.Parameters.AddWithValue("@ProviderFaxNo", preauth.providerFaxNo);
                    cmd.Parameters.AddWithValue("@ProviderEmailId", preauth.providerEmailId);
                    cmd.Parameters.AddWithValue("@ProviderWebsite", preauth.providerWebsite);
                    cmd.Parameters.AddWithValue("@DatabaseType", preauth.databaseType);
                    cmd.Parameters.AddWithValue("@OverseasId", preauth.overseasId);
                    cmd.Parameters.AddWithValue("@AssistantCharges", preauth.assistantCharges);
                    cmd.Parameters.AddWithValue("@AssistantCurrency", preauth.assistantFeesCurrencyId);
                    cmd.Parameters.AddWithValue("@ProviderRequestId", preauth.providerRequestId);
                    cmd.Parameters.AddWithValue("@ApprovedRejectedBy", preauth.approvedRejectedBy);
                    cmd.Parameters.AddWithValue("@ApprovedRejectedDate", preauth.approvedRejectedDate);
                    cmd.Parameters.AddWithValue("@PatientStatusId", preauth.patientStatusId);
                    //cmd.Parameters.AddWithValue("@TempNotesId", preauth.tempNotesId);
                    //cmd.Parameters.AddWithValue("@PreAuthRemarksId", preauth.preAuthRemarksId);
                    cmd.Parameters.AddWithValue("@TimeOfAdmission", preauth.timeOfAdmission);
                    cmd.Parameters.AddWithValue("@TimeOfDischarge", preauth.timeOfDischarge);
                    cmd.Parameters.AddWithValue("@NegotiatedAmount", preauth.negotiatedAmount);
                    cmd.Parameters.AddWithValue("@DiscountAmount", preauth.discountAmount);
                    cmd.Parameters.AddWithValue("@AmountWithoutDeduction", preauth.amountWithoutDeduction);
                    cmd.Parameters.AddWithValue("@DependentCode", preauth.dependentCode);
                    cmd.Parameters.AddWithValue("@RevisedAmount", preauth.revisedAmount);
                    cmd.Parameters.AddWithValue("@PayerId", preauth.payerId);
                    cmd.Parameters.AddWithValue("@PayerName", preauth.payerName);
                    cmd.Parameters.AddWithValue("@HospitalEmailId", preauth.hospitalEmailId);
                    cmd.Parameters.AddWithValue("@LogNumber", preauth.logNumber);
                    cmd.Parameters.AddWithValue("@MemberEmailId", preauth.memberEmailId);
                    cmd.Parameters.AddWithValue("@CaseReceiptDate", preauth.caseReceiptDate);
                    cmd.Parameters.AddWithValue("@EmailRespondDate", preauth.emailRespondDate);
                    cmd.Parameters.AddWithValue("@CaseRemarks", preauth.caseRemarks);
                    cmd.Parameters.AddWithValue("@IntimationId", preauth.intimationNo);
                    cmd.Parameters.AddWithValue("@ProductId", preauth.productId);
                    cmd.Parameters.AddWithValue("@PlanId", preauth.planId);
                    cmd.Parameters.AddWithValue("@ClaimBenefitId", preauth.claimBenefitId);
                    cmd.Parameters.AddWithValue("@EmployeeCode", preauth.employeeCode);
                    cmd.Parameters.AddWithValue("@EnrollmentId", preauth.enrollmentId);
                    cmd.Parameters.AddWithValue("@MemberId", preauth.memberId);
                    cmd.Parameters.AddWithValue("@OverrideAlert", preauth.overrideAlert);
                    cmd.Parameters.AddWithValue("@GroupPolicyId", preauth.groupPolicyId);
                 //   cmd.Parameters.AddWithValue("@IsDeficiencyRetrieved", preauth.isDeficiencyRetrieved);
                    cmd.Parameters.AddWithValue("@PHMCommentsId", preauth.phmCommentsId);
                    cmd.Parameters.AddWithValue("@PHMCommentsOther", preauth.phmCommentsOther);
                    cmd.Parameters.AddWithValue("@BranchId", preauth.branchId);
                    cmd.Parameters.AddWithValue("@EwiseCheck", preauth.ewiseCheck);
                    cmd.Parameters.AddWithValue("@EwiseCheckRemarks", preauth.ewiseCheckRemarks);
                    cmd.Parameters.AddWithValue("@MOUDiscountCheck", preauth.mouDiscountCheck);
                    cmd.Parameters.AddWithValue("@MOUDiscountCheckRemarks", preauth.mouDiscountCheckRemarks);
                    cmd.Parameters.AddWithValue("@HospitalTypeId", preauth.hospitalTypeId);
                  //  cmd.Parameters.AddWithValue("@MedicalOpinionIds", preauth.medicalOpinionIds);
                    cmd.Parameters.AddWithValue("@InsuranceStatusId", preauth.insuranceStatusId);
                    cmd.Parameters.AddWithValue("@LoginTypeId", preauth.loginTypeId);
                    cmd.Parameters.AddWithValue("@TariffDeduction", preauth.tariffDeduction);
                    cmd.Parameters.AddWithValue("@LoginType", preauth.loginType);
                    cmd.Parameters.AddWithValue("@ReferToInsuranceYesNo", preauth.referToInsuranceYesNo);

                    // Output parameters
                    SqlParameter preAuthIDResult = new SqlParameter("@PreAuthIDResult", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(preAuthIDResult);

                    SqlParameter preAuthNoResult = new SqlParameter("@PreAuthNoResult", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(preAuthNoResult);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    int preAuthID = (int)cmd.Parameters["@PreAuthIDResult"].Value;
                    string preAuthNo = cmd.Parameters["@PreAuthNoResult"].Value != DBNull.Value ? (string)cmd.Parameters["@PreAuthNoResult"].Value : null;
                    if (preAuthID > 0)
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
                LogError("InsertPreAuthDetails", "PreAuthController", ex.Message, "PreAuthDal");
                return "An error occurred while processing the request.";
            }
        }

        public List<PreAuth> getPreAuth()   //Usp_GetPreAuthDetailsById

        {

            List<PreAuth> preauth = new List<PreAuth>();

            SqlConnection connection = new SqlConnection(conString);

            SqlDataAdapter da = new SqlDataAdapter("[Usp_GetPreAuthDetailsById]", connection);

            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try

            {

                DataTable dt = new DataTable();

                connection.Open();

                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)

                {

                    preauth.Add(new PreAuth
                    {
                        srNo = Convert.ToInt32(dr["SrNo"].ToString()),
                        preAuthID = Convert.ToInt32(dr["PreAuthID"]),
                        preAuthNumber = dr["PreAuthNumber"].ToString(),
                        insuredName = dr["InsuredName"].ToString(),

                        policyNo = dr["PolicyNo"].ToString(),
                        treatmentDate = dr["TreatmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["TreatmentDate"]) : (DateTime?)null,
                        caseType = dr["CaseTypeName"].ToString(),
                        caseTypeId = Convert.ToInt32(dr["CaseType"].ToString()),
                        patientStatus = dr["PatientStatus"].ToString(),
                        modifyDate = dr["ModifyDate"] != DBNull.Value ? Convert.ToDateTime(dr["ModifyDate"]) : (DateTime?)null,
                        // insuranceStatusId= Convert.ToInt32(dr["InsuranceStatusId"].ToString()),
                        insuranceCompanyId = Convert.ToInt32(dr["insuranceCompanyId"].ToString()),
                        intimationNo = (dr["IntimationNo"].ToString())


                    });

                }

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

            return preauth;

        }


        public List<PreAuth> SearchPreAuth(PreAuth p)
        {
            List<PreAuth> preauthList = new List<PreAuth>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("Usp_PreAuthSearch", connection)
            {
                SelectCommand = { CommandType = CommandType.StoredProcedure }
            };
            da.SelectCommand.CommandTimeout = 600;
            da.SelectCommand.Parameters.AddWithValue("@Status", p.status);
            da.SelectCommand.Parameters.AddWithValue("@PreAuthNumber", p.previousPreAuthNo);
            da.SelectCommand.Parameters.AddWithValue("@InsuredName", p.insuredName);
            da.SelectCommand.Parameters.AddWithValue("@InsuranceId", p.insuranceCompanyId);
            da.SelectCommand.Parameters.AddWithValue("@PolicyCardNo", p.policyNo);
            da.SelectCommand.Parameters.AddWithValue("@FromDate", p.fromDate);
            da.SelectCommand.Parameters.AddWithValue("@ToDate", p.toDate);
            da.SelectCommand.Parameters.AddWithValue("@OrderByCol", p.orderByCol);
            da.SelectCommand.Parameters.AddWithValue("@ProviderID", p.providerId);
            da.SelectCommand.Parameters.AddWithValue("@LoginTypeId", p.loginTypeId);
            da.SelectCommand.Parameters.AddWithValue("@StageId", p.stageId);
            da.SelectCommand.Parameters.AddWithValue("@PatientStatusId", p.patientStatusId);
            da.SelectCommand.Parameters.AddWithValue("@CaseType", p.caseType);
            da.SelectCommand.Parameters.AddWithValue("@AilmentId", p.ailmentId);
            da.SelectCommand.Parameters.AddWithValue("@InsuranceType", p.insuranceType);
            da.SelectCommand.Parameters.AddWithValue("@PHSFIRNo", p.phsFIRNo);
            da.SelectCommand.Parameters.AddWithValue("@EmployeeCode", p.employeeCode);
            da.SelectCommand.Parameters.AddWithValue("@InsuranceIDs", p.insuranceIds);

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                connection.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    preauthList.Add(new PreAuth
                    {
                        //srNo = Convert.ToInt32(dr["SRNO"].ToString()),
                        preAuthID = Convert.ToInt32(dr["PreAuthID"]),
                        preAuthNumber = dr["PreAuthNumber"].ToString(),
                        insuredName = dr["InsuredName"].ToString(),

                        policyNo = dr["PolicyNo"].ToString(),
                        treatmentDate = dr["TreatmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["TreatmentDate"]) : (DateTime?)null,
                        //  //caseType = dr["CaseTypeName"].ToString(),
                        //  // caseTypeId = Convert.ToInt32(dr["CaseType"].ToString()),
                        patientStatus = dr["PatientStatus"].ToString(),
                        modifyDate = dr["ModifyDate"] != DBNull.Value ? Convert.ToDateTime(dr["ModifyDate"]) : (DateTime?)null,
                        insuranceStatusId= dr["InsuranceStatusId"] != DBNull.Value ? Convert.ToInt32(dr["InsuranceStatusId"].ToString()) : (int?)null,
                        insuranceCompanyId = Convert.ToInt32(dr["insuranceCompanyId"].ToString()),
                        intimationNo = dr["IntimationNo"].ToString()
                    }) ;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (implementation of commondal.LogError not shown here)
                commondal.LogError("SearchPreAuth", "PreAuthSearchDAL", ex.Message, "SearchPreAuth");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return preauthList;
        }



    }
}
