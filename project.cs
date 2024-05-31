using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Project
{
    public partial class MyForm : Form
    {
        private PictureBox pictureBox1;
        //private PictureBox pictureBox2;    
        private Bitmap? originalImage;
        
        MyButton load1Button = new();
        MyButton load2Button = new();
        MyButton classifyButton = new();
    

        public MyForm()
        {
            pictureBox1 = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Normal
            };
            BackColor = ColorTranslator.FromHtml("#202123");


            load1Button = new()
            {
                Text = "إدخال صورة",
                Dock = DockStyle.Top
            };
            load1Button.Click += (sender, e) =>
            {
                LoadImage(sender, e, true);
            };

            load2Button = new()
            {
                Text = "مقارنة",
                Dock = DockStyle.Top,
                Visible = false,
            };
            load2Button.Click += (sender, e) =>
            {
                LoadImage(sender, e, false); //not working
            };

            classifyButton = new()
            {
                Text = "تصنيف",
                Dock = DockStyle.Top,
                Visible = false,
            };
            classifyButton.Click += ClassifyImage;

        
            TableLayoutPanel layout = new()
            {
                Dock = DockStyle.Fill
            };
            //layout.RowCount = 7;
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            // layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            
            layout.Controls.Add(pictureBox1, 0, 0);
            layout.Controls.Add(load1Button, 0, 2);
            layout.Controls.Add(load2Button, 0, 3);
            layout.Controls.Add(classifyButton, 0, 4);
            //layout.SetRowSpan(pictureBox2, 20);

            Controls.Add(layout);
        }

        private void LoadImage(object? sender, EventArgs e, bool og)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap selectedImage = new(openFileDialog.FileName);
                    if(og){
                        originalImage = selectedImage;
                        pictureBox1.Image = originalImage;
                        load2Button.Visible = true;
                        classifyButton.Visible = true;
                    }
                    else CompareImages(selectedImage); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("\nخطا في اختيار الصورة" + ex.Message);
                }
            }
            // else{
            //     MessageBox.Show(DialogResult.ToString());
            // }
        }

        private void CompareImages(Bitmap newImage)
        {
            // what if they werent the same lsize, return false?
            double simillar = 0;
     
            for (int x = 0; x < Math.Min(originalImage!.Width, newImage.Width); x++)
            {
                for (int y = 0; y < Math.Min(originalImage!.Height, newImage.Height); y++)
                {
                    Color first = originalImage.GetPixel(x, y);
                    Color second = newImage.GetPixel(x, y);
                    if (first.R == second.R && first.G == second.G && first.B == second.B) simillar++;
                }
            }
            double percentage = simillar/(originalImage.Width*originalImage.Height)*100;
           
            MessageBox.Show(
                (percentage > 0.7 ? "لا يوجد تقدم او تأخر ملحوظ في المرض" : "يوجد تغير في حالة المرض") + $"\nنسبة التشابه {percentage}"
            );
        }

        private void ClassifyImage(object? sender, EventArgs e)
        {
            MessageBox.Show("حالة سيئة");
        }


        static class Program
        {
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MyForm());
            }
        }

    }
}
