using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Font;
using iText.Layout.Element;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.IO.Font;
using Path = System.IO.Path;
using iText.Layout.Properties;
using iText.Layout.Borders;  // 加在其他 using 語句區域

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
        //                // 建立 PDF 文件
        //                var writer = new PdfWriter(ms);
        //                var pdf = new PdfDocument(writer);
        //                var document = new Document(pdf, PageSize.A4);

        //                // 設定中文字型
        //                string fontPath = Path.Combine(_webHostEnvironment.WebRootPath, "fonts", "NotoSansTC-Regular.ttf");
        //                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

        //                // 加入內容
        //                Paragraph para = new Paragraph("這是PDF測試文件");
        //                para.SetFont(font);
        //                document.Add(para);

        //                // 建立表格
        //                Table table = new Table(3);
        //                Cell cell1 = new Cell().Add(new Paragraph("項目1").SetFont(font));
        //                Cell cell2 = new Cell().Add(new Paragraph("項目2").SetFont(font));
        //                Cell cell3 = new Cell().Add(new Paragraph("項目3").SetFont(font));
        //                table.AddCell(cell1);
        //                table.AddCell(cell2);
        //                table.AddCell(cell3);
        //                document.Add(table);

        //                // 關閉文件
        //                document.Flush();
        //                document.Close();

        //                // 回傳檔案
        //                return File(ms.ToArray(), "application/pdf", "下載檔案.pdf");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest($"生成PDF時發生錯誤: {ex.Message}");
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

        //                // 設定中文字型
        //                string fontPath = Path.Combine(_webHostEnvironment.WebRootPath, "fonts", "NotoSansTC-Regular.ttf");
        //                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

        //                // 修改標題部分：
        //                Paragraph title = new Paragraph(model.Title)
        //                    .SetFont(font)
        //                    .SetFontSize(20);
        //                // 如果要設定粗體，我們可以使用字型的方式：
        //                PdfFont boldFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
        //                title.SetFont(boldFont);

        //                // 或者完全不設定粗體，只保留字體大小：
        //                //Paragraph title = new Paragraph(model.Title)
        //                //    .SetFont(font)
        //                //    .SetFontSize(20);

        //                // 加入內容
        //                Paragraph content = new Paragraph(model.Content)
        //                    .SetFont(font)
        //                    .SetFontSize(12);
        //                document.Add(content);

        //                // 如果有表格資料
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

        //                // 關閉文件
        //                document.Flush();
        //                document.Close();

        //                return File(ms.ToArray(), "application/pdf", $"{model.FileName ?? "下載檔案"}.pdf");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest($"生成PDF時發生錯誤: {ex.Message}");
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
            // 模擬要生成的內容
            var data = new[]
            {
                new { Name = "Alice", Age = 25 },
                new { Name = "Bob", Age = 30 },
                new { Name = "Charlie", Age = 35 }
            };

            // 使用 MemoryStream 來生成 PDF
            using (var stream = new MemoryStream())
            {
                // 初始化 PDF Writer 和 Document
                using (var writer = new PdfWriter(stream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        // 添加標題
                        document.Add(new Paragraph("User Information").SetBold().SetFontSize(18));

                        // 添加表格內容
                        foreach (var item in data)
                        {
                            document.Add(new Paragraph($"Name: {item.Name}, Age: {item.Age}"));
                        }

                        document.Close();
                    }
                }

                // 回傳 PDF 檔案內容
                var fileName = "UserInformation.pdf";
                return File(stream.ToArray(), "application/pdf", fileName);
            }
        }

        [HttpGet("generate-inquiry-pdf")]
        public IActionResult GenerateInquiryPdf()
        {
            // 假資料，模擬從資料庫取出
            var supplierInfo = new
            {
                Name = "測試2",
                TaxId = "00000000",
                Contact = "陳先生",
                Phone = "04-00000000#301",
                Fax = "04-00000000",
                Address = "407台中市西屯區"
            };

            var companyInfo = new
            {
                Name = "啟富興業股份有限公司",
                TaxId = "00153661",
                Contact = "李至凱",
                Phone = "04-25340972",
                Fax = "",
                Address = "台中市潭子區中山路三段493巷8號"
            };

            var items = new List<dynamic>
            {
                new { Index = 1, Name = "不鏽鋼/304/2B/黑色板", Thickness = "0.1", Size = "4' x 8'", Quantity = 1, Remarks = "" },
                new { Index = 2, Name = "不鏽鋼/304/2B/黑色板", Thickness = "0.1", Size = "4' x 8'", Quantity = 1, Remarks = "" },
                new { Index = 3, Name = "不鏽鋼/304/2B/黑色板", Thickness = "0.1", Size = "4' x 8'", Quantity = 1, Remarks = "" }
            };

            var pdfGenerator = new PdfGenerator(_webHostEnvironment);
            var pdfContent = pdfGenerator.GeneratePdf();
            return File(pdfContent, "application/pdf", "Inquiry.pdf");
        }
    }
}
