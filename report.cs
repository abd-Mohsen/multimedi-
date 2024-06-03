using MaterialSkin.Controls;
using MaterialSkin;
using Xceed.Words.NET;
using System.Diagnostics;
using Project;
using Spire.Doc;

using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace Report
{
    public class ReportForm : MaterialForm
    {
        TextBox textBox;
        MaterialButton generateButton;
        MaterialButton exportButton;

        MainForm mainForm;
       
        public ReportForm(MainForm mainForm){
            this.mainForm = mainForm;
            
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;

            Size = new Size(300, 450);
            Text = "انشاء و تصدير تقرير";

            textBox = new()
            {
                Multiline = true,
                Height = 250,
                Width = 290,
                Name = "اكتب التقرير هنا",
                //ScrollBars = RichTextBoxScrollBars.Vertical,
                //Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            };
            
            generateButton = new()
            {
                Text = "إنشاء تقرير",
                Dock = DockStyle.Top
            };
            generateButton.Click += GenerateReport;

            exportButton = new()
            {
                Text = "pdf",
                Dock = DockStyle.Top,
                Visible = false,
            };
            exportButton.Click += ExportReport;

            TableLayoutPanel layout = new()
            {
                Dock = DockStyle.Fill,
                //AutoScroll = true,
            };
            
            //layout.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            
            layout.Controls.Add(textBox, 0, 0);
            layout.Controls.Add(generateButton, 0, 1);
            layout.Controls.Add(exportButton, 0, 2);
        

            Controls.Add(layout);
        }

        private void GenerateReport(object? sender, EventArgs e){
            DocX wordFile = DocX.Create("output/report");
            wordFile.SetDefaultFont(fontFamily: null, fontSize: 16);
            wordFile.InsertParagraph(textBox.Text);
            //wordFile.AddImage("output/selected.jpeg");
            wordFile.Save();
            exportButton.Visible = true;
            //Process.Start("winword.exe","output/report.docx");
        }

        private void ExportReport(object? sender, EventArgs e){
                
            try
            {
                Document document = new Document();
                document.LoadFromFile("output/report.docx");
                document.SaveToFile("output/report.pdf", FileFormat.PDF);
                MessageBox.Show("Conversion Successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
    
        }

    private void CreatePdf2(object? sender, EventArgs e)
    {
        PdfDocument document = new PdfDocument();
        document.Info.Title = "medical report";

        PdfPage page = document.AddPage();

        XGraphics gfx = XGraphics.FromPdfPage(page);

        XFont font = new("Verdana", 16);

        // Split the text into lines
        string[] lines = textBox.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        // Define starting position
        double yPos = 40; // Start 40 points from the top of the page
        double lineHeight = font.GetHeight() + 5; // Line height including some space between lines

        foreach (string line in lines)
        {
            gfx.DrawString(line, font, XBrushes.Black, new XRect(20, yPos, page.Width - 40, page.Height), XStringFormats.TopLeft);
            yPos += lineHeight; // Move down for the next line
        }

        // Save the document
        document.Save("output/report2.pdf");
    }

    }
}