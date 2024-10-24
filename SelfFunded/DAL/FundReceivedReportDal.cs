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
    public class FundReceivedReportDal
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        CommonDal commondal;

        public FundReceivedReportDal(IConfiguration configuration, CommonDal common)
        {
            _configuration = configuration;
            _connectionString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
            commondal = common;
        }


        public List<Dictionary<string, object>> getALlFundReceivedReport(FundReceivedReport fundrpt)
        {
            // List<FundReceivedReport> report = new List<FundReceivedReport>();
            //var reports = new List<object>();
            List<Dictionary<string, object>> report = new List<Dictionary<string, object>>();

            SqlConnection connection = null;
            DataTable dt = new DataTable();

            try
            {
                connection = new SqlConnection(_connectionString);
                SqlDataAdapter da = new SqlDataAdapter("GetOutStandingAmtFromOustanding", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 600;
                da.SelectCommand.Parameters.AddWithValue("@InsuranceID", fundrpt.insuranceID);
                da.SelectCommand.Parameters.AddWithValue("@DebitID", fundrpt.debitID);
                da.SelectCommand.Parameters.AddWithValue("@ClaimNO", fundrpt.claimNO);

                connection.Open();
                da.Fill(dt);




                ////    Convert DataTable to List<FundReceivedReport>


                //foreach (DataRow row in dt.Rows)
                //{
                //    var report= new
                //    {
                //        srNo= row["Sr.No"] != DBNull.Value ? Convert.ToInt32( row["Sr.No"]) : (int?)null,
                //        claimNO = row["ClaimNo"] != DBNull.Value ? row["ClaimNo"].ToString() : null,
                //        insuranceCompany = row["InsuranceCompany"] != DBNull.Value ? row["InsuranceCompany"].ToString() : null,
                //        insuredName = row["InsuredName"] != DBNull.Value ? row["InsuredName"].ToString() : null,
                //        providerName = row["ProviderName"] != DBNull.Value ? row["ProviderName"].ToString() : null,
                //        policyNo = row["PolicyNo"] != DBNull.Value ? row["PolicyNo"].ToString() : null,
                //        debitNoteNo = row["DebitNoteNo"] != DBNull.Value ? row["DebitNoteNo"].ToString() : null,
                //        toCurrencyCode = row["ToCurrencyCode"] != DBNull.Value ? row["ToCurrencyCode"].ToString() : null,
                //        invoiceProviderAmount = row["InvoiceProviderAmount"] != DBNull.Value ? Convert.ToDecimal(row["InvoiceProviderAmount"]) : 0,
                //        feesInvoiceAmount = row["FeesInvoiceAmount"] != DBNull.Value ? Convert.ToDecimal(row["FeesInvoiceAmount"]) : 0,
                //        assistanceFees = row["Assistance Fees"] != DBNull.Value ? Convert.ToDecimal(row["Assistance Fees"]) : 0,
                //        totalReceivable = row["Total Receivable"] != DBNull.Value ? Convert.ToDecimal(row["Total Receivable"]) : 0,
                //        stateName = row["StateName"] != DBNull.Value ? row["StateName"].ToString() : null,
                //        outwardDate = row["OutwardDate"] != DBNull.Value ? Convert.ToDateTime(row["OutwardDate"]) : (DateTime?)null,
                //        outwardNo = row["Outward No."] != DBNull.Value ? row["Outward No."].ToString() : null,
                //        aging = row["Aging"] != DBNull.Value ? Convert.ToInt32(row["Aging"]) : 0,
                //        certificateNo = row["CertificateNo"] != DBNull.Value ? row["CertificateNo"].ToString() : null,
                //        invoiceReceivedAmount = row["InvoiceRecievedAmount"] != DBNull.Value ? Convert.ToDecimal(row["InvoiceRecievedAmount"]) : 0,
                //        feesReceivedAmount = row["FeesRecievedAmount"] != DBNull.Value ? Convert.ToDecimal(row["FeesRecievedAmount"]) : 0,
                //        providerOutstandingAmount = row["ProviderOustandingAmount"] != DBNull.Value ? Convert.ToDecimal(row["ProviderOustandingAmount"]) : 0,
                //        feesOutstandingAmount = row["FeesOustandingAmount"] != DBNull.Value ? Convert.ToDecimal(row["FeesOustandingAmount"]) : 0,
                //        assistanceFeesOutstanding = row["Assistance Fees OutStanding"] != DBNull.Value ? Convert.ToDecimal(row["Assistance Fees OutStanding"]) : 0,
                //        totalOutstanding = row["Total Outstanding"] != DBNull.Value ? Convert.ToDecimal(row["Total Outstanding"]) : 0,
                //        remarks = row["Remarks"] != DBNull.Value ? row["Remarks"].ToString() : null,
                //        debitAutoId = row["DebitAutoId"] != DBNull.Value ? Convert.ToInt32(row["DebitAutoId"]) : 0,
                //        insuranceID = row["InsuranceID"] != DBNull.Value ? Convert.ToInt32(row["InsuranceID"]) : 0,
                //        providerExcessAmount = row["ProviderExcessAmount"] != DBNull.Value ? Convert.ToDecimal(row["ProviderExcessAmount"]) : 0,
                //        assistanceName = row["Assistance Name"] != DBNull.Value ? row["Assistance Name"].ToString() : null,
                //        phmFeesExcessAmount = row["PHMFeesExcessAmount"] != DBNull.Value ? Convert.ToDecimal(row["PHMFeesExcessAmount"]) : 0,
                //        visibleView = row["VisibleView"] != DBNull.Value ? Convert.ToInt32(row["VisibleView"]) : 0
                //    };

                //    reports.Add(report);
                //}
                //return reports;
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
                commondal.LogError("GetFundReceivedReport", "FundReceivedReportController", ex.Message, "FundReceivedReportDal");
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
