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
    public class SavingReportDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public SavingReportDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }

        public DataTable getSavingReport(SavingReport saverpt)
        {
            List<SavingReport> report = new List<SavingReport>();
            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("Usp_GetPreauthClaimsSavingReport", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;

                // 0,'','','01/07/2022','02/07/2022'
                da.SelectCommand.Parameters.AddWithValue("@InsuranceCompanyId", saverpt.insuranceCompanyId);
                da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(saverpt.fromDate) ? (object)DBNull.Value : DateTime.Parse(saverpt.fromDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(saverpt.toDate) ? (object)DBNull.Value : DateTime.Parse(saverpt.toDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@PreAuthNumber", saverpt.preAuthNumber);
                da.SelectCommand.Parameters.AddWithValue("@PatientName", saverpt.patientName );               

                connection.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                commondal.LogError("GetSavingReport", "SavingReportController", ex.Message, "SavingReportDal");
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
