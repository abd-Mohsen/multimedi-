using MaterialSkin.Controls;
using MaterialSkin;
using Xceed.Words.NET;
using System.Diagnostics;
using Project;

namespace Report
{
    public class ReportForm : MaterialForm
    {
        MaterialTextBox textBox;
        MaterialButton generateButton;
        MaterialButton exportButton;

        MainForm mainForm;
       
        public ReportForm(MainForm mainForm){
            this.mainForm = mainForm;
            
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;

            Size = new Size(300, 400);
            Text = "انشاء و تصدير تقرير";

            textBox = new()
            {
                Multiline = true,
                Height = 200,
                Width = 200,
                ScrollBars = RichTextBoxScrollBars.Vertical,
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
                Text = "تصدير ك pdf",
                Dock = DockStyle.Top,
                Visible = false,
            };
            generateButton.Click += GenerateReport;

            TableLayoutPanel layout = new()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };
            
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            
            layout.Controls.Add(textBox, 0, 0);
            layout.Controls.Add(generateButton, 0, 1);
        

            Controls.Add(layout);
        }

        private void GenerateReport(object? sender, EventArgs e){
            DocX wordFile = DocX.Create("output/report.docx");
            wordFile.InsertParagraph(textBox.Text);
            wordFile.Save();
            exportButton.Visible = true;
        }

        private void exportReport(object? sender, EventArgs e){
            //export to pdf
        }
    }
}