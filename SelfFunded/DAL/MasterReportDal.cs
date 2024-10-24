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
    public class MasterReportDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public MasterReportDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }



        public DataTable getMasterReport(MasterReport rpt)
        {
            List<MasterReport> report = new List<MasterReport>();
            SqlConnection connection = null;
            DataTable dt = new DataTable();
          //  rpt.insuranceType = "isselfFunded";

            if (rpt.insuranceType =="isselfFunded")
            {
                try
                {
                    connection = new SqlConnection(_connectionString);
                    SqlDataAdapter da = new SqlDataAdapter("Usp_GetPreauthClaimsReport_SelfFunded", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 600;
                    da.SelectCommand.Parameters.AddWithValue("@InsuranceCompanyId", rpt.insuranceCompanyId);
                    da.SelectCommand.Parameters.AddWithValue("@PreAuthNumber", rpt.preAuthNumber);
                    da.SelectCommand.Parameters.AddWithValue("@DiagnosisId", rpt.diagnosisId);
                    da.SelectCommand.Parameters.AddWithValue("@InsuredName", rpt.insuredName);
                    da.SelectCommand.Parameters.AddWithValue("@PolicyNo", rpt.policyNo);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", rpt.fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", rpt.toDate);
                    da.SelectCommand.Parameters.AddWithValue("@Type", rpt.type);
                    da.SelectCommand.Parameters.AddWithValue("@ClaimStatus", rpt.claimStatus);
                    da.SelectCommand.Parameters.AddWithValue("@ProviderId", rpt.providerId);
                    da.SelectCommand.Parameters.AddWithValue("@AilmentId", rpt.ailmentId);
                    da.SelectCommand.Parameters.AddWithValue("@InsuranceType", rpt.insuranceType);
                    da.SelectCommand.Parameters.AddWithValue("@GroupPolicyId", rpt.groupPolicyId);
                    da.SelectCommand.Parameters.AddWithValue("@LoginTypeId", rpt.loginTypeId);
                    da.SelectCommand.Parameters.AddWithValue("@InsuranceIDs", rpt.insuranceIDs);
                    connection.Open();
                    da.Fill(dt);
                    return dt;

                }
                catch (Exception ex)
                {
                    commondal.LogError("SearchEnrollment", "EnrollmentController", ex.Message, "EnrollmentDal");
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
            else
            {
                try
                {
                    connection = new SqlConnection(_connectionString);
                    SqlDataAdapter da = new SqlDataAdapter("Usp_GetPreauthClaimsReport", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 600;

                    da.SelectCommand.Parameters.AddWithValue("@InsuranceCompanyId", rpt.insuranceCompanyId);
                    da.SelectCommand.Parameters.AddWithValue("@PreAuthNumber", rpt.preAuthNumber);
                    da.SelectCommand.Parameters.AddWithValue("@DiagnosisId", rpt.diagnosisId);
                    da.SelectCommand.Parameters.AddWithValue("@InsuredName", rpt.insuredName);
                    da.SelectCommand.Parameters.AddWithValue("@PolicyNo", rpt.policyNo );
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(rpt.fromDate) ? (object)DBNull.Value : DateTime.Parse(rpt.fromDate).ToString("dd-MM-yyyy"));
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(rpt.toDate) ? (object)DBNull.Value : DateTime.Parse(rpt.toDate).ToString("dd-MM-yyyy"));

                    da.SelectCommand.Parameters.AddWithValue("@Type", rpt.type);
                    da.SelectCommand.Parameters.AddWithValue("@ClaimStatus", rpt.claimStatus);
                    da.SelectCommand.Parameters.AddWithValue("@ProviderId", rpt.providerId);
                    da.SelectCommand.Parameters.AddWithValue("@AilmentId", rpt.ailmentId);
                    da.SelectCommand.Parameters.AddWithValue("@InsuranceType", rpt.insuranceType);
                    da.SelectCommand.Parameters.AddWithValue("@GroupPolicyId", rpt.groupPolicyId);

                    connection.Open();
                    da.Fill(dt);
                    return dt;

                }
                catch (Exception ex)
                {
                    commondal.LogError("SearchEnrollment", "EnrollmentController", ex.Message, "EnrollmentDal");
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
    }

