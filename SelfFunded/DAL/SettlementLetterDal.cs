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
using System.Security.Claims;

namespace SelfFunded.DAL
{
    public class SettlementLetterDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public SettlementLetterDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }


        public List<Dictionary<string, object>> getSettlementLetterDetails(SettlementLetter stldtls)
        {

            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();

            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("usp_GetSettlementDetailsSelfFunded", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;
                da.SelectCommand.Parameters.AddWithValue("@ClaimNo", stldtls.claimNo);
                da.SelectCommand.Parameters.AddWithValue("@EmployeeCode", stldtls.employeeCode);
                da.SelectCommand.Parameters.AddWithValue("@InsuranceId", stldtls.insuranceCompanyId);

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
                return report;

            }
            catch (Exception ex)
            {
                commondal.LogError("GetSettlementLetterDetails", "SettlementLetterController", ex.Message, "SettlementLetterDal");
                return report;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public List<List<Dictionary<string, object>>> SettlementLetter(int stldtls)
        {
            List<List<Dictionary<string, object>>> report = new List<List<Dictionary<string, object>>>();
            SqlConnection connection = null;
            List<DataTable> dataTables = new List<DataTable>();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlCommand command = new SqlCommand("Usp_GetSettlementLetterSelfFunded", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 600;
                command.Parameters.AddWithValue("@ClaimId", stldtls);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Read multiple result sets
                    do
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader); // Load each result set into a DataTable
                        dataTables.Add(dt); // Add the DataTable to the list
                    } while (!reader.IsClosed && reader.NextResult());
                }

                // Process each DataTable in the list
                foreach (var dt in dataTables)
                {
                    List<Dictionary<string, object>> tableReport = new List<Dictionary<string, object>>();

                    foreach (DataRow row in dt.Rows)
                    {
                        Dictionary<string, object> rowDict = new Dictionary<string, object>();
                        foreach (DataColumn column in dt.Columns)
                        {
                            rowDict[column.ColumnName] = row[column];
                        }
                        tableReport.Add(rowDict);
                    }

                    // Add the processed table report to the main report list
                    report.Add(tableReport);
                }

                return report;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetSettlementLetter", "SettlementLetterController", ex.Message, "SettlementLetterDal");
                return report; // Return the current state of the report in case of an exception
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