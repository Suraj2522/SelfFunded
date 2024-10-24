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
    public class DailyProductivityReportDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public DailyProductivityReportDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }

        public List<Dictionary<string, object>> getDailyProductivityReport(DailyProductivityReport dailyrpt)
        {
            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();
            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("Usp_ImportExcelToDatabaseTableSelfFunded", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;
                da.SelectCommand.Parameters.AddWithValue("@InsuranceId", dailyrpt.insuranceId);
                da.SelectCommand.Parameters.AddWithValue("@UserCode", dailyrpt.userCode=0);
                da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(dailyrpt.fromDate) ? (object)DBNull.Value : DateTime.Parse(dailyrpt.fromDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(dailyrpt.toDate) ? (object)DBNull.Value : DateTime.Parse(dailyrpt.toDate).ToString("dd-MM-yyyy") );


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
                commondal.LogError("GetDailyProductivityReport", "DailyProductivityReportController", ex.Message, "DailyProductivityReportDal");
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
     
    }
}
