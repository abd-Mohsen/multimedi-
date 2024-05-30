// using System;
// using System.IO;
// using System.Linq;
// using System.Collections.Generic;
// using System.Drawing;
// using System.Drawing.Imaging;
// using System.Windows.Forms;

// namespace Project
// {
//     public partial class MyForm : Form
//     {
//         private PictureBox pictureBox;
//         private PictureBox pictureBox2;
//         private List<Rectangle> rois = new List<Rectangle>();
//         private Pen roiPen = new Pen(Color.Yellow, 2);
//         private Bitmap originalImage;
//         private Bitmap coloredImage;
//         private bool isDrawing = false;
//         private Point startPoint;
//         private string colormap = "RGB"; // Default colormap



//         public MyForm()
//         {
//                       pictureBox = new PictureBox();
//             pictureBox.Dock = DockStyle.Fill;
//             pictureBox.SizeMode = PictureBoxSizeMode.Normal;
//             pictureBox.Paint += paint;
//             pictureBox.MouseDown += MouseDown;
//             pictureBox.MouseMove += MouseMove;
//             pictureBox.MouseUp += MouseUp;

//             Button btnLoad = new Button();
//             btnLoad.Text = "Load Radiograph";
//             btnLoad.Dock = DockStyle.Top;
//             btnLoad.Click += Load;

//             Button btnLoadSecondImage = new Button();
//             btnLoadSecondImage.Text = "Load Second Radiograph";
//             btnLoadSecondImage.Dock = DockStyle.Top;
//             btnLoadSecondImage.Click += LoadSecond;

//             Button btnColorSelectedRegions = new Button();
//             btnColorSelectedRegions.Text = "Color Selected Regions";
//             btnColorSelectedRegions.Dock = DockStyle.Top;
//             btnColorSelectedRegions.Click += ApplyColor;

//             Button btnDisplayColoredImage = new Button();
//             btnDisplayColoredImage.Text = "Display Colored Image";
//             btnDisplayColoredImage.Dock = DockStyle.Top;
//             btnDisplayColoredImage.Click += Display;

//             Button btnSaveColoredImage = new Button();
//             btnSaveColoredImage.Text = "Save Colored Image";
//             btnSaveColoredImage.Dock = DockStyle.Top;
//             btnSaveColoredImage.Click += Save;

//             ComboBox colormapSelector = new ComboBox();
//             colormapSelector.Dock = DockStyle.Top;
//             colormapSelector.Items.AddRange(new string[] { "RGB", "Grayscale", "Jet", "Viridis" });
//             colormapSelector.SelectedIndexChanged += (sender, e) => colormap = colormapSelector.SelectedItem.ToString();
//             colormapSelector.SelectedIndex = 0;

//             TableLayoutPanel layout = new TableLayoutPanel();
//             layout.Dock = DockStyle.Fill;
//             layout.RowCount = 7;
//             layout.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
//             layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//             layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//             layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//             layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//             layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//             layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//             layout.Controls.Add(pictureBox, 2, 0);
//             layout.Controls.Add(btnLoad, 0, 1);
//             layout.Controls.Add(btnLoadSecondImage, 0, 2);
//             layout.Controls.Add(colormapSelector, 0, 3);
//             layout.Controls.Add(btnColorSelectedRegions, 0, 4);
//             layout.Controls.Add(btnDisplayColoredImage, 0, 5);
//             layout.Controls.Add(btnSaveColoredImage, 0, 6);
//             //layout.SetRowSpan(pictureBox2, 20);

//             Controls.Add(layout);
//         }



//         private void paint(object sender, PaintEventArgs e)
//         {
//             for (int i = 0; i < rois.Count; i++)
//             {
//                 e.Graphics.DrawRectangle(roiPen, rois[i]);
//             }

//             if (isDrawing)
//             {
//                 Rectangle currentRoi = GetRoi();
//                 e.Graphics.DrawRectangle(roiPen, currentRoi);
//             }
//         }


//         private void MouseDown(object sender, MouseEventArgs e)
//         {
//             if (pictureBox.Image != null)
//             {
//                 isDrawing = true;
//                 startPoint = e.Location;
//             }
//         }



//         private void MouseMove(object sender, MouseEventArgs e)
//         {
//             if (isDrawing)
//             {
//                 pictureBox.Invalidate();
//             }
//         }

//         private void MouseUp(object sender, MouseEventArgs e)
//         {
//             if (isDrawing)
//             {
//                 isDrawing = false;
//                 Rectangle newRoi = GetRoi();
//                 rois.Add(newRoi);
//                 pictureBox.Invalidate();
//             }
//         }

//         private Rectangle GetRoi()
//         {
//             Point endPoint = pictureBox.PointToClient(Cursor.Position);
//             int x = Math.Min(startPoint.X, endPoint.X);
//             int y = Math.Min(startPoint.Y, endPoint.Y);
//             int width = Math.Abs(startPoint.X - endPoint.X);
//             int height = Math.Abs(startPoint.Y - endPoint.Y);
//             return new Rectangle(x, y, width, height);
//         }


//         private void Load(object sender, EventArgs e)
//         {
//             OpenFileDialog openFileDialog = new OpenFileDialog();
//             openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";

//             if (openFileDialog.ShowDialog() == DialogResult.OK)
//             {
//                 try
//                 {
//                     originalImage = new Bitmap(openFileDialog.FileName);
//                     pictureBox.Image = originalImage;
//                     coloredImage = null; // Reset colored image
//                 }
//                 catch (Exception ex)
//                 {
//                     MessageBox.Show("Error loading the radiograph: " + ex.Message);
//                 }
//             }
//         }


//         private void LoadSecond(object sender, EventArgs e)
//         {
//             OpenFileDialog openFileDialog = new OpenFileDialog();
//             openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";

//             if (openFileDialog.ShowDialog() == DialogResult.OK)
//             {
//                 try
//                 {
//                     pictureBox2.Image = new Bitmap(openFileDialog.FileName);
//                 }
//                 catch (Exception ex)
//                 {
//                     MessageBox.Show("Error loading the second image: " + ex.Message);
//                 }
//             }
//         }

//         private void ColorSelected(object sender, EventArgs e)
//         {
//             if (originalImage != null)
//             {
//                 coloredImage = GetColoredImg();
//                 pictureBox.Image = coloredImage;
//             }
//             else
//             {
//                 MessageBox.Show("Please load an image first.");
//             }
//         }

//         private void Display(object sender, EventArgs e)
//         {
//             if (coloredImage != null)
//             {
//                 Form coloredImageForm = new Form();
//                 coloredImageForm.Text = "Colored Image";

//                 PictureBox coloredPictureBox = new PictureBox();
//                 coloredPictureBox.Dock = DockStyle.Fill;
//                 coloredPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
//                 coloredPictureBox.Image = coloredImage;

//                 coloredImageForm.Controls.Add(coloredPictureBox);
//                 coloredImageForm.ShowDialog();
//             }
//             else
//             {
//                 MessageBox.Show("Please color the selected regions first.");
//             }
//         }

//         private void Save(object sender, EventArgs e)
//         {
//             if (coloredImage != null)
//             {
//                 SaveFileDialog saveFileDialog = new SaveFileDialog();
//                 saveFileDialog.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|PNG Image|*.png";
//                 saveFileDialog.Title = "Save Colored Image";
//                 saveFileDialog.ShowDialog();

//                 if (saveFileDialog.FileName != "")
//                 {
//                     ImageFormat format = ImageFormat.Jpeg;
//                     switch (saveFileDialog.FilterIndex)
//                     {
//                         case 1:
//                             format = ImageFormat.Jpeg;
//                             break;
//                         case 2:
//                             format = ImageFormat.Bmp;
//                             break;
//                         case 3:
//                             format = ImageFormat.Png;
//                             break;
//                     }
//                     coloredImage.Save(saveFileDialog.FileName, format);
//                 }
//             }
//             else
//             {
//                 MessageBox.Show("Please color the selected regions first.");
//             }
//         }

//         private Bitmap GetColoredImg()
//         {
//             Bitmap result = new Bitmap(originalImage);
//             for (int i = 0; i < rois.Count; i++)
//             {
//                 for (int y = rois[i].Top; y < rois[i].Bottom; y++)
//                 {
//                     for (int x = rois[i].Left; x < rois[i].Right; x++)
//                     {
//                         if (x >= 0 && x < result.Width && y >= 0 && y < result.Height)
//                         {
//                             Color originalColor = originalImage.GetPixel(x, y);
//                             Color newColor = ApplyColor(originalColor, colormap);
//                             result.SetPixel(x, y, newColor);
//                         }
//                     }
//                 }
//             }
//             return result;
//         }

//         private Color ApplyColor(Color originalColor, string colormap)
//         {
//             int grayValue = (int)(originalColor.R * 0.1 + originalColor.G * 0.8 + originalColor.B * 0.1);

//             switch (colormap)
//             {
//                 case "RGB":
//                     return Color.FromArgb(grayValue, 0, 255 - grayValue);
//                 case "Grayscale":
//                     return Color.FromArgb(grayValue, grayValue, grayValue);
//                 case "Jet":
//                     return Jet(grayValue);
//                 case "Viridis":
//                     return Viridis(grayValue - 10);
//                 default:
//                     return originalColor;
//             }
//         }


//         private Color Viridis(int value)
//         {
//             // تطبيق colormap Viridis - يمكنك تعديل هذه الوظيفة لتناسب تطبيقك.
//             // هذه هي القيم التقريبية للطيف اللوني Viridis.
//             value = Math.Max(0, Math.Min(255, value));
//             double[][] viridisColors = new double[][]
//             {
//                 new double[] { 68 / 255.0, 1, 84 / 255.0 },
//                 new double[] { 72 / 255.0, 40 / 255.0, 120 / 255.0 },
//                 new double[] { 62 / 255.0, 74 / 255.0, 137 / 255.0 },
//                 new double[] { 49 / 255.0, 104 / 255.0, 142 / 255.0 },
//                 new double[] { 38 / 255.0, 130 / 255.0, 142 / 255.0 },
//                 new double[] { 31 / 255.0, 158 / 255.0, 137 / 255.0 },
//                 new double[] { 53 / 255.0, 183 / 255.0, 121 / 255.0 },
//                 new double[] { 109 / 255.0, 205 / 255.0, 89 / 255.0 },
//                 new double[] { 180 / 255.0, 222 / 255.0, 44 / 255.0 },
//                 new double[] { 253 / 255.0, 231 / 255.0, 37 / 255.0 }
//             };

//             double fraction = value / 255.0 * (viridisColors.Length - 1);
//             int index1 = (int)Math.Floor(fraction);
//             int index2 = (int)Math.Ceiling(fraction);
//             fraction -= index1;

//             double r = (viridisColors[index2][0] - viridisColors[index1][0]) * fraction + viridisColors[index1][0];
//             double g = (viridisColors[index2][1] - viridisColors[index1][1]) * fraction + viridisColors[index1][1];
//             double b = (viridisColors[index2][2] - viridisColors[index1][2]) * fraction + viridisColors[index1][2];

//             return Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
//         }


//         private Color Jet(int value)
//         {
//             value = Math.Max(0, Math.Min(255, value));
//             int r = (value < 85) ? 0 :
//                     (value < 170) ? (int)(255 * (value - 85) / 85.0) : 255;
//             int g = (value < 85) ? (int)(255 * value / 85.0) :
//                     (value < 170) ? 255 : (int)(255 * (255 - value) / 85.0);
//             int b = (value < 85) ? 255 :
//                     (value < 170) ? (int)(255 * (170 - value) / 85.0) : 0;
//             return Color.FromArgb(r, g, b);
//         }

//         static class Program
//         {
//             [STAThread]
//             static void Main()
//             {
//                 Application.EnableVisualStyles();
//                 Application.SetCompatibleTextRenderingDefault(false);
//                 Application.Run(new MyForm());
//             }
//         }

//     }
// }
