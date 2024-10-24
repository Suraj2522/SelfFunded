using SelfFunded.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SelfFunded.DAL;
using System.Security.AccessControl;
using System.Security.Claims;

public class IntimationSheetInboundDal
{
    private readonly CommonDal _commondal;
    private readonly string _conString;

    public IntimationSheetInboundDal(IConfiguration configuration)
    {
        _commondal = new CommonDal(configuration);
        _conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
    }

    public string insertIntimationSheetInbound(IntimationSheetInbound intimation)
    {
        SqlConnection connection = null;

        try
        {
            using (connection = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("[SPIntimationIBInsertUpdate]", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters as per the stored procedure
                cmd.Parameters.AddWithValue("@IntimationId", intimation.intimationId);
                cmd.Parameters.AddWithValue("@AlternateContactNo", intimation.alternateContactNo);
                cmd.Parameters.AddWithValue("@CaseType", intimation.caseType);
                cmd.Parameters.AddWithValue("@ClaimTypeID", intimation.claimTypeId);
                cmd.Parameters.AddWithValue("@ContactNo", intimation.contactNo);
                cmd.Parameters.AddWithValue("@Diagnosis", intimation.diagnosis);
                // cmd.Parameters.AddWithValue("@EmployeeTypeId", intimation.employeeTypeId);
                cmd.Parameters.AddWithValue("@HospitalAddress", intimation.hospitalAddress1);
                cmd.Parameters.AddWithValue("@HospitalContactNo", intimation.hospitalContactNo);
                cmd.Parameters.AddWithValue("@HospitalName", intimation.hospitalName);
                cmd.Parameters.AddWithValue("@InsuranceCompanyId", intimation.insuranceCompanyId);
                cmd.Parameters.AddWithValue("@InsuredName", intimation.insuredName);
                cmd.Parameters.AddWithValue("@IntimationFrom", intimation.intimationFrom);
                cmd.Parameters.AddWithValue("@IntimationType", intimation.intimationType);
                cmd.Parameters.AddWithValue("@PersonContactNo", intimation.personContactNo);
                cmd.Parameters.AddWithValue("@PersonEmailId", intimation.personEmailId);
                cmd.Parameters.AddWithValue("@PolicyEndDate", intimation.policyEndDate);
                cmd.Parameters.AddWithValue("@PolicyNo", intimation.policyNo);
                cmd.Parameters.AddWithValue("@PolicyStartDate", intimation.policyStartDate);
                cmd.Parameters.AddWithValue("@PrimaryMember", intimation.primaryMember);
                //   cmd.Parameters.AddWithValue("@RelationId", intimation.relationId);
                cmd.Parameters.AddWithValue("@Remarks", intimation.remarks);
                cmd.Parameters.AddWithValue("@RequestType", intimation.requestType);
                //   cmd.Parameters.AddWithValue("@Status", intimation.recStatus);
                cmd.Parameters.AddWithValue("@Treatment", intimation.treatment);
                cmd.Parameters.AddWithValue("@TreatmentDate", intimation.treatmentDate);

                cmd.Parameters.Add("@IntimationIdResult", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@IntimationNoResult", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                connection.Open();
                int result = cmd.ExecuteNonQuery();
                string intimationIdResult = cmd.Parameters["@IntimationIdResult"].Value.ToString();
                string intimationNoResult = cmd.Parameters["@IntimationNoResult"].Value.ToString();
                connection.Close();

                return "Data add successfully";
            }
        }
        catch (Exception ex)
        {
            _commondal.LogError("insertIntimationSheetInbound", "IntimationSheetInboundController", ex.Message, "IntimationSheetInboundDal");
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

    public List<IntimationSheetInbound> getIntimationDetails(IntimationSheetInbound ISI)
    {
        
        List<IntimationSheetInbound> clmdtls = new List<IntimationSheetInbound>();
        SqlConnection connection = new SqlConnection(_conString);
        SqlDataAdapter da = new SqlDataAdapter("[Usp_IntimationINBSearch]", connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddWithValue("@InsuranceID", ISI.insuranceCompanyId);
        da.SelectCommand.Parameters.AddWithValue("@CaseType", ISI.caseType);
        da.SelectCommand.Parameters.AddWithValue("@InsuredName", ISI.insuredName);
        da.SelectCommand.Parameters.AddWithValue("@Intimation", ISI.intimationNo);
        da.SelectCommand.Parameters.AddWithValue("@FromDate", ISI.fromDate);
        da.SelectCommand.Parameters.AddWithValue("@ToDate", ISI.toDate);
        da.SelectCommand.Parameters.AddWithValue("@OrderByCol", ISI.orderByCol);
        da.SelectCommand.Parameters.AddWithValue("@LoginTypeId", ISI.loginTypeId);
        da.SelectCommand.Parameters.AddWithValue("@InsuranceIDs", ISI.insuranceIDs);


        try
        {
            DataTable dt = new DataTable();
            connection.Open();
            da.Fill(dt);
          
                foreach (DataRow dr in dt.Rows)
                {
                    clmdtls.Add(new IntimationSheetInbound
                    {

                        srNo = Convert.ToInt32(dr["SrNo"]),
                        intimationId = Convert.ToInt32(dr["intimationId"]),
                        insuranceCompanyId = Convert.ToInt32(dr["insuranceCompanyId"]),
                        intimationNo = dr["IntimationNo"].ToString(),
                        insuredName = dr["InsuredName"].ToString(),
                        primaryMember = dr["PrimaryMember"].ToString(),
                        dateOfIntimation = dr["DateOfIntimation"] != DBNull.Value ? Convert.ToDateTime(dr["DateOfIntimation"]) : (DateTime?)null,
                        timeOfIntimation = dr["TimeOfIntimation"].ToString(),
                        policyNo = dr["PolicyNo"].ToString(),
                        caseType = dr["CaseType"].ToString(),
                        claimTypeId = Convert.ToInt32(dr["ClaimTypeId"].ToString()),
                        claimType = dr["ClaimType"].ToString(),

                    });
                }
          
            
        }
        catch (Exception ex)
        {
            _commondal.LogError("GetCashlessType", "ClaimsController", ex.Message, "ClaimsDal.getCashlessType");
            throw; // Re-throwing to propagate the exception to the caller
        }
        finally
        {
            connection.Close();
        }

        return clmdtls;
    }

    


}

