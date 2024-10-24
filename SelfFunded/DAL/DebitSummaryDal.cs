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
    public class DebitSummaryDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public DebitSummaryDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }


        public List<Dictionary<string, object>> getDebitSummary(DebitSummary dbtsmry)
        {

            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();

            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("usp_GetDebitNoteSelfFundedSummary", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;
                da.SelectCommand.Parameters.AddWithValue("@InsuranceCompanyId", dbtsmry.insuranceCompanyId);
                da.SelectCommand.Parameters.AddWithValue("@DebitNoteNo", dbtsmry.debitNoteNo);
                da.SelectCommand.Parameters.AddWithValue("@InsuredName", dbtsmry.insuredName);
                da.SelectCommand.Parameters.AddWithValue("@ClaimNo", dbtsmry.claimNo);
                da.SelectCommand.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(dbtsmry.fromDate) ? (object)DBNull.Value : DateTime.Parse(dbtsmry.fromDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(dbtsmry.toDate) ? (object)DBNull.Value : DateTime.Parse(dbtsmry.toDate).ToString("dd-MM-yyyy"));


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
                commondal.LogError("GetDebitSummary", "DebitNoteController", ex.Message, "DebitNoteDal");
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
        public List<Dictionary<string, object>> getDebitPdf(string dbtsmry)

        {

            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();

            SqlConnection connection = null;

            DataTable dt = new DataTable();

            try

            {

                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("usp_GetDebitNoteSelfFunded", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;
                da.SelectCommand.Parameters.AddWithValue("@DebitNoteNo", dbtsmry.ToString());
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
                commondal.LogError("GetDebitPdf", "DebitNoteController", ex.Message, "DebitNoteDal");
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

        public DataTable  getDebitExport(string dbtsmry)
        {
            SqlConnection connection = null;
            DataTable dt = new DataTable();
            try

            {

                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("Usp_GetDebitNoteSelfFundedAnnexure", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;
                da.SelectCommand.Parameters.AddWithValue("@DebitNoteNumber", dbtsmry.ToString());
                connection.Open();
                da.Fill(dt);

               
                return dt;
            }

            catch (Exception ex)
            {
                commondal.LogError("getDebitExport", "DebitNoteController", ex.Message, "DebitNoteDal");
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



        //public List<Dictionary<string, object>> getDebitAnnexurePrint(string dbtNo)
        //{
        //    List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();
        //    SqlConnection connection = null;
        //    try
        //    {

        //        using (connection = new SqlConnection(_connectionString))
        //        {
        //            DataTable dt = new DataTable();
        //            SqlDataAdapter da = new SqlDataAdapter("Usp_GetDebitNoteSelfFundedAnnexure", connection);
        //            da.SelectCommand.CommandType = CommandType.StoredProcedure;
        //            da.SelectCommand.CommandTimeout= 600;   
        //            da.SelectCommand.Parameters.AddWithValue("@DebitNoteNumber", dbtNo);
        //            connection.Open();
        //            da.Fill(dt);
        //            connection.Close();
        //            foreach (DataRow row in dt.Rows)

        //            {

        //                Dictionary<string, object> rowDict = new Dictionary<string, object>();
        //                foreach (DataColumn column in dt.Columns)
        //                {
        //                    rowDict[column.ColumnName] = row[column];

        //                }
        //                report.Add(rowDict);
        //            }
        //            return report;

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        commondal.LogError("GetDebitAnnexurePrint", "DebitSummaryController", ex.Message, "DebitSummaryDal");
        //        return report;
        //    }
        //    finally
        //    {
        //        if (connection != null && connection.State == ConnectionState.Open)
        //        {
        //            connection.Close();
        //        }
        //    }
        //}

        public List<Dictionary<string, object>> getDebitAnnexurePrint(string debitNoteNo)
        {
            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Usp_GetDebitNoteSelfFundedAnnexure", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DebitNoteNumber", debitNoteNo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Process the first result set
                        DataTable dt1 = new DataTable();
                        dt1.Load(reader);
                        foreach (DataRow row in dt1.Rows)
                        {
                            Dictionary<string, object> rowDict = new Dictionary<string, object>();
                            foreach (DataColumn column in dt1.Columns)
                            {
                                rowDict[column.ColumnName] = row[column];
                            }
                            report.Add(rowDict);
                        }

                       
                            DataTable dt2 = new DataTable();
                            dt2.Load(reader);
                            foreach (DataRow row in dt2.Rows)
                            {
                                Dictionary<string, object> rowDict = new Dictionary<string, object>();
                                foreach (DataColumn column in dt2.Columns)
                                {
                                    rowDict[column.ColumnName] = row[column];
                                }
                                report.Add(rowDict);
                            }
                        }
                    
                    connection.Close();
              }
            }
            catch (Exception ex)
            {
                commondal.LogError("GetDebitAnnexurePrint", "DebitSummaryController", ex.Message, "DebitSummaryDal");
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return report;
        }

        public List<DebitSummary> bindDebitEditDetails(int id)
        {
            List<DebitSummary> dtls = new List<DebitSummary>();
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlDataAdapter da = new SqlDataAdapter("GetDebitNoteSelfFundedByDeditId", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@DebitId", id);

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    dtls.Add(new DebitSummary
                    {
                        debitTransactionId = Convert.ToInt32(dr["DebitTransactionId"]),
                        debitId = Convert.ToInt32(dr["DebitId"]),
                        debitNoteNo = dr["DebitNoteNo"].ToString(),
                        claimId = Convert.ToInt32(dr["ClaimId"]),
                        claimNo = dr["ClaimNo"].ToString(),
                        amount = Convert.ToDecimal(dr["Amount"]),
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("BindDebitEditDetails", "DebitSummaryController", ex.Message, "DebitSummaryDal");
                Console.WriteLine($"Error in getSubmenu method: {ex.Message}");
                throw; // Re-throw the exception to propagate it to the caller
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return dtls;
        }

        public string updateDebitDetails( DebitSummary dbtdtls)
        {
            SqlConnection connection = null;
            try
            {


                connection = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand("Usp_UpdateDebitNoteSelfFunded", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddWithValue("@DebitTransactionId", dbtdtls.debitTransactionId);
                cmd.Parameters.AddWithValue("@UserId", dbtdtls.userId=111);
                cmd.Parameters.AddWithValue("@Amount", dbtdtls.amount);
                cmd.Parameters.AddWithValue("@ClaimNo", dbtdtls.claimNo);

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
                commondal.LogError("UpdateDebitetails", "DebitSummaryController", ex.Message, "DebitSummaryDal");
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
        public string deleteDebitDetails( DebitSummary dbtdtls)
        {
            SqlConnection connection = null;
            try
            {


                connection = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand("Usp_DeleteDebitNoteSelfFunded", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddWithValue("@DebitTransactionId", dbtdtls.debitTransactionId);
                cmd.Parameters.AddWithValue("@UserId", dbtdtls.userId=111);
                cmd.Parameters.AddWithValue("@ClaimNo", dbtdtls.claimNo);


                connection.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    return "Data deleted successfully";
                }
                else
                {
                    return "Error occurred";
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("DeleteDebitDetails", "DebitSummaryController", ex.Message, "DebitSummaryDal");
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

