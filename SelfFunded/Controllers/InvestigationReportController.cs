using Microsoft.AspNetCore.Mvc;
using SelfFunded.DAL;
using SelfFunded.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EvoPdf;
using System.Data;

namespace SelfFunded.Controllers
{
    public class InvestigationReportController : Controller
    {
        private readonly InvestigationReportDal _investigationReportDal;
        private readonly CommonDal _commondal;
        private string _evoPdfSettings ;


        public InvestigationReportController(IConfiguration configuration, CommonDal common)
        {
            _commondal = common;
            var folderName = "InvestigationReport";
            var licenseKey = "F5mKmI2ImIqJj5iPloiYi4mWiYqWgYGBgZiI";
            _investigationReportDal = new InvestigationReportDal(configuration, common, folderName, licenseKey);
            _evoPdfSettings = configuration["EvoPdfSettings:EvoInternalFileName"] ?? "";
        }

        [Route("api/InvestigationReport/GetInvestigationReport")]
        [HttpPost]
        public IActionResult GetInvestigationReport()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                InvestigationReport rpt = new InvestigationReport();
                rpt.insuranceId = Convert.ToInt32(httpRequest.Form["insurance"]);
                rpt.insuredName = httpRequest.Form["insuredName"].ToString();
                rpt.claimId = httpRequest.Form["claimNo"].ToString();
                
                rpt.fromDate = httpRequest.Form["fromDate"].ToString();
                rpt.toDate = httpRequest.Form["toDate"].ToString();
                var report = _investigationReportDal.GetInvestigationReport(rpt);

                if (report == null || report.Count == 0)
                {
                    return NotFound(new{message = "No data found."});
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                _commondal.LogError("GetInvestigationReport", "InvestigationReportController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/InvestigationReport/DownloadPdf")]
        [HttpPost]
        public IActionResult DownloadPdf([FromQuery] int claimId)
        {
            try
            {
               int claimIds= claimId;
                // Fetch the HTML content to be converted to PDF
                string htmlContent = GetHtmlContentForPdf(claimIds);

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
                return File(pdfBytes, "application/pdf", $"Report_{claimId}.pdf");
            }
            catch (Exception ex)
            {
                _commondal.LogError("DownloadPdf", "InvestigationReportController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        private string GetHtmlContentForPdf(int claimId)
        {
            // Fetch data based on claimId
            var investigationReport = _investigationReportDal.getInvestigationDetails(claimId);

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
            foreach (var row in investigationReport)
            {
                foreach (var item in row)
                {
                    // Extract and store specific values based on known keys
                    switch (item.Key)
                    {
                       
                        case "PolicyNo":
                            policyNo = item.Value?.ToString();
                            break;
                        case "ClaimNo":
                            claimNo = item.Value?.ToString();
                            break;
                        case "Age":
                            age = item.Value?.ToString();
                            break;
                        case "InsuredName":
                            insuredName = item.Value?.ToString();
                            break;
                        case "ContactNo":
                            contactNo = item.Value?.ToString();
                            break;
                        case "AllocationDate":
                            allocationDate = item.Value?.ToString();
                            break;
                        case "ReportDate":
                            reportDate = item.Value?.ToString();
                            break;
                        // Add more cases as needed for other parameters
                        case "Gender":
                            sex = item.Value?.ToString();
                            break;
                            // Add more cases as needed for other parameters
                    }
                }
            }



            StringBuilder strHtml = new StringBuilder();
           
            strHtml.Append("<html><body>");
            strHtml.Append("<div class='container' id='formInvestigationReport'  runat='server' visible='false'>");
            strHtml.Append("<table style='width:100%;'>");
            strHtml.Append("<tr>");
            strHtml.Append("<th width='15%'; style='background-color:#ffffff;'>");
            strHtml.Append("<img src='images/header_report.jpg'  style='width:99%; padding:10px;' />");
            strHtml.Append("</th>");
            strHtml.Append("</tr>	");
            strHtml.Append("</table>");
            strHtml.Append("<!-------------------------->");
            strHtml.Append("");
            strHtml.Append("<table style='width:100%;'>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1' style='text-align:center; font-size:20px;'>");
            strHtml.Append("<b style='color:blue; font-style: italic;'>INVESTIGATION REPORT</b><br/>");
            strHtml.Append("");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</table>");
            strHtml.Append("");
            strHtml.Append("<br />");
            strHtml.Append("<!-------------------------->");
            strHtml.Append("<table style='width:100%;'>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Case Allocation Date:</span>");
              strHtml.Append("</td>");
             strHtml.Append("<td class='hdr1'>");
            strHtml.Append($"<td>{allocationDate}</td>");
            strHtml.Append("</td>");
          
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Report Date:</span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append($"<td>{reportDate}</td>");
            strHtml.Append("</td>");
            strHtml.Append("");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Name of the driver partner :</span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblInsuredName' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Age :</span> <b><asp:Label ID='lblAge' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append($"<td>{age}</td>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Sex :</span> <b><asp:Label ID='lblGender' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Vehicle Registration No. :</span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblDrivingLicense' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Contact No. :</span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append($"<td>{contactNo}</td>");
            strHtml.Append("<b><asp:Label ID='lblContactNo' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Uber Claim No. :</span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblPolicyNo' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>PHM Claim No. :</span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblClaimNo' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("</table>");
            strHtml.Append("<!---------------------------->");
            strHtml.Append("<br />");
            strHtml.Append("");
            strHtml.Append("<table style='width:100%; margin-top:5px; border:2px solid #000000;'> ");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<th class='hdr1' style='text-align:left;'>");
            strHtml.Append("<b style='font-size:18px; color:blue;font-family:Arial;'>Hospital Info</b>");
            strHtml.Append("</th>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td  class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblProviderName' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</table>");
            strHtml.Append("<br />");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("<!--------------------------->");
            strHtml.Append("");
            strHtml.Append("<!-------------------------->");
            strHtml.Append("<table style='width:100%;'>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Network Status :</span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblNetworkType' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Date of Admission:</span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblDateOfAdmission' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Date of Discharge: </span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblDateOfDischarge' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Date of Death:</span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblDateOfDeath' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Details of Treatment :</span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblTreatment' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Diagnosis:</span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblDiagnosis' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("</table>");
            strHtml.Append("<br />");
            strHtml.Append("<!---------------------------->");
            strHtml.Append("<!---------------------------->");
            strHtml.Append("<table style='width:100%; margin-top:5px; border:2px solid #000000;'> ");
            strHtml.Append("<tr>");
            strHtml.Append("<th class='hdr1' style='text-align:left;'>");
            strHtml.Append("<b style='font-size:18px; color:blue;font-family:Arial;'>Incidence History</b>");
            strHtml.Append("</th>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td  class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblRemarks' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</table>");
            strHtml.Append("<br />");
            strHtml.Append("<!--------------------------->");
            strHtml.Append("<!---------------------------->");
            strHtml.Append("<table style='width:100%; margin-top:5px; border:2px solid #000000;'> ");
            strHtml.Append("<tr>");
            strHtml.Append("<th class='hdr1' style='text-align:left;'>");
            strHtml.Append("<b style='font-size:18px; color:blue;font-family:Arial;'>Cause of Death</b>");
            strHtml.Append("</th>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td  class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblCauseOfDeath' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</table>");
            strHtml.Append("<br />");
            strHtml.Append("<!--------------------------->");
            strHtml.Append("<!---------------------------->");
            strHtml.Append("<table style='width:100%; margin-top:5px; border:2px solid #000000;'> ");
            strHtml.Append("<tr>");
            strHtml.Append("<th class='hdr1' style='text-align:left;'>");
            strHtml.Append("<b style='font-size:18px; color:blue;font-family:Arial;'>PHM Doctor Comments</b>");
            strHtml.Append("</th>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td  class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblAuditorRemarks' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</table>");
            strHtml.Append("<br />");
            strHtml.Append("<!--------------------------->");
            strHtml.Append("");
            strHtml.Append("<!---------------------------->");
            strHtml.Append("<table style='width:100%; margin-top:5px; border:2px solid #000000;'> ");
            strHtml.Append("<tr>");
            strHtml.Append("<th class='hdr1' style='text-align:left;'>");
            strHtml.Append("<b style='font-size:18px; color:blue;font-family:Arial;'>Documents Received</b>");
            strHtml.Append("</th>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td  class='hdr1'>");
            strHtml.Append("<b><asp:Label ID='lblDocuments' runat='server' SkinID='lblLabelNormal'></asp:Label></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</table>");
            strHtml.Append("<br />");
            strHtml.Append("<!--------------------------->");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("<!---------------------------->");
            strHtml.Append("");
            strHtml.Append("<table width='40%'; style='margin-top:5px; border:2px solid #000000; float:right;'>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr4' style='text-align:center;'>");
            strHtml.Append("<b style='font-size:16px;font-family:Arial;'>For Paramount Healthcare Management Private Ltd.</b><br/>");
            strHtml.Append("<img src='images/stamp.png' width='30%' align='center'/><br/>");
            strHtml.Append("<b style='font-size:16px;font-family:Arial;'>Authorized Signatory</b><br/>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("</table>");
            strHtml.Append("<!---------------------------->");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("</div>");
            //strHtml.Append("</body>");


            // Add content from investigationReport
            // Example: Adding simple table from the report data
          //  strHtml.Append("<table class='table'><thead><tr><th>Column1</th><th>Column2</th></tr></thead><tbody>");
            //foreach (var row in investigationReport)
            //{
            //    strHtml.Append("<tr>");
            //    foreach (var item in row)
            //    {
            //        strHtml.Append($"<td>{item.Value}</td>");
            //    }
            //    strHtml.Append("</tr>");
            //}
            strHtml.Append("</tbody></table>");

            strHtml.Append("</body></html>");

            return strHtml.ToString();
        }
    }
}
