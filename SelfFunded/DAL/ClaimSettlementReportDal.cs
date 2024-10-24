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
    public class ClaimSettlementReportDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public ClaimSettlementReportDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }

        public DataTable getClaimSettlementReport(ClaimSettlementReport setrpt)
        {
            List<ClaimSettlementReport> report = new List<ClaimSettlementReport>();
            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("GetClaimSettlementReport", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;
                da.SelectCommand.Parameters.AddWithValue("@InsuranceID", setrpt.insuranceID);
                da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(setrpt.fromDate) ? (object)DBNull.Value : DateTime.Parse(setrpt.fromDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(setrpt.toDate) ? (object)DBNull.Value : DateTime.Parse(setrpt.toDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@InsuredName", setrpt.insuredName);
                da.SelectCommand.Parameters.AddWithValue("@ClaimNO", setrpt.claimNO);
                da.SelectCommand.Parameters.AddWithValue("@ProviderNo", setrpt.providerNo);

                connection.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetClaimSettlementReport", "ClaimSettlementReportController", ex.Message, "ClaimSettlementReportDal");
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
