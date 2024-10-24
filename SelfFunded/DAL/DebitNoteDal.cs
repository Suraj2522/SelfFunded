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
    public class DebitNoteDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public DebitNoteDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }


        public List<Dictionary<string, object>> getDebitNoteSearch(DebitNote dbtnote)
        {
            
            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();

            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("USP_GetDebitNoteSearchSelfFunded", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;
                da.SelectCommand.Parameters.AddWithValue("@InsuranceID", dbtnote.insuranceID);
                da.SelectCommand.Parameters.AddWithValue("@PlanID", dbtnote.planId=167);
                da.SelectCommand.Parameters.AddWithValue("@ClaimTypeID", dbtnote.claimTypeId);
                da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(dbtnote.fromDate) ? (object)DBNull.Value : DateTime.Parse(dbtnote.fromDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(dbtnote.toDate) ? (object)DBNull.Value : DateTime.Parse(dbtnote.toDate).ToString("dd-MM-yyyy"));

                da.SelectCommand.Parameters.AddWithValue("@ClaimNumber", dbtnote.claimNumber);

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
                commondal.LogError("GetDebitNoteSearch", "DebitNoteController", ex.Message, "DebitNoteDal");
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
        public String generateDebitNote(DebitNote dbtnote)
        {
            SqlConnection connection = null;

            try
            {
                int id = 0;
                using (connection = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("USP_IUDebitNoteSelfFunded", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", dbtnote.userId=111);
                    cmd.Parameters.AddWithValue("@ClaimIds", dbtnote.claimIds.ToString());
                    cmd.Parameters.AddWithValue("@InsuranceId", dbtnote.insuranceID);
                    cmd.Parameters.AddWithValue("@ClaimTypeId", dbtnote.claimTypeId);
                    cmd.Parameters.AddWithValue("@PlanId", dbtnote.planId=97);
                    cmd.Parameters.AddWithValue("@NumberOfClaims", dbtnote.numberOfClaims);
                    cmd.Parameters.AddWithValue("@Amount", dbtnote.netAmount);
                    cmd.Parameters.AddWithValue("@AccountId", dbtnote.accountId=1);

                    connection.Open();
                    id = cmd.ExecuteNonQuery();
                    connection.Close();
                }
                if (id > 0)
                {
                    return "Genetaed successfully";
                }
                else
                {
                    return "error occured";
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("GetDebitNoteSearch", "DebitNoteController", ex.Message, "DebitNoteDal");
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
