using MaterialSkin.Controls;
using MaterialSkin;
using Xceed.Words.NET;
using System.Diagnostics;



namespace Report
{
    public class SearchForm : MaterialForm
    {
        DateTimePicker date = new();
        MaterialTextBox size = new();

        public SearchForm(){
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;

            Size = new Size(300, 260);
            Text = "بحث";

            MaterialButton searchSizeButton = new()
            {
                Text = " KB حجم الصورة",
                Dock = DockStyle.Top
            };
            searchSizeButton.Click +=  BtnSearchbysize_Click;

            MaterialButton searchDateButton = new()
            {
                Text = "تاريخ التعديل على الصورة",
                Dock = DockStyle.Top
            };
            searchDateButton.Click += BtnSearch_Click;
            
            size.Name = "الحجم";
            size.Width = 200;
            size.Height = 200;


            date.Name = "التاريخ";
            date.Width = 250;
            date.Height = 250;

            TableLayoutPanel layout = new()
            {
                Dock = DockStyle.Fill,
            };        
            layout.Controls.Add(size, 0, 1);
            layout.Controls.Add(searchSizeButton, 0, 2);
            layout.Controls.Add(date, 0, 4);
            layout.Controls.Add(searchDateButton, 0, 5);
        

            Controls.Add(layout);
        }

             private void BtnSearch_Click(object sender, EventArgs e)
        {
            DateTime modificationDate;
            if (!DateTime.TryParse(date.Text, out modificationDate))
            {
                MessageBox.Show("ادخل قيمة صالحة");
                return;
            }
            SearchImagesbydate(modificationDate);
        }





        private void BtnSearchbysize_Click(object sender, EventArgs e)
        {
            long imageSize;
            if (!long.TryParse(size.Text, out imageSize))
            {
                MessageBox.Show("ادخل قيمة صالحة");
                return;
            }
            SearchImagesbysize(imageSize);
        }




        private void SearchImagesbysize(long imageSize)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult result = folderBrowser.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                var matchedImages = new List<string>();

                DirectoryInfo directoryInfo = new DirectoryInfo(folderBrowser.SelectedPath);

                FileInfo[] imageFiles =
                [
                    .. directoryInfo.GetFiles("*.jpg", SearchOption.AllDirectories),
                    .. directoryInfo.GetFiles("*.jpeg", SearchOption.AllDirectories),
                    .. directoryInfo.GetFiles("*.png", SearchOption.AllDirectories),
                    .. directoryInfo.GetFiles("*.bmp", SearchOption.AllDirectories),
                ];

                foreach (var file in imageFiles)
                {
                    long fileSize = file.Length / 1024; 
                    if (fileSize == imageSize) matchedImages.Add(file.FullName);
                }
                if (matchedImages.Count != 0)
                {
                    string message = "";
                    foreach (var path in matchedImages)
                    {
                        message += path + "\n";
                    }
                    MessageBox.Show(message, "الصور المحققة للشرط");
                }
                else
                {
                    MessageBox.Show("لا يوجد أي صورة تحقق الشرط المطلوب");
                }
            }
        }




        private void SearchImagesbydate(DateTime modificationDate)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult result = folderBrowser.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                var matchedImages = new List<string>();

                DirectoryInfo directoryInfo = new DirectoryInfo(folderBrowser.SelectedPath);

                FileInfo[] imageFiles =
                [
                    .. directoryInfo.GetFiles("*.jpg", SearchOption.AllDirectories),
                    .. directoryInfo.GetFiles("*.jpeg", SearchOption.AllDirectories),
                    .. directoryInfo.GetFiles("*.png", SearchOption.AllDirectories),
                    .. directoryInfo.GetFiles("*.bmp", SearchOption.AllDirectories),
                ];

                foreach (var file in imageFiles)
                {
                    if (file.LastWriteTime.Date == modificationDate.Date)
                    {
                        matchedImages.Add(file.FullName);
                    }
                }

                if (matchedImages.Count != 0)
                {
                    string message = "";
                    foreach (var path in matchedImages)
                    {
                        message += path + "\n";
                    }
                    MessageBox.Show(message, "الصور المحققة للشرط");
                }
                else
                {
                    MessageBox.Show("لا يوجد أي صورة تحقق الشرط المطلوب");
                }
            }
        }

    }
}