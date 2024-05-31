using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Material
{
    public partial class MainForm2 : MaterialForm
    {
        private MaterialButton materialButton1;
        private PictureBox pictureBox1;

        public MainForm2()
        {
            //InitializeComponent();

            // Initialize MaterialSkinManager
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Create and customize the button
            materialButton1 = new MaterialButton();
            materialButton1.Text = "Click Me";
            materialButton1.Size = new Size(100, 50);
            materialButton1.Location = new Point(50, 50);
            materialButton1.Click += MaterialButton1_Click;
            Controls.Add(materialButton1);

            // Create and customize the picture box
            pictureBox1 = new PictureBox();
            pictureBox1.Image = Image.FromFile("path_to_your_image.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Size = new Size(200, 200);
            pictureBox1.Location = new Point(50, 150);
            Controls.Add(pictureBox1);
        }

        private void MaterialButton1_Click(object sender, EventArgs e)
        {
            // Handle button click event here
            MessageBox.Show("Button clicked!");
        }
    }
}