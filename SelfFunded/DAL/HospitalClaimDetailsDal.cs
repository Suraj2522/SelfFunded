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
    public class HospitalClaimDetailsDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public HospitalClaimDetailsDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }


        public List<Dictionary<string, object>> getHospitalClaimDetails(HospitalClaimDetails hospdtls)
        {

            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();

            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
               // hospdtls.userId = 111;
              //  hospdtls.bankName= "1";
                if (hospdtls.bankName=="1")
                {
                    if(hospdtls.userId==111)
                    {
                        SqlDataAdapter da = new SqlDataAdapter("Usp_HospitalClaimSearch_SelfFunded", connection);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = 600;
                        da.SelectCommand.Parameters.AddWithValue("@InsuranceId", hospdtls.insuranceId);
                        da.SelectCommand.Parameters.AddWithValue("@ClaimNumber", hospdtls.claimNumber);
                        da.SelectCommand.Parameters.AddWithValue("@InsuredName", hospdtls.insuredName);
                        da.SelectCommand.Parameters.AddWithValue("@OrderByCol", hospdtls.orderByCol);
                        da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(hospdtls.fromDate) ? (object)DBNull.Value : DateTime.Parse(hospdtls.fromDate).ToString("dd-MM-yyyy"));
                        da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(hospdtls.toDate) ? (object)DBNull.Value : DateTime.Parse(hospdtls.toDate).ToString("dd-MM-yyyy"));
                        da.SelectCommand.Parameters.AddWithValue("@CorporateId", hospdtls.corporateId);
                        da.SelectCommand.Parameters.AddWithValue("@ClaimType", hospdtls.claimType);

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
                    else
                    {
                        SqlDataAdapter da = new SqlDataAdapter("Usp_HospitalClaimSearch", connection);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = 600;
                        da.SelectCommand.Parameters.AddWithValue("@InsuranceId", hospdtls.insuranceId);
                        da.SelectCommand.Parameters.AddWithValue("@ClaimNumber", hospdtls.claimNumber);
                        da.SelectCommand.Parameters.AddWithValue("@InsuredName", hospdtls.insuredName);
                        da.SelectCommand.Parameters.AddWithValue("@OrderByCol", hospdtls.orderByCol="");
                        da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(hospdtls.fromDate) ? (object)DBNull.Value : DateTime.Parse(hospdtls.fromDate).ToString("dd-MM-yyyy"));
                        da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(hospdtls.toDate) ? (object)DBNull.Value : DateTime.Parse(hospdtls.toDate).ToString("dd-MM-yyyy"));
                        da.SelectCommand.Parameters.AddWithValue("@CorporateId", hospdtls.corporateId);
                        da.SelectCommand.Parameters.AddWithValue("@ClaimType", hospdtls.claimType);

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


                }
                else
                {
                    SqlDataAdapter da = new SqlDataAdapter("Usp_HospitalClaimSearch_HSBCBank", connection);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 600;
                    da.SelectCommand.Parameters.AddWithValue("@InsuranceId", hospdtls.insuranceId);
                    da.SelectCommand.Parameters.AddWithValue("@ClaimNumber", hospdtls.claimNumber);
                    da.SelectCommand.Parameters.AddWithValue("@InsuredName", hospdtls.insuredName);
                    da.SelectCommand.Parameters.AddWithValue("@OrderByCol", hospdtls.orderByCol="");
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(hospdtls.fromDate) ? (object)DBNull.Value : DateTime.Parse(hospdtls.fromDate).ToString("dd-MM-yyyy"));
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(hospdtls.toDate) ? (object)DBNull.Value : DateTime.Parse(hospdtls.toDate).ToString("dd-MM-yyyy"));
                    da.SelectCommand.Parameters.AddWithValue("@CorporateId", hospdtls.corporateId);
                    da.SelectCommand.Parameters.AddWithValue("@ClaimType", hospdtls.claimType);

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
               

            }
            catch (Exception ex)
            {
                commondal.LogError("GetHospitalClaimDetails", "HospitalClaimDetailsController", ex.Message, "HospitalClaimDetailsDal");
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

        public string updateDebitDetails(HospitalClaimDetails hospdtls)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand("Usp_UpdateTDSDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClaimId", hospdtls.claimType);
                cmd.Parameters.AddWithValue("@UserId", hospdtls.userId = 111);
                cmd.Parameters.AddWithValue("@TDSValueAfterLimit", hospdtls.tdsValueAfterLimit);
                cmd.Parameters.AddWithValue("@TDSValueBeforeLimit", hospdtls.tdsValueBeforeLimit);


                connection.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return "Data updated successfully";
                }
                else
                {
                    return "Error occurred";
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("UpdateDebitDetails", "HospitalClaimDetailsController", ex.Message, "HospitalClaimDetailsDal");
                return "An error occurred while processing the request.";
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }
        public string saveTDSDetails(HospitalClaimDetails hospdtls)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand("Usp_IUTDSDetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClaimId", hospdtls.claimId);
                cmd.Parameters.AddWithValue("@UserId", hospdtls.userId);
                cmd.Parameters.AddWithValue("@cheque_in_the_name_of", hospdtls.chequeInTheNameOf);
                cmd.Parameters.AddWithValue("@benf_bank_acc_no", hospdtls.benfBankAccNo);
                cmd.Parameters.AddWithValue("@IFSCCode", hospdtls.ifscCode);
                cmd.Parameters.AddWithValue("@BankName", hospdtls.bankName);
                cmd.Parameters.AddWithValue("@AccountType", hospdtls.accountType);


                connection.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return "Data updated successfully";
                }
                else
                {
                    return "Error occurred";
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("SaveTDSDetails", "HospitalClaimDetailsController", ex.Message, "HospitalClaimDetailsDal");
                return "An error occurred while processing the request.";
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
