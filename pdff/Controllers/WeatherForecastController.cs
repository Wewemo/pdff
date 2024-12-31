using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Font;
using iText.Layout.Element;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.IO.Font;
using Path = System.IO.Path;
using iText.Layout.Properties;
using iText.Layout.Borders;  // �[�b��L using �y�y�ϰ�

namespace pdff.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string fontPath;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //    [HttpGet("download")]
        //    public IActionResult DownloadPdf()
        //    {
        //        try
        //        {
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                // �إ� PDF ���
        //                var writer = new PdfWriter(ms);
        //                var pdf = new PdfDocument(writer);
        //                var document = new Document(pdf, PageSize.A4);

        //                // �]�w����r��
        //                string fontPath = Path.Combine(_webHostEnvironment.WebRootPath, "fonts", "NotoSansTC-Regular.ttf");
        //                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

        //                // �[�J���e
        //                Paragraph para = new Paragraph("�o�OPDF���դ��");
        //                para.SetFont(font);
        //                document.Add(para);

        //                // �إߪ��
        //                Table table = new Table(3);
        //                Cell cell1 = new Cell().Add(new Paragraph("����1").SetFont(font));
        //                Cell cell2 = new Cell().Add(new Paragraph("����2").SetFont(font));
        //                Cell cell3 = new Cell().Add(new Paragraph("����3").SetFont(font));
        //                table.AddCell(cell1);
        //                table.AddCell(cell2);
        //                table.AddCell(cell3);
        //                document.Add(table);

        //                // �������
        //                document.Flush();
        //                document.Close();

        //                // �^���ɮ�
        //                return File(ms.ToArray(), "application/pdf", "�U���ɮ�.pdf");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest($"�ͦ�PDF�ɵo�Ϳ��~: {ex.Message}");
        //        }
        //    }

        //    [HttpPost("generate-pdf")]
        //    public IActionResult GeneratePdf([FromBody] PdfDataModel model)
        //    {
        //        try
        //        {
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                var writer = new PdfWriter(ms);
        //                var pdf = new PdfDocument(writer);
        //                var document = new Document(pdf, PageSize.A4);

        //                // �]�w����r��
        //                string fontPath = Path.Combine(_webHostEnvironment.WebRootPath, "fonts", "NotoSansTC-Regular.ttf");
        //                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

        //                // �ק���D�����G
        //                Paragraph title = new Paragraph(model.Title)
        //                    .SetFont(font)
        //                    .SetFontSize(20);
        //                // �p�G�n�]�w����A�ڭ̥i�H�ϥΦr�����覡�G
        //                PdfFont boldFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
        //                title.SetFont(boldFont);

        //                // �Ϊ̧������]�w����A�u�O�d�r��j�p�G
        //                //Paragraph title = new Paragraph(model.Title)
        //                //    .SetFont(font)
        //                //    .SetFontSize(20);

        //                // �[�J���e
        //                Paragraph content = new Paragraph(model.Content)
        //                    .SetFont(font)
        //                    .SetFontSize(12);
        //                document.Add(content);

        //                // �p�G�������
        //                if (model.TableData != null && model.TableData.Any())
        //                {
        //                    Table table = new Table(model.TableData.First().Count);
        //                    foreach (var row in model.TableData)
        //                    {
        //                        foreach (var cellData in row)
        //                        {
        //                            Cell cell = new Cell();
        //                            cell.Add(new Paragraph(cellData).SetFont(font));
        //                            table.AddCell(cell);
        //                        }
        //                    }
        //                    document.Add(table);
        //                }

        //                // �������
        //                document.Flush();
        //                document.Close();

        //                return File(ms.ToArray(), "application/pdf", $"{model.FileName ?? "�U���ɮ�"}.pdf");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest($"�ͦ�PDF�ɵo�Ϳ��~: {ex.Message}");
        //        }
        //    }
        //}

        //public class PdfDataModel
        //{
        //    public string Title { get; set; }
        //    public string Content { get; set; }
        //    public List<List<string>> TableData { get; set; }
        //    public string FileName { get; set; }

        [HttpGet("download-pdf")]
        public IActionResult DownloadPdf()
        {
            // �����n�ͦ������e
            var data = new[]
            {
                new { Name = "Alice", Age = 25 },
                new { Name = "Bob", Age = 30 },
                new { Name = "Charlie", Age = 35 }
            };

            // �ϥ� MemoryStream �ӥͦ� PDF
            using (var stream = new MemoryStream())
            {
                // ��l�� PDF Writer �M Document
                using (var writer = new PdfWriter(stream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        // �K�[���D
                        document.Add(new Paragraph("User Information").SetBold().SetFontSize(18));

                        // �K�[��椺�e
                        foreach (var item in data)
                        {
                            document.Add(new Paragraph($"Name: {item.Name}, Age: {item.Age}"));
                        }

                        document.Close();
                    }
                }

                // �^�� PDF �ɮפ��e
                var fileName = "UserInformation.pdf";
                return File(stream.ToArray(), "application/pdf", fileName);
            }
        }

        [HttpGet("generate-inquiry-pdf")]
        public IActionResult GenerateInquiryPdf()
        {
            // ����ơA�����q��Ʈw���X
            var supplierInfo = new
            {
                Name = "����2",
                TaxId = "00000000",
                Contact = "������",
                Phone = "04-00000000#301",
                Fax = "04-00000000",
                Address = "407�x������ٰ�"
            };

            var companyInfo = new
            {
                Name = "�ҴI���~�ѥ��������q",
                TaxId = "00153661",
                Contact = "���ܳ�",
                Phone = "04-25340972",
                Fax = "",
                Address = "�x������l�Ϥ��s���T�q493��8��"
            };

            var items = new List<dynamic>
            {
                new { Index = 1, Name = "���ÿ�/304/2B/�¦�O", Thickness = "0.1", Size = "4' x 8'", Quantity = 1, Remarks = "" },
                new { Index = 2, Name = "���ÿ�/304/2B/�¦�O", Thickness = "0.1", Size = "4' x 8'", Quantity = 1, Remarks = "" },
                new { Index = 3, Name = "���ÿ�/304/2B/�¦�O", Thickness = "0.1", Size = "4' x 8'", Quantity = 1, Remarks = "" }
            };

            var pdfGenerator = new PdfGenerator(_webHostEnvironment);
            var pdfContent = pdfGenerator.GeneratePdf();
            return File(pdfContent, "application/pdf", "Inquiry.pdf");
        }
    }
}
