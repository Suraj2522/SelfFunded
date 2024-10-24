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
    public class TDSReportDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public TDSReportDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }

        public List<Dictionary<string, object>> getTDSReport(TDSReport tdsrpt)
        {
            //  List<TDSReport> report = new List<TDSReport>();
            // List<object> report = new List<object>();
            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();
            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("GetTdsReport", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@InsuranceId", tdsrpt.insuranceId);
                da.SelectCommand.Parameters.AddWithValue("@ProviderNo", tdsrpt.providerNo);
                da.SelectCommand.Parameters.AddWithValue("@ClaimNO", tdsrpt.claimNo);
                da.SelectCommand.Parameters.AddWithValue("@FromDate", DateTime.Parse(tdsrpt.fromDate).ToString("dd-MM-yyyy"));
                da.SelectCommand.Parameters.AddWithValue("@ToDate", DateTime.Parse(tdsrpt.toDate).ToString("dd-MM-yyyy"));

                da.SelectCommand.Parameters.AddWithValue("@OrderByCol", tdsrpt.orderByCol="");

                connection.Open();
                da.Fill(dt);

                // Convert DataTable to List<FundReceivedReport>
                //foreach (DataRow row in dt.Rows)
                //{
                //    var obj = new
                //    {

                //        ref_no = row["Ref_no"] != DBNull.Value ? row["Ref_no"].ToString() : null,
                //        providerNo = row["ProviderNo"] != DBNull.Value ? row["ProviderNo"].ToString() : null,
                //        pan_No = row["Pan_No"] != DBNull.Value ? row["Pan_No"].ToString() : null,
                //        name_of_hospital = row["Name_of_hospital"] != DBNull.Value ? row["Name_of_hospital"].ToString() : null,
                //        net_Amount_Before_Limit = row["Net_Amount_Before_Limit"] != DBNull.Value ? Convert.ToDecimal(row["Net_Amount_Before_Limit"]) : (decimal?)null,
                //        tax_Rate_Before_Limit = row["Tax_Rate_Before_Limit"] != DBNull.Value ? Convert.ToDecimal(row["Tax_Rate_Before_Limit"]) : (decimal?)null,
                //        tdS_Value_Before_Limit = row["TDS_Value_Before_Limit"] != DBNull.Value ? Convert.ToDecimal(row["TDS_Value_Before_Limit"]) : (decimal?)null,
                //        net_Amount_Before_Limit_With_TDS = row["Net_Amount_Before_Limit_With_TDS"] != DBNull.Value ? Convert.ToDecimal(row["Net_Amount_Before_Limit_With_TDS"]) : (decimal?)null,
                //        net_Amount_After_Limit = row["Net_Amount_After_Limit"] != DBNull.Value ? Convert.ToDecimal(row["Net_Amount_After_Limit"]) : (decimal?)null,
                //        tax_Rate_After_Limit = row["Tax_Rate_After_Limit"] != DBNull.Value ? Convert.ToDecimal(row["Tax_Rate_After_Limit"]) : (decimal?)null,
                //        tdS_Value_After_Limit = row["TDS_Value_After_Limit"] != DBNull.Value ? Convert.ToDecimal(row["TDS_Value_After_Limit"]) : (decimal?)null,
                //        net_Amount_After_Limit_With_TDS = row["Net_Amount_After_Limit_With_TDS"] != DBNull.Value ? Convert.ToDecimal(row["Net_Amount_After_Limit_With_TDS"]) : (decimal?)null,
                //        gross_Amt = row["Gross_Amt"] != DBNull.Value ? Convert.ToDecimal(row["Gross_Amt"]) : (decimal?)null,
                //        tds_Amt = row["Tds_Amt"] != DBNull.Value ? Convert.ToDecimal(row["Tds_Amt"]) : (decimal?)null,
                //        payableAmount = row["PayableAmount"] != DBNull.Value ? Convert.ToDecimal(row["PayableAmount"]) : (decimal?)null,
                //        booking_Date = row["Booking_Date"] != DBNull.Value ? Convert.ToDateTime(row["Booking_Date"]) : (DateTime?)null

                //    };

                //    report.Add(obj);
                //}

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
                commondal.LogError("GetTDSReport", "TDSReportController", ex.Message, "TDSReportDal");
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
    }
}
