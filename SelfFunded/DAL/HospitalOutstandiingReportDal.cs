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
    public class HospitalOutstandiingReportDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public HospitalOutstandiingReportDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }

        public DataTable getHospitalOutstandingReport(HospitalOutstandingReport hsprpts)
        {
            List<HospitalOutstandingReport> report = new List<HospitalOutstandingReport>();
            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("GetOutStandingReport", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;
                da.SelectCommand.Parameters.AddWithValue("@InsuranceID", hsprpts.insuranceID);
                da.SelectCommand.Parameters.AddWithValue("@DebitNoteNo", hsprpts.debitNoteNo="");
                da.SelectCommand.Parameters.AddWithValue("@ClaimNO", hsprpts.claimNO);
                da.SelectCommand.Parameters.AddWithValue("@OutwardNo", hsprpts.outwardNo);
                da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(hsprpts.fromDate) ? (object)DBNull.Value : DateTime.Parse(hsprpts.fromDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(hsprpts.toDate) ? (object)DBNull.Value : DateTime.Parse(hsprpts.toDate).ToString("dd-MM-yyyy"));
                connection.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetHospitalOutstandingReport", "HospitalOutstandingReportController", ex.Message, "HospitalOutstandiingReportDal");
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
