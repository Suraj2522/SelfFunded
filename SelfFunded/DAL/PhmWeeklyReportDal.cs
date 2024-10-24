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
    public class PhmWeeklyReportDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public PhmWeeklyReportDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }

        public DataTable getPhmWeeklyReport(PhmWeeklyReport rpts)
        {
            List<PhmWeeklyReport> report = new List<PhmWeeklyReport>();
            SqlConnection connection = null;
            DataTable dt = new DataTable();
           
                try
                {
                    connection = new SqlConnection(_connectionString);
                    SqlDataAdapter da = new SqlDataAdapter("GetPHMWeeklyReport", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 600;
                    da.SelectCommand.Parameters.AddWithValue("@InsuranceID", rpts.insuranceId);
                da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(rpts.fromDate) ? (object)DBNull.Value : DateTime.Parse(rpts.fromDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(rpts.toDate) ? (object)DBNull.Value : DateTime.Parse(rpts.toDate).ToString("dd-MM-yyyy"));

                connection.Open();
                    da.Fill(dt);
                    return dt;
                }
                catch (Exception ex)
                {
                    commondal.LogError("GetPhmWeeklyReport", "PhmWeeklyReportController", ex.Message, "PhmWeeklyReportDal");
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
