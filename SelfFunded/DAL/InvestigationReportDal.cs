using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using Dapper;
using EvoPdf;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using SelfFunded.Models;

namespace SelfFunded.DAL
{
    public class InvestigationReportDal
    {
        private readonly string _connectionString;
        private readonly string _folderName;
        private readonly string _licenseKey;
        private readonly string _filePath;
        private readonly CommonDal _commondal;

        public InvestigationReportDal(IConfiguration configuration, CommonDal common, string folderName, string licenseKey)
        {
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            _folderName = folderName;
            _licenseKey = licenseKey;
            _filePath = configuration["DocumentUpload"] ?? "";
            _commondal = common;
        }

        public List<Dictionary<string, object>> GetInvestigationReport(InvestigationReport invrpt)
        {
            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var dt = new DataTable();
                try
                {
                    var da = new SqlDataAdapter("Usp_GetInvestigationDetails", connection)
                    {
                        SelectCommand =
                        {
                            CommandType = CommandType.StoredProcedure
                            
                        }
                    };
                    da.SelectCommand.CommandTimeout = 600;
                    da.SelectCommand.Parameters.AddWithValue("@InsuranceId", invrpt.insuranceId);
                    da.SelectCommand.Parameters.AddWithValue("@InsuredName", invrpt.insuredName);
                    da.SelectCommand.Parameters.AddWithValue("@ClaimNo", invrpt.claimId);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(invrpt.fromDate) ? (object)DBNull.Value : DateTime.Parse(invrpt.fromDate).ToString("dd-MM-yyyy"));
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(invrpt.toDate) ? (object)DBNull.Value : DateTime.Parse(invrpt.toDate).ToString("dd-MM-yyyy"));
                    da.SelectCommand.Parameters.AddWithValue("@OrderByCol", invrpt.orderByCol = "");

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
                    _commondal.LogError("GetInvestigationReport", "InvestigationReportDal", ex.Message, "InvestigationReportDal");
                }
            }
            return report;
        }
     
public List<Dictionary<string, object>> getInvestigationDetails(int claimId)

        {

            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();

            SqlConnection connection = new SqlConnection(_connectionString);

            SqlDataAdapter da = new SqlDataAdapter("Usp_GetInvestigationReport", connection);

            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 120;

            da.SelectCommand.Parameters.AddWithValue("@ClaimId", claimId);

            try

            {

                DataTable dt = new DataTable();

                connection.Open();

                da.Fill(dt);

                foreach (DataRow row in dt.Rows)

                {

                    Dictionary<string, object> rowDict = new Dictionary<string, object>();

                    foreach (DataColumn column in dt.Columns)

                    {

                        rowDict[column.ColumnName] = row[column];

                    }

                    report.Add(rowDict);

                }

                // Call CreatePdf before returning the report data

                //CreatePdf(claimId, report);

            }
            catch (Exception ex)
            {

                _commondal.LogError("GetInvestigationReport", "InvestigationReportController", ex.Message, "InvestigationReportDal");

                Console.WriteLine($"Error in getInvestigationDetails method: {ex.Message}");

                throw; // Re-throw the exception to propagate it to the caller

            }

            finally

            {

                if (connection.State == ConnectionState.Open)

                {

                    connection.Close();

                }

            }

            return report;

        }


    }
}
