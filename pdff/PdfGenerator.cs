using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting;
using Org.BouncyCastle.Asn1.X509;

namespace pdff
{
    public class PdfGenerator
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PdfGenerator(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public byte[] GeneratePdf()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // 建立 PDF 寫入器
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                Document document = new Document(pdfDocument);

                // 字型路徑（來自 wwwroot/fonts 資料夾）
                string fontPath = Path.Combine(_webHostEnvironment.WebRootPath, "fonts", "NotoSansTC-Regular.ttf");

                // 使用 PdfFontFactory 生成字型，並強制嵌入
                PdfFont font = PdfFontFactory.CreateFont(
                    fontPath,
                    PdfEncodings.IDENTITY_H,
                    PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED
                );

                // 標題
                // 設定 Logo 圖片
                Image logoImage = new Image(ImageDataFactory.Create("wwwroot/images/logo.png"))
                    .ScaleToFit(50, 50) // 調整圖片大小
                    .SetMarginRight(10); // 與文字的間距

                // 創建一個表格來排列 Logo 和文字
                Table titleTable = new Table(new float[] { 1, 400 }).UseAllAvailableWidth(); // 定義寬度比例
                titleTable.SetMarginTop(20);

                // 右邊放 Logo
                titleTable.AddCell(new Cell()
                    .Add(logoImage)
                    .SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetPaddingLeft(130)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)); // 圖片垂直置中

                // 右邊放公司名稱
                titleTable.AddCell(new Cell()
                    .Add(new Paragraph("啟富興業股份有限公司\n詢價單")
                    .SetFont(font)
                    .SetFontSize(16)
                    .SetBold())
                    .SetBorder(Border.NO_BORDER)
                    .SetPaddingRight(160)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)); // 文字垂直置中

                // 將表格添加到文件
                document.Add(titleTable);

                // 單號與日期（同一行）
                Table headerTable = new Table(new float[] { 1, 1 }).UseAllAvailableWidth();
                headerTable.AddCell(new Cell()
                    .Add(new Paragraph($"詢價單號: IS20241224171601")
                    .SetFont(font))
                    .SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE));
                headerTable.AddCell(new Cell()
                    .Add(new Paragraph($"日期: "+ DateTime.Now)
                    .SetFont(font))
                    .SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE));
                document.Add(headerTable);

                // 供應商資訊與公司資訊
                Table infoTable = new Table(new float[] { 1, 1 }).UseAllAvailableWidth();
                infoTable.SetMarginTop(10);

                // 標題行
                infoTable.AddCell(new Cell()
                    .Add(new Paragraph("供應商資訊")
                    .SetFont(font)
                    .SetFontSize(14)
                    .SetBold())
                    .SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.LEFT));
                infoTable.AddCell(new Cell()
                    .Add(new Paragraph("公司資訊")
                    .SetFont(font)
                    .SetFontSize(14)
                    .SetBold())
                    .SetBorder(Border.NO_BORDER)
                    .SetPaddingLeft(200)  // 增加左側padding，使區塊往右移
                    .SetTextAlignment(TextAlignment.LEFT));

                infoTable.AddCell(new Cell()
                    .Add(new Paragraph($"名稱：測試公司\n統一編號：00000000\n聯絡人：陳先生\n電話：04-00000000#301\n傳真：04-00000000\n地址：407台中市西屯區")
                    .SetFont(font)
                    .SetFontSize(11))
                    .SetBold()
                    .SetBorder(Border.NO_BORDER));
                infoTable.AddCell(new Cell()
                    .Add(new Paragraph($"名稱：啟富企業有限公司\n統一編號：00153661\n聯絡人：李玉琪\n電話：04-25340972\n傳真：04-25340972\n地址：台中市潭子區中山路三段493巷8號")
                    .SetFont(font)
                    .SetFontSize(11))
                    .SetBold()
                    .SetBorder(Border.NO_BORDER)
                    .SetPaddingLeft(200)  // 增加左側padding，使區塊往右移
                    .SetTextAlignment(TextAlignment.LEFT));
                document.Add(infoTable);

                // 添加表格標題
                Paragraph tableTitle = new Paragraph("\n詢價項目")
                    .SetFont(font)
                    .SetFontSize(14)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.LEFT);
                document.Add(tableTitle);

                // 建立詢價表格
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3, 1, 1, 1, 1 }))
                    .UseAllAvailableWidth();

                // 表頭
                table.AddHeaderCell(CreateHeaderCell("項次", font));
                table.AddHeaderCell(CreateHeaderCell("品名/規格", font));
                table.AddHeaderCell(CreateHeaderCell("厚度", font));
                table.AddHeaderCell(CreateHeaderCell("尺寸", font));
                table.AddHeaderCell(CreateHeaderCell("數量", font));
                table.AddHeaderCell(CreateHeaderCell("單價", font));

                // 表格內容
                for (int i = 1; i <= 7; i++)
                {
                    table.AddCell(CreateCell(i.ToString(), font, false));
                    table.AddCell(CreateCell("不鏽鋼/304/2B/黑色板", font, false));
                    table.AddCell(CreateCell("0.1", font, false));
                    table.AddCell(CreateCell("4' x 8'", font, false));
                    table.AddCell(CreateCell("1", font, false));
                    table.AddCell(CreateCell("-", font, false));
                }

                document.Add(table);

                // 詢價項目下方的交期、備註、注意事項表格
                Table detailsTable = new Table(new float[] { 1, 4 }).UseAllAvailableWidth();

                // 交期
                detailsTable.AddCell(new Cell()
                    .Add(new Paragraph("交期").SetFont(font).SetBold())
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE));
                detailsTable.AddCell(new Cell()
                    .Add(new Paragraph("此處可填寫交期內容").SetFont(font))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE));

                // 備註
                detailsTable.AddCell(new Cell()
                    .Add(new Paragraph("備註").SetFont(font).SetBold())
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE));
                detailsTable.AddCell(new Cell()
                    .Add(new Paragraph("此處可填寫備註內容").SetFont(font))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE));

                // 注意事項
                detailsTable.AddCell(new Cell()
                    .Add(new Paragraph("注意事項").SetFont(font).SetBold())
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE));
                detailsTable.AddCell(new Cell()
                    .Add(new Paragraph("1. 詢價商品以型號、規格、數量為準。\n2. 報價請於接到本詢價後3個工作日內回覆。\n3. 報價有效期為25日。").SetFont(font))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE));

                document.Add(detailsTable);

                // 關閉文件
                document.Close();

                // 返回生成的 PDF 資料
                return stream.ToArray();
            }
        }

        private Cell CreateCell(string content, PdfFont font, bool isHeader)
        {
            return new Cell()
                .Add(new Paragraph(content))
                .SetFont(font)
                    .SetFontSize(12)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER);
        }

        private Cell CreateHeaderCell(string content, PdfFont font)
        {
            return new Cell()
                .Add(new Paragraph(content))
                .SetFont(font)
                    .SetFontSize(12)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER);
        }
    }
}
