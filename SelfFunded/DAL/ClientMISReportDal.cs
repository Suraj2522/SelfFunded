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
    public class ClientMISReportDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public ClientMISReportDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }

        public DataTable getClientMISReport(ClientMISReport misrpt)
        {
            List<ClientMISReport> report = new List<ClientMISReport>();
            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("Usp_GetClientMISReport", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;
                da.SelectCommand.Parameters.AddWithValue("@InsuranceCompanyId", misrpt.insuranceCompanyId);
                da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(misrpt.fromDate) ? (object)DBNull.Value : DateTime.Parse(misrpt.fromDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(misrpt.toDate) ? (object)DBNull.Value : DateTime.Parse(misrpt.toDate).ToString("dd-MM-yyyy"));


                connection.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetClientMISReport", "ClientMISReportController", ex.Message, "ClientMISReportDal");
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
