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
    public class BalanceSumInsuredReportDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public BalanceSumInsuredReportDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }

        public DataTable getBalanceSumInsuredReport(BalanceSumInsuredReport balsirpt)
        {
            List<BalanceSumInsuredReport> report = new List<BalanceSumInsuredReport>();
            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("GetSumInsuredData", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 1200;
                da.SelectCommand.Parameters.AddWithValue("@InsuranceCompanyId", balsirpt.insuranceCompanyId);
                da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(balsirpt.fromDate) ? (object)DBNull.Value : DateTime.Parse(balsirpt.fromDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(balsirpt.toDate) ? (object)DBNull.Value : DateTime.Parse(balsirpt.toDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@GroupPolicyId", balsirpt.groupPolicyId);               

                connection.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetBalanceSumInsuredReport", "BalanceSumInsuredReportController", ex.Message, "BalanceSumInsuredReportDal");
                return dt;
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
