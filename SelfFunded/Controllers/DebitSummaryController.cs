using EvoPdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Identity.Client;
using OfficeOpenXml;
using SelfFunded.DAL;
using SelfFunded.Models;
using System.Data;
using System.Diagnostics.Tracing;
using System.Security.Claims;
using System.Text;
using LicenseContext = OfficeOpenXml.LicenseContext;
using OfficeOpenXml;
using System.Net.Http.Headers;
namespace SelfFunded.Controllers
{
    public class DebitSummaryController : Controller
    {
        private readonly DebitSummaryDal _debitSummaryDal;
        string ConfigureFilePath;
        CommonDal commondal;
        private string _evoPdfSettings;
        private readonly int _maxColumnCount;
        public DebitSummaryController(IConfiguration configuration, CommonDal common)
        {
            _debitSummaryDal = new DebitSummaryDal(configuration, common);
            ConfigureFilePath = configuration["DocumentUpload"] ?? "";
            _maxColumnCount = configuration.GetValue<int>("ColumnSettings:MaxColumnCount");
            _evoPdfSettings = configuration["EvoPdfSettings:EvoInternalFileName"] ?? "";

        }

        [Route("api/DebitSummary/GetDebitSummary")]
        [HttpPost]
        public IActionResult GetDebitSummary()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                DebitSummary dbtsmry = new DebitSummary();
                dbtsmry.insuranceCompanyId = Convert.ToInt32(httpRequest.Form["insuranceCompanyIdSummary"]);
                dbtsmry.debitNoteNo = httpRequest.Form["debitNoteNumberSummary"];
                dbtsmry.insuredName = httpRequest.Form["insuredNameSummary"];
                dbtsmry.claimNo = httpRequest.Form["claimNumberSummary"];
                dbtsmry.fromDate = httpRequest.Form["fromDateSummary"];
                dbtsmry.toDate = httpRequest.Form["toDateSummary"];


                var report = _debitSummaryDal.getDebitSummary(dbtsmry);

                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the exception
                commondal.LogError("GetDebitSummary", "DebitSummaryController", ex.Message, "DebitSummaryDal");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/DebitSummary/GetDebitPdf")]
        [HttpPost]
        public IActionResult GetDebitPdf([FromQuery] string DebitNoteNo)
        {
            try
            {
                ;
                // Fetch the HTML content to be converted to PDF
                string htmlContent = GetHtmlContentForPdf(DebitNoteNo);

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
                return File(pdfBytes, "application/pdf", $"DebitNote_{DebitNoteNo}.pdf");
            }
            catch (Exception ex)
            {
                // Log the exception
                commondal.LogError("GetDebitPdf", "DebitSummaryController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }


        [Route("api/DebitSummary/GetDebitAnnexurePrint")]
        [HttpPost]
        public IActionResult GetDebitAnnexurePrint([FromQuery] string DebitNoteNo)
        {
            string msg = "";
            try
            {
                ;
                // Fetch the HTML content to be converted to PDF
                string htmlContent = GetHtmlContentForPdfAnnexurePrint(DebitNoteNo);

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
                return File(pdfBytes, "application/pdf", $"DebitAnnexurePrint_.pdf");
            }
            catch (Exception ex)
            {
                commondal.LogError("GetDebitAnnexurePrint", "DebitSummaryController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }

        [Route("api/DebitSummary/GetDebitAnnexureExport")]
        [HttpPost]
        public IActionResult GetDebitAnnexureExport([FromQuery] string DebitNoteNo)
        {
            string msg = "";
            try
            {
                DataTable dt = _debitSummaryDal.getDebitExport(DebitNoteNo);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return NotFound(new { message = "No data found " });
                }

                // Convert DataTable to Excel file (as a byte array)
                byte[] excelData;


                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Generate Excel file
                using (var package = new OfficeOpenXml.ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Annexure");

                    // Load data table into the worksheet
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                    excelData = package.GetAsByteArray();
                }

                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Annexure.xlsx"
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            catch (Exception ex)
            {
                commondal.LogError("GetDebitAnnexurePrint", "DebitSummaryController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
        [Route("api/DebitSummary/BindDebitEditDetails")]
        [HttpGet]
        public IActionResult BindDebitEditDetails([FromQuery] int id)
        {
            try
            {
                List<DebitSummary> list = _debitSummaryDal.bindDebitEditDetails(id);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred while edit debit details: " + ex.Message);
            }
        }

        [Route("api/DebitSummary/UpdateDebitDetails")]
        [HttpPost]
        public IActionResult UpdateDebitDetails()
        {
            string msg = "";
            var httpRequest = HttpContext.Request;
            DebitSummary dbtsmry = new DebitSummary();
            dbtsmry.debitTransactionId = Convert.ToInt32(httpRequest.Form["debitTransactionId"]);
            dbtsmry.userId = Convert.ToInt32(httpRequest.Form["userId"]);
            dbtsmry.amount = Convert.ToInt32(httpRequest.Form["amount"]);
            dbtsmry.claimNo = httpRequest.Form["claimNo"];
            
            

            try
            {
                
                msg = _debitSummaryDal.updateDebitDetails(dbtsmry);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("UpdateDebitDetails", "DebitNoteController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

        }

        [Route("api/DebitSummary/DeleteDebitDetails")]
        [HttpPost]
        public IActionResult DeleteDebitDetails()
        {
            string msg = "";
            var httpRequest = HttpContext.Request;
            DebitSummary dbtsmry = new DebitSummary();
            dbtsmry.debitTransactionId = Convert.ToInt32(httpRequest.Form["debitTransactionId"]);
            dbtsmry.userId = Convert.ToInt32(httpRequest.Form["userId"]);
          //  dbtsmry.amount = Convert.ToInt32(httpRequest.Form["amount"]);
            dbtsmry.claimNo = httpRequest.Form["claimNo"];

            try
            {
              

                msg = _debitSummaryDal.deleteDebitDetails(dbtsmry);
                return Ok(new { message = msg });
            }
            catch (Exception ex)
            {
                commondal.LogError("DeleteDebitDetails", "DebitNoteController", ex.Message, "");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }

        }
        private string GetHtmlContentForPdf(string debitnote)
        {
            // Fetch data based on claimId
            var investigationReport = _debitSummaryDal.getDebitPdf(debitnote);

            if (investigationReport == null || investigationReport.Count == 0)
            {
                return null;
            }

            string insurancecompany = string.Empty;
            string panno = string.Empty;
            string gstno = string.Empty;
            string debitnoteno = string.Empty;
            string invoiceDate = string.Empty;
            string address1 = string.Empty;
            string numberOfClaims = string.Empty;
            string amount = string.Empty;
            string monthYear = string.Empty;
            string planName = string.Empty;
            string bankName = string.Empty;
            string accountNo = string.Empty;
            string accountname = string.Empty;
            string ifscCode = string.Empty;
            string words = string.Empty;

            // Iterate over the investigation report
            foreach (var row in investigationReport)
            {
                foreach (var item in row)
                {
                    // Extract and store specific values based on known keys
                    switch (item.Key)
                    {
                        case "InsuranceCompany":
                            insurancecompany = item.Value?.ToString();
                            break;
                        case "PanNo":
                            panno = item.Value?.ToString();
                            break;
                        case "GstNo":
                            gstno = item.Value?.ToString();
                            break;
                        case "DebitNoteNo":
                            debitnoteno = item.Value?.ToString();
                            break;
                        case "InvoiceDate":
                            invoiceDate = item.Value?.ToString();
                            break;
                        case "Addrss1":
                            address1 = item.Value?.ToString();
                            break;
                        case "NumberOfClaims":
                            numberOfClaims = item.Value?.ToString();
                            break;
                        case "Amount":
                            amount = item.Value?.ToString();
                            //int amt = (Convert.ToInt32(amount));
                            //words=commondal.ConvertToWords(amt);
                            break;
                        case "MonthYear":
                            monthYear = item.Value?.ToString();
                            break;
                        case "PlanName":
                            planName = item.Value?.ToString();
                            break;
                        case "BankName":
                            bankName = item.Value?.ToString();
                            break;
                        case "AccountNo":
                            accountNo = item.Value?.ToString();
                            break;
                        case "AccountName":
                            accountname = item.Value?.ToString();
                            break;
                        case "IFSCCode":
                            ifscCode = item.Value?.ToString();
                            break;
                            // Add more cases as needed for other parameters
                    }
                }
            }



            var strHtml = new StringBuilder();
            strHtml.Append("<html><body>");
            strHtml.Append("<div class='container' id='formDebit'  runat='server' visible='false'>");
            strHtml.Append("<table style='width:100%;'>");
            strHtml.Append("<tr>");
            strHtml.Append("<th width='15%;' style='background-color:#ffffff;'>");
            strHtml.Append("<img src='D:\\netProjects\\SelfFunded\\SelfFunded\\Images\\header_SelfFunded.jpg' style='width:99%; padding:10px;'>");
            strHtml.Append("</th>");
            strHtml.Append("</tr>	");
            strHtml.Append("</table>");
            strHtml.Append("<!-------------------------->");
            strHtml.Append("<table style='width:100%;'>");
            strHtml.Append("<tr>");
            strHtml.Append("<td colspan='2' class='hdr1'  style='text-align:center; font-size:22px;padding:10px;'>");
            strHtml.Append("<b style='color:blue; font-style: italic;font-family:Arial'>DEBIT NOTE</b><br/>");
            strHtml.Append("");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1' style='padding:10px;'>");
            strHtml.Append($"<span style='font-family:Arial;font-size:16px;'>Name: </span><b>{insurancecompany}</b><br />");
            strHtml.Append($"<span style='font-family:Arial;font-size:16px;'></span><b><span style='font-family:Arial;font-size:16px;'>PAN NO :- </span></b><b>{panno}</b><b><span style='font-family:Arial;font-size:16px;'> : GST NO :- </span></b><b>{gstno}</b><br />");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='text-align:left; border:2px solid #000000; width:35%;padding:10px;'>");
            strHtml.Append($"<b><span style='font-family:Arial;font-size:16px;'>Dr. Note No.:</span></b> <b>{debitnoteno}</b><br/><br />");
            strHtml.Append($"<b><span style='font-family:Arial;font-size:16px;'>Date:</span></b> <b>{invoiceDate}</b><br/>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1' style='padding:10px;'>");
            strHtml.Append($"<span style='font-family:Arial;font-size:16px;'>Address:</span> {address1} <br/>");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("</td>");
            strHtml.Append("");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("</table>");
            strHtml.Append("<!---------------------------->");
            strHtml.Append("<table style='width:100%; margin-top:5px; border:2px solid #000000;border-collapse: collapse;'> ");
            strHtml.Append("<tr >");
            strHtml.Append("<td class='hdr1' style='border-right:2px solid #000000;width:8%;padding:10px;text-align:center'>");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>Sr. No. </span></b><br />");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-right:2px solid #000000;width:69%;padding:10px;text-align:center'>");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>Description of Services </span></b><br />");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-right:2px solid #000000;width:13%;padding:10px;text-align:center'>");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>No. of Claims </span></b><br />");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='width:10%;padding:10px;text-align:center'>");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>Amount</span> </b><br />");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1' style='border-top:2px solid #000000;width:8%;padding:10px;text-align:center'   valign='top'>");
            strHtml.Append("1 ");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000;border-top:2px solid #000000; width:69%;padding:10px;'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'>Being amount receivable towards the hospitalization expenses of your members as per the details attached.</span> <br /><br />");
            strHtml.Append($"<span style='font-family:Arial;font-size:16px;'>For the month of</span> {monthYear}<br/><br/>");
            strHtml.Append("");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000;border-top:2px solid #000000;  width:13%;padding:10px;text-align:center' valign='top'>");
            strHtml.Append($"{numberOfClaims}");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000;border-top:2px solid #000000;  width:10%;padding:10px;text-align:right' valign='top'>");
            strHtml.Append($"{amount}");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1' style='width:8%;padding:10px;' valign='top'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'></span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000; width:69%;padding:10px;'>");
            strHtml.Append($"<b><span style='font-family:Arial;font-size:16px;'>Entity : </span></b><b>{planName}</b><br/><br/>");
            strHtml.Append("");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000; width:13%;padding:10px;' valign='top'>");
            strHtml.Append("");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000;  width:10%;padding:10px;'>");
            strHtml.Append("");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1' style='width:8%;padding:10px;'>");
            strHtml.Append("<b></b>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000; width:69%;padding:10px;'>");
            strHtml.Append("<b></b><br/><br/>");
            strHtml.Append("");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000; width:13%;padding:10px;'>");
            strHtml.Append("<b> </b>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000; width:10%;padding:10px;'>");
            strHtml.Append("<b> </b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1' style='width:8%;padding:10px;'  valign='top'>");
            strHtml.Append("<span style='font-family:Arial;font-size:16px;'></span>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000; width:69%;padding:10px;'>");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>PAN NO :- AABCP3183B</span></b><br /><br />");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>GST NO :- 27AABCP3183B1ZQ</span></b><br /><br />");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>SAC CODE :- 999799</span></b><br /><br />");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000; width:13%;padding:10px;' valign='top'>");
            strHtml.Append("");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border-left:2px solid #000000; width:10%;padding:10px;' valign='top'>");
            strHtml.Append("");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1' style='border:2px solid #000000; width:8%;padding:10px;'>");
            strHtml.Append("<b> </b>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border:2px solid #000000; width:69%;padding:10px;'>");
            strHtml.Append($"<b>Rs.one crore</b><br/>");
            strHtml.Append("");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border:2px solid #000000; width:13%;padding:10px;' valign='top'>");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>Total</span></b><br/>");
            strHtml.Append("</td>");
            strHtml.Append("<td class='hdr1' style='border:2px solid #000000; width:10%;padding:10px;text-align:right'>");
            strHtml.Append($"{amount}");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("</table>");
            strHtml.Append("");
            strHtml.Append("<!---------------------------->");
            strHtml.Append("<table width='45%;' style='margin-top:5px; border:2px solid #000000; float:left;'>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr4' style='padding:10px;'>");
            strHtml.Append("Payment Mode <br/>");
            strHtml.Append("Cheque / Demand Draft / NEFT in the name of<br/>");
            strHtml.Append($"{accountname}<br/>"); // Replace '123' with your dynamic variable if needed
            strHtml.Append($"Bank Name: {bankName}<br/>"); // Replace '123' with your dynamic variable if needed
            strHtml.Append($"Bank A/C No.: {accountNo}<br/>"); // Replace '123' with your dynamic variable if needed
            strHtml.Append($"IFSC Code No.: {ifscCode}<br/><br/><br/>"); // Replace '123' with your dynamic variable if needed
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("</table>");
            strHtml.Append("<table width='45%;' style='margin-top:5px; border:2px solid #000000; float:right;'>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr4' style='text-align:center;padding:10px;'>");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>For Paramount Healthcare Management Private Ltd.</span></b><br/>");
            strHtml.Append("<img src='D:\\netProjects\\SelfFunded\\SelfFunded\\Images\\stamp.png' width='30%' align='center'><br/>");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>Authorized Signatory</span></b><br/>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("");
            strHtml.Append("</table>");
            strHtml.Append("<!---------------------------->");
            strHtml.Append("<table style='width:100%;'> ");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1' style='text-align:center; font-style: italic; text-decoration:underline;padding:10px;'>");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>PARAMOUNT HEALTH : Your Link to Good Health </span></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("<tr>");
            strHtml.Append("<td class='hdr1' style='text-align:center; font-style: italic; text-decoration:underline;padding:10px;'>");
            strHtml.Append("<b><span style='font-family:Arial;font-size:16px;'>This is computer generated debit note. </span></b>");
            strHtml.Append("</td>");
            strHtml.Append("</tr>");
            strHtml.Append("</table>");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("");
            strHtml.Append("</div>");
            strHtml.Append("</body></html>");
            return strHtml.ToString();
        }

        private string GetHtmlContentForPdfAnnexurePrint(string debitNo)
        {
            var AnnexurePrint = _debitSummaryDal.getDebitAnnexurePrint(debitNo);
            if (AnnexurePrint == null || AnnexurePrint.Count == 0)
            {
                return null;
            }

            var strHtml = new StringBuilder();
            strHtml.Append("<html>");
            strHtml.Append("<head>");
            strHtml.Append("<style>");
            strHtml.Append("table { width: 100%; border-collapse: collapse; }");
            strHtml.Append("th, td { border: 1px solid black; padding: 8px; text-align: left; }");
            strHtml.Append("th { background-color: #f2f2f2; }");
            strHtml.Append("</style>");
            strHtml.Append("</head>");
            strHtml.Append("<body>");

            strHtml.Append("<h2>Debit Annexure Print</h2>");

            strHtml.Append("<table>");
            strHtml.Append("<tr>");

            // Add table headers dynamically
            foreach (var key in AnnexurePrint[0].Keys)
            {
                strHtml.Append($"<th>{key}</th>");
            }
            strHtml.Append("</tr>");

            // Add table rows dynamically
            foreach (var row in AnnexurePrint)
            {
                strHtml.Append("<tr>");
                foreach (var cell in row.Values)
                {
                    strHtml.Append($"<td>{cell}</td>");
                }
                strHtml.Append("</tr>");
            }

            strHtml.Append("</table>");
            strHtml.Append("</body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }

    }
}
