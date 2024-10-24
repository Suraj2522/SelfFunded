using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Security.AccessControl;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

public class EnrollmentDal
{
    private readonly string _connectionString;
    private readonly IConfiguration _configuration;
    CommonDal commondal;
    RandomPass pass = new RandomPass();

    public EnrollmentDal(IConfiguration configuration, CommonDal common)
    {
        _configuration = configuration;
        _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        commondal = common;
    }

    public async Task<string> CheckEnrollmentUploadValidity(int groupPolicyId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.ExecuteScalarAsync<string>("CheckEnrollmentUploadValidity", new { GroupPolicyId = groupPolicyId }, commandType: CommandType.StoredProcedure);
        }
    }





    public DataTable GetDataTableFromExcel(string filePath)
    {
        DataTable dt = new DataTable();
        try
        {
            FileInfo fileInfo = new FileInfo(filePath);
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // Assuming the first row contains column headers
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        dt.Columns.Add(worksheet.Cells[1, col].Value?.ToString());
                    }

                    // Start adding data from the second row
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        DataRow newRow = dt.NewRow();
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            newRow[col - 1] = worksheet.Cells[row, col].Value?.ToString();
                        }
                        dt.Rows.Add(newRow);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            commondal.LogError("GetDataTableFromExcel", "EnrollmentController", ex.Message, "EnrollmentDal");
            // Handle exceptions or log them as needed
            Console.WriteLine("Error reading Excel file: " + ex.Message);
            throw;
        }

        return dt;
    }

    public DataTable TrimData(DataTable dataTable)
    {
        foreach (DataRow row in dataTable.Rows)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                if (row[column] is string)
                {
                    row[column] = row[column].ToString().Trim();
                }
            }
        }
        return dataTable;
    }

    public DataTable DeleteBlankRows(DataTable dataTable)
    {
        for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
        {
            if (dataTable.Rows[i].ItemArray.All(field => string.IsNullOrWhiteSpace(field?.ToString())))
            {
                dataTable.Rows[i].Delete();
            }
        }
        dataTable.AcceptChanges();
        return dataTable;
    }

    public async Task<DataTable> UploadData(DataTable dataTable, string fileName, int insuranceCompanyId, int? groupPolicyId, string loginType, string typeOfEnrollment)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var dt = new DataTable();

            try
            {
                string enrollmentNo;
                using (var batchIdCommand = new SqlCommand("SELECT CONVERT(VARCHAR(8), (SELECT dbo.fnGetDate()), 112) + '' + REPLACE(CONVERT(VARCHAR(8), (SELECT dbo.fnGetDate()), 108), ':', '')", connection))
                {
                    await connection.OpenAsync();
                    enrollmentNo = (string)await batchIdCommand.ExecuteScalarAsync();
                    await connection.CloseAsync();
                }

                using (var cmd = new SqlCommand("Usp_ImportExcelEnrollment", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1200;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@FileName", fileName);
                    cmd.Parameters.AddWithValue("@InsurerID", insuranceCompanyId);
                    cmd.Parameters.AddWithValue("@GroupPolicyId", groupPolicyId);
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(loginType));
                    cmd.Parameters.AddWithValue("@typeOfEnrollment", typeOfEnrollment);
                    cmd.Parameters.AddWithValue("@EnrollmentNo", enrollmentNo);

                    // Manually add the TVP parameter
                    var tvpParam = new SqlParameter("@dtExcel", SqlDbType.Structured)
                    {
                        TypeName = "TableTypeImportData", // Your TVP type name
                        Value = dataTable
                    };
                    cmd.Parameters.Add(tvpParam);

                    var da = new SqlDataAdapter(cmd);
                    await connection.OpenAsync();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("UploadData", "EnrollmentController", ex.Message, "EnrollmentDal");
                Console.WriteLine($"Error uploading data: {ex.Message}");
                throw;
            }

            return dt;
        }
    }

  

  

    //----------------------------------------------------------------------------------------------------

    public List<Enrollment> getAllEnrollment()
    {
        List<Enrollment> enroll = new List<Enrollment>();
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter("GetImportExcelLog", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            connection.Open();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                enroll.Add(new Enrollment
                {
                    srNo = Convert.ToInt32(dr["SRNO"]),
                    fileName = dr["FileName"].ToString(),
                    totalRecords = Convert.ToInt32(dr["TotalRecord"]),
                    importId = Convert.ToInt32(dr["importID"]),
                    validation = Convert.ToInt32(dr["validation"]),
                    proceed = Convert.ToInt32(dr["proceed"]),
                    typeOfEnrollment = dr["typeOfEnrollment"].ToString(),
                    enrollmentNo = dr["EnrollmentNo"].ToString()

                }); ;
            }
            return enroll;
        }
        catch (Exception ex)
        {
            commondal.LogError("getAllEnrollment", "EnrollmentController", ex.Message, "EnrollmentDal");
            return enroll;
        }
        finally
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public byte[] ValidateImport(int importId)
    {
        List<Enrollment> results = new List<Enrollment>();
        SqlConnection connection = null;
        DataTable dt = new DataTable();

        try
        {
            connection = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter("Usp_ValidationOfEnrollmentExcelData", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 1200;
            da.SelectCommand.Parameters.AddWithValue("@ImportID", importId);

            SqlParameter errCountParam = new SqlParameter("@result", SqlDbType.VarChar, 50);
            errCountParam.Direction = ParameterDirection.Output;
            da.SelectCommand.Parameters.Add(errCountParam);

            connection.Open();
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {

                Enrollment enrollment = new Enrollment();

                results.Add(enrollment);
            }
            string errCountValue = da.SelectCommand.Parameters["@result"].Value.ToString();
            // Set the license context

            if (errCountValue.Equals("0"))
            {
                return null;
            }
            else
            {


                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Generate Excel file
                using (var package = new OfficeOpenXml.ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("ValidationResults");

                    // Load data table into the worksheet
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);

                    return package.GetAsByteArray();
                }
            }
        }
        catch (Exception ex)
        {
            commondal.LogError("ValidateImport", "EnrollmentController", ex.Message, "EnrollmentDal");
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
        finally
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }




    public string ProceedToUpload(int importId)
    {
        List<Enrollment> results = new List<Enrollment>();
        SqlConnection connection = null;

        try
        {
            connection = new SqlConnection(_connectionString);

           

            SqlDataAdapter da = new SqlDataAdapter("Usp_ImportEnrollmentExcelToDatabaseTable", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 1200;
            da.SelectCommand.Parameters.AddWithValue("@ImportID", importId);
          // 

            // Adding output parameter @ErrCount
            SqlParameter errCountParam = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500);
            errCountParam.Direction = ParameterDirection.Output;
            da.SelectCommand.Parameters.Add(errCountParam);

            DataTable dt = new DataTable();
            connection.Open();
            da.Fill(dt);

            // Process the result set if needed
            foreach (DataRow row in dt.Rows)
            {
                Enrollment enrollment = new Enrollment();
                // Populate enrollment object here
                results.Add(enrollment);
            }

            string errCountValue = da.SelectCommand.Parameters["@ErrorMessage"].Value.ToString();

            return errCountValue;
        }
        catch (Exception ex)
        {
            commondal.LogError("ProceedToUpload", "EnrollmentController", ex.Message, "EnrollmentDal");
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
        finally
        {
            // Clean up resources
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    public async Task<int> deleteImportedData(int importId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@importID", importId);

            try
            {
                var result = await connection.ExecuteAsync("Usp_EnrollmentExcelDataDel", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                commondal.LogError("deleteImportedData", "EnrollmentController", ex.Message, "EnrollmentDal");

                Console.WriteLine($"Error deleting data: {ex.Message}");
                throw;
            }
        }


    }
    public List<Enrollment> getPolicyDate(int insuranceId)
    {
        List<Enrollment> list = new List<Enrollment>();
        SqlConnection connection = new SqlConnection(_connectionString);
        SqlDataAdapter da = new SqlDataAdapter("SP_GetPolicyDate", connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddWithValue("@InsuranceId", insuranceId);

        try
        {
            DataTable dt = new DataTable();
            connection.Open();
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Enrollment
                {
                    insuranceId = Convert.ToInt32(dr["InsuranceId"]),
                    validFrom = Convert.ToDateTime(dr["ValidFrom"]),
                    validTo = Convert.ToDateTime(dr["ValidTo"]),

                });
            }
        }
        catch (Exception ex)
        {
            commondal.LogError("getPolicyDate", "EnrollmentController", ex.Message, "EnrollmentDal");
            Console.WriteLine($"Error in getSubmenu method: {ex.Message}");
            throw; // Re-throw the exception to propagate it to the caller
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        return list;
    }

    public List<Enrollment> searchEnrollment(Enrollment enr)
    {
        List<Enrollment> enroll = new List<Enrollment>();
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter("Usp_EnrollmentSearch", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@InsuranceId", enr.insuranceCompanyId);
            da.SelectCommand.Parameters.AddWithValue("@PolicyNo", enr.policyNo);
            da.SelectCommand.Parameters.AddWithValue("@PlanId", enr.planId);
            da.SelectCommand.Parameters.AddWithValue("@InsuredName", enr.insuredName);
            da.SelectCommand.Parameters.AddWithValue("@PolicyHolder", enr.policyHolder);
            da.SelectCommand.Parameters.AddWithValue("@EmployeeCode", enr.employeeCode);
            DataTable dt = new DataTable();
            connection.Open();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                enroll.Add(new Enrollment
                {
                    enrollmentId = Convert.ToInt32(dr["EnrollmentId"]),
                    insuranceCompanyId = dr["InsuranceCompanyId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["InsuranceCompanyId"]) : null,
                    //uWOfficeCode = dr["UWOfficeCode"].ToString(),
                    //uWOfficeName = dr["UWOfficeName"].ToString(),
                    policyNo = dr["PolicyNo"].ToString(),
                    srNo= Convert.ToInt32(dr["SrNo"]),
                    //policyTypeId = dr["PolicyTypeId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["PolicyTypeId"]) : null,
                    //group_CorporateName = dr["Group_CorporateName"].ToString(),
                    //productId = dr["ProductId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["ProductId"]) : null,
                    //planId = dr["PlanId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["PlanId"]) : null,
                    //familyId = dr["FamilyId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["FamilyId"]) : null,
                    //memberId = dr["MemberId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["MemberId"]) : null,
                    //policyTenure = dr["PolicyTenure"].ToString(),
                    //policyStartDate = dr["PolicyStartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["PolicyStartDate"]) : null,
                    //policyEndDate = dr["PolicyEndDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["PolicyEndDate"]) : null,
                    //policyPremium = dr["PolicyPremium"] != DBNull.Value ? (decimal?)Convert.ToDecimal(dr["PolicyPremium"]) : null,
                    //premiumCurrency = dr["PremiumCurrency"].ToString(),
                    //insuredName = dr["InsuredName"].ToString(),
                    //policyHolder = dr["PolicyHolder"].ToString(),
                    //dateOfBirth = dr["DateOfBirth"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["DateOfBirth"]) : null,
                    //gender = dr["Gender"].ToString(),
                    //age = dr["Age"] != DBNull.Value ? (int?)Convert.ToInt32(dr["Age"]) : null,
                    //sumInsured = dr["SumInsured"] != DBNull.Value ? (decimal?)Convert.ToDecimal(dr["SumInsured"]) : null,
                    //sIType = dr["SIType"] != DBNull.Value ? (int?)Convert.ToInt32(dr["SIType"]) : null,
                    //primaryAddress1 = dr["PrimaryAddress1"].ToString(),
                    //primaryAddress2 = dr["PrimaryAddress2"].ToString(),
                    //primaryCity = dr["PrimaryCity"].ToString(),
                    //primaryState = dr["PrimaryState"] != DBNull.Value ? (int?)Convert.ToInt32(dr["PrimaryState"]) : null,
                    //primaryCountry = dr["PrimaryCountry"] != DBNull.Value ? (int?)Convert.ToInt32(dr["PrimaryCountry"]) : null,
                    //primaryPin = dr["PrimaryPin"] != DBNull.Value ? (int?)Convert.ToInt32(dr["PrimaryPin"]) : null,
                    //primaryContactPhone = dr["PrimaryContactPhone"].ToString(),
                    //primaryContactMobile = dr["PrimaryContactMobile"].ToString(),
                    //primaryContactFax = dr["PrimaryContactFax"].ToString(),
                    //primaryContactEmail = dr["PrimaryContactEmail"].ToString(),
                    //passportNo = dr["PassportNo"].ToString(),
                    //nationality = dr["Nationality"].ToString(),
                    //uploadedPolicyCopy = dr["UploadedPolicyCopy"].ToString(),
                    //createDate = dr["CreateDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["CreateDate"]) : null,
                    //modifyDate = dr["ModifyDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["ModifyDate"]) : null,
                    //createByUserId = dr["CreateByUserId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["CreateByUserId"]) : null,
                    //modifyByUserId = dr["ModifyByUserId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["ModifyByUserId"]) : null,
                    //cancelledCheque = dr["CancelledCheque"].ToString(),
                    //intimationNo = dr["IntimationNo"] != DBNull.Value ? (int?)Convert.ToInt32(dr["IntimationNo"]) : null,
                    //chequeInFavourOf = dr["ChequeInFavourOf"].ToString(),
                    //policyIssueDate = dr["PolicyIssueDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["PolicyIssueDate"]) : null,
                    //maritalStatus = dr["MaritalStatus"].ToString(),
                    //recStatus = dr["RecStatus"].ToString(),
                    //uploadedDate = dr["UploadedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["UploadedDate"]) : null,
                    //employeeCode = dr["EmployeeCode"].ToString(),
                    //relationId = dr["RelationId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["RelationId"]) : null,
                    //remarks = dr["Remarks"].ToString(),
                    //groupPolicyId = dr["GroupPolicyId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["GroupPolicyId"]) : null,
                    //certificateNumber = dr["CertificateNumber"].ToString(),
                    //tripId = dr["TripId"].ToString(),
                    //tripType = dr["TripType"].ToString(),
                    //tripDuration = dr["TripDuration"] != DBNull.Value ? (int?)Convert.ToInt32(dr["TripDuration"]) : null,
                    //isCertificateEmailSent = dr["IsCertificateEmailSent"] != DBNull.Value ? (bool?)Convert.ToBoolean(dr["IsCertificateEmailSent"]) : null,
                    //isEcardGenerated = dr["IsEcardGenerated"] != DBNull.Value ? (bool?)Convert.ToBoolean(dr["IsEcardGenerated"]) : null,
                    //branchId = dr["BranchId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["BranchId"]) : null,
                    //isWelcomeEmailSent = dr["IsWelcomeEmailSent"] != DBNull.Value ? (bool?)Convert.ToBoolean(dr["IsWelcomeEmailSent"]) : null,
                    //dateOfJoining = dr["DateOfJoining"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["DateOfJoining"]) : null,
                    //subMemberId = dr["SubMemberId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["SubMemberId"]) : null,
                    //policyCategory = dr["PolicyCategory"].ToString(),
                    //unitId = dr["UnitId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["UnitId"]) : null,
                    //enrollmentLogId = dr["EnrollmentLogId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["EnrollmentLogId"]) : null,
                    //policyHolder1 = dr["PolicyHolder1"].ToString(),
                    //insuredName1 = dr["InsuredName1"].ToString()

                });             }
            return enroll;
        }
        catch (Exception ex)
        {
            commondal.LogError("SearchEnrollment", "EnrollmentController", ex.Message, "EnrollmentDal");
            return enroll;
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


