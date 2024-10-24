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


namespace SelfFunded.DAL
{
    public class PaymentUploadSelfFundedDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;
        RandomPass pass = new RandomPass();

        public PaymentUploadSelfFundedDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
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

        public async Task<string> UploadData(DataTable dataTable, string fileName, string loginType)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var dt = new DataTable();
                string message = ""; // Initialize output message

                try
                {

                    using (SqlCommand command = new SqlCommand("dbo.Usp_ImportExcelTempPaymentSelfFunded", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        command.Parameters.AddWithValue("@FileName", fileName);
                        command.Parameters.AddWithValue("@dtExcel", dataTable);
                        command.Parameters.AddWithValue("@UserID", loginType);

                        // Add output parameter
                        SqlParameter outputMessage = new SqlParameter("@message", SqlDbType.VarChar, 100);
                        outputMessage.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputMessage);

                        connection.Open();
                        await command.ExecuteNonQueryAsync();

                        // Capture the output message
                        message = outputMessage.Value.ToString();
                    }


                }

                catch (Exception ex)
                {
                    commondal.LogError("UploadData", "PaymentUploadSelfFunded", ex.Message, "PaymentUploadSelfFundedDal");
                    Console.WriteLine($"Error uploading data: {ex.Message}");
                    throw;
                }

                // Return both the DataTable and the output message
                return (message);
            }
        }

        public List<Dictionary<string, object>> getPaymentLog()
        {
            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter("[GetImportExcelPaymentLogSelfFunded]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
               
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    var rowDict = new Dictionary<string, object>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        rowDict[column.ColumnName] = row[column];
                    }
                    report.Add(rowDict);
                }


            }
            catch (Exception ex)
            {
                commondal.LogError("getPaymentLog", "PaymentUploadSelfFunded", ex.Message, "PaymentUploadSelfFundedDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return report;
        }
        public byte[] ValidateImport(int importId)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("Usp_ValidationOfExcelDataSelfFunded", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 1200;
                da.SelectCommand.Parameters.AddWithValue("@ImportID", importId);

                connection.Open();
                da.Fill(dt);


                // Set the license context

                if (dt.Rows.Count == 0)
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
                commondal.LogError("ValidateImport", "PaymentUploadSelfFunded", ex.Message, "PaymentUploadSelfFundedDal");
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
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                SqlCommand command = new SqlCommand("[Usp_ImportExcelToDatabaseTableSelfFunded]", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 1200
                };

                // Adding input parameters
                command.Parameters.AddWithValue("@ImportID", importId);
                command.Parameters.AddWithValue("@UserId", 159);

                // Adding output parameter @ErrorMessage
                SqlParameter errCountParam = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 500)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(errCountParam);

                connection.Open();

                // Execute the stored procedure
                command.ExecuteNonQuery();

                // Retrieve the output parameter value
                string errCountValue = errCountParam.Value != null ? errCountParam.Value.ToString() : string.Empty;

                return errCountValue;
            }
            catch (Exception ex)
            {
                commondal.LogError("ProceedToUpload", "PaymentUploadSelfFunded", ex.Message, "PaymentUploadSelfFundedDal");
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
        public async Task<int> deleteData(int importId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@importID", importId);

                try
                {
                    var result = await connection.ExecuteAsync("Usp_ImportedExcelDataDelSelfFunded", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception ex)
                {
                    commondal.LogError("deleteData", "PaymentUploadSelfFunded", ex.Message, "_paymentUploadSelfFundedDal");

                    Console.WriteLine($"Error deleting data: {ex.Message}");
                    throw;
                }
            }


        }
    }
}

    
