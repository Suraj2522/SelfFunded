using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data.SqlClient;
using System.Security.AccessControl;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using System.Text;
using EvoPdf;

namespace SelfFunded.Controllers
{
    public class SettlementLetterController : Controller
    {
        private readonly SettlementLetterDal _settlementLetterDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private readonly int _maxColumnCount;
        string _evoPdfSettings;
        public SettlementLetterController(IConfiguration configuration, CommonDal common)
        {
            _settlementLetterDal = new SettlementLetterDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");
            _evoPdfSettings = configuration["EvoPdfSettings:EvoInternalFileName"] ?? "";

        }

        [Route("api/SettlementLetter/GetSettlementLetterDetails")]
        [HttpPost]
        public IActionResult GetSettlementLetterDetails()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                SettlementLetter stldtls = new SettlementLetter();


                stldtls.insuranceCompanyId = Convert.ToInt32(httpRequest.Form["insuranceCompanyId"]);
                stldtls.claimNo = httpRequest.Form["claimNo"];
                stldtls.employeeCode = httpRequest.Form["employeeCode"];

                var report = _settlementLetterDal.getSettlementLetterDetails(stldtls);
                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the exception
                commondal.LogError("GetSettlementLetterDetails", "SettlementLetterController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
       


        [Route("api/SettlementLetter/SettlementLetter")]
        [HttpPost]
        public IActionResult GetSettlementLetter([FromQuery] int claimId)
        {
            try
            {
                // Fetch the HTML content to be converted to PDF
                string htmlContent = GetHtmlContentForPdf(claimId);

                if (string.IsNullOrEmpty(htmlContent))
                {
                    return NotFound(new { message = "No data found for the provided claim ID." });
                }

                // Configure the HTML to PDF converter
                HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter
                {
                    LicenseKey = "F5mKmI2ImIqJj5iPloiYi4mWiYqWgYGBgZiI",
                    PdfDocumentOptions = { PdfPageSize = PdfPageSize.A4 },
                    EvoInternalFileName = _evoPdfSettings // Set the full path to the evointernal.dat file


                };

                // Convert HTML to PDF
                byte[] pdfBytes = htmlToPdfConverter.ConvertHtml(htmlContent, "");

                // Return PDF file as a downloadable file
                return File(pdfBytes, "settlementLetter/pdf", $"Report_{claimId}.pdf");
            }
            catch (Exception ex)
            {
                // Log the exception
                commondal.LogError("GetSettlementLetter", "SettlementLetterController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        private string GetHtmlContentForPdf(int claimId)
        {
            // Fetch data based on claimId
            var investigationReport = _settlementLetterDal.SettlementLetter(claimId);

            if (investigationReport == null || investigationReport.Count == 0)
            {
                return null;
            }

            string policyNo = string.Empty;
            string claimNo = string.Empty;
            string insuredName = string.Empty;
            string contactNo = string.Empty;
            string allocationDate = string.Empty;
            string reportDate = string.Empty;
            string age = string.Empty;
            string sex = string.Empty;

            // Iterate over the investigation report
            

        

            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<html><body>");
            strHtml.Append("<div class='container' id='formSettlement'  runat='server' visible='false' style='border:1px solid #000000;'>");
            strHtml.Append("<div class='head' style='margin:10px;'>");
            strHtml.Append("<img src='images/header_Report1.jpg' style='width:95%;'>");
            strHtml.Append("</div>");
            strHtml.Append("");
            strHtml.Append("<div class='address_bar' style='text-align:center; '>");
            strHtml.Append("<span style='font-size:25px; '><b>Settlement Letter</b></span>");
            strHtml.Append("");
            strHtml.Append("<table style='width:100%; border-collapse: collapse; margin-top:10px;'>");
            strHtml.Append("<tr style='background-color:#ddd; '>");
            strHtml.Append("<th colspan='2' style='text-align:center; font-size:16px; border:1px solid #000000; border-collapse: collapse; padding:5px;'><b>CLAIMANT DETAILS</b></th>");
            strHtml.Append("<th colspan='2' style='text-align:center; font-size:16px; border:1px solid #000000; border-collapse: collapse; padding:5px;'><b>PAYMENT DETAILS</b></th>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Patient Name</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Account Holder's Name</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Insured Name</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Bank Name</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Corporate Name</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>IFSC Code</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Employee ID</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>UTR No.</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Policy No.</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Amount Paid</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Plan Name</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;' id='tdPaymentDateLabel' runat='server'>Date of Payment</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;' id='tdPaymentDate' runat='server'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'> TDS Deduction</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("</table>");
            strHtml.Append("<br/>");
            strHtml.Append("<!-------------------------->");
            strHtml.Append("<table style='width:100%; border-collapse: collapse;'>");
            strHtml.Append("<tr style='background-color:#ddd;'>");
            strHtml.Append("<th colspan='2' style='text-align:center; font-size:16px; border:1px solid #000000; border-collapse: collapse; padding:10px;'>CLAIM DETAILS</th>");
            strHtml.Append("");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Claim No.</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Claim Type</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Case Type</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Hospital Name</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'c>Date of Admission</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Date of Dischage</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Ailment</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Amount Claimed</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Deductions</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Co-Pay</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr id='trTariffDeduction' runat='server'>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Tariff Deduction</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Claim Amount Settled</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>Amount Paid to Patient / Hospital</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>123</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</table>");
            strHtml.Append("<br/>");
            strHtml.Append("<!-------------------------->");
            strHtml.Append("");
            strHtml.Append("<HeaderTemplate>");
            strHtml.Append("<table style='width:100%; border-collapse: collapse;'>");
            strHtml.Append("<tr style='background-color:#ddd;'>");
            strHtml.Append("<th colspan='7' style='text-align:center; font-size:16px; border:1px solid #000000; border-collapse: collapse; padding:10px;'>DEDUCTION DETAILS</th>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Sr. No.</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Date</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Invoice No.</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Description</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Amount Claimed</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Deduction</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Reason for Deduction</b></td>");
            strHtml.Append("</tr>");
            strHtml.Append("</HeaderTemplate>");
            strHtml.Append("<ItemTemplate>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;text-align:right;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:px; border-collapse: collapse;text-align:right;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</ItemTemplate>");
            strHtml.Append("<FooterTemplate>");
            strHtml.Append("</table>");
            strHtml.Append("</FooterTemplate>");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("<HeaderTemplate>");
            strHtml.Append("<table style='width:100%; border-collapse: collapse;'>");
            strHtml.Append("<tr style='background-color:#ddd;'>");
            strHtml.Append("<th colspan='7' style='text-align:center; font-size:16px; border:1px solid #000000; border-collapse: collapse; padding:10px;'>DEDUCTION DETAILS</th>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Sr. No.</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Category</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Amount Claimed</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Deduction</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Discount</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Net Amount</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Reason for Deduction</b></td>");
            strHtml.Append("</tr>");
            strHtml.Append("</HeaderTemplate>");
            strHtml.Append("<ItemTemplate>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;text-align:right;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;text-align:right;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;text-align:right;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:px; border-collapse: collapse;text-align:right;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'>");
            strHtml.Append("123");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</ItemTemplate>");
            strHtml.Append("<FooterTemplate>");
            strHtml.Append("</table>");
            strHtml.Append("</FooterTemplate>");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("<br/>");
            strHtml.Append("<!-------------------------->");
            strHtml.Append("<table style='width:100%; border-collapse: collapse;'>");
            strHtml.Append("<tr style='background-color:#ddd;'>");
            strHtml.Append("<th colspan='8' style='text-align:center; font-size:16px; border:1px solid #000000; border-collapse: collapse; padding:10px;'>SETTLEMENT SUMMARY</th>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Gross Amount</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>123</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Deduction Amount</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>123</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>TDS Amount</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>123</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>Net Amount</b></td>");
            strHtml.Append("<td style='border:1px solid #000000; font-size:16px; padding:5px; border-collapse: collapse;'><b>123</b></td>");
            strHtml.Append("</tr>");
            strHtml.Append("</table>");
            strHtml.Append("<!-------------------------->");
            strHtml.Append("<br/>");
            strHtml.Append("<h4 style='text-align:center; font-size:14px;'>********** This is a computer-generated settlement letter and does not require a signature *********</h4>");
            strHtml.Append("<p style='text-align:left; font-weight:bold; padding:5px;'> Regards <br/>");
            strHtml.Append("Paramount Healthcare Management Pvt. Ltd.");
            strHtml.Append("</p>");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("</div>");
            strHtml.Append("</div>");
            strHtml.Append("</body></html>");

            return strHtml.ToString();
        }
    }
}
