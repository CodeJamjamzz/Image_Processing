using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using WebCamLib;
using ConvolutionaryMatrix;

namespace Image_Processing
{
    public partial class Pinca_Image_Processing : Form
    {

        Bitmap processedImage = null;
        Bitmap imageA, imageB;
        private Device[] webcam;
        private bool isVideo = false;
        Device device; 
        public Pinca_Image_Processing()
        {
            InitializeComponent();
        }
         

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (file.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(file.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (file.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(file.FileName);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (isVideo)
            {
                isVideo = false;
                button6.Text = "Display Webcam";
                webcam[0].Stop();
            }

            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (file.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(file.FileName);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (file.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.Image = Image.FromFile(file.FileName);
                imageB = new Bitmap(pictureBox3.Image);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (file.ShowDialog() == DialogResult.OK)
            {
                pictureBox4.Image = Image.FromFile(file.FileName);
                imageA = new Bitmap(pictureBox4.Image);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap originalImage = new Bitmap(pictureBox1.Image);
                Bitmap copyImage = new Bitmap(originalImage.Width, originalImage.Height);

                for (int x = 0; x < originalImage.Width; x++)
                {
                    for (int y = 0; y < originalImage.Height; y++)
                    {
                        Color pixel = originalImage.GetPixel(x, y);
                        copyImage.SetPixel(x, y, pixel);
                    }
                }

                pictureBox2.Image = copyImage;
                processedImage = copyImage;
            }
        }

        private void greyScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image != null)
            {
                Bitmap originalImage = new Bitmap(pictureBox1.Image);
                Bitmap copyImage = new Bitmap(originalImage.Width, originalImage.Height);

                for (int x = 0; x < originalImage.Width; x++)
                {
                    for (int y = 0; y < originalImage.Height; y++)
                    {
                        Color pixel = originalImage.GetPixel(x, y);
                        int greyImage = (pixel.R + pixel.G + pixel.B) / 3;
                        Color greyColor = Color.FromArgb(greyImage, greyImage, greyImage);
                        copyImage.SetPixel(x, y, greyColor);
                    }
                }

                pictureBox2.Image = copyImage;
                processedImage = copyImage;
            }
        }
        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap originalImage = new Bitmap(pictureBox1.Image);
                Bitmap copyImage = new Bitmap(originalImage.Width, originalImage.Height);

                for (int x = 0; x < originalImage.Width; x++)
                {
                    for (int y = 0; y < originalImage.Height; y++)
                    {
                        Color pixel = originalImage.GetPixel(x, y);
                        Color colorInversion = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                        copyImage.SetPixel(x, y, colorInversion);
                    }
                }

                pictureBox2.Image = copyImage;
                processedImage = copyImage;
            }
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap originalImage = new Bitmap(pictureBox1.Image);
                Bitmap copyImage = new Bitmap(originalImage.Width, originalImage.Height);

                for (int x = 0; x < originalImage.Width; x++)
                {
                    for (int y = 0; y < originalImage.Height; y++)
                    {
                        Color pixel = originalImage.GetPixel(x, y);
                        int grey = (pixel.R + pixel.G + pixel.B) / 3;
                        originalImage.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                    }
                }

                int[] histogram = new int[256];
                for (int x = 0; x < copyImage.Width; x++)
                {
                    for (int y = 0; y < copyImage.Height; y++)
                    {
                        int intensity = originalImage.GetPixel(x, y).R;
                        histogram[intensity]++;
                    }
                }

                Bitmap histgramDiagram = new Bitmap(originalImage.Width, originalImage.Height);

                int max = histogram.Max();

                using (Graphics scale = Graphics.FromImage(histgramDiagram))
                {
                    scale.Clear(Color.White);

                    for (int i = 0; i < 256; i++)
                    {
                        int barHeight = (int)((histogram[i] / (float)max) * histgramDiagram.Height);
                        scale.DrawLine(Pens.Black,
                                       new Point(i, histgramDiagram.Height),
                                       new Point(i, histgramDiagram.Height - barHeight));
                    }
                }

                pictureBox2.Image = histgramDiagram;
                processedImage = copyImage;

            }
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap originalImage = new Bitmap(pictureBox1.Image);
                Bitmap copyImage = new Bitmap(originalImage.Width, originalImage.Height);

                for (int x = 0; x < originalImage.Width; x++)
                {
                    for (int y = 0; y < originalImage.Height; y++)
                    {
                        Color pixel = originalImage.GetPixel(x, y);
                        int red = Math.Min(255, (int)((0.393 * pixel.R) + (0.769 * pixel.G) + (0.189 * pixel.B)));
                        int green = Math.Min(255, (int)((0.349 * pixel.R) + (0.686 * pixel.G) + (0.168 * pixel.B)));
                        int blue = Math.Min(255, (int)((0.272 * pixel.R) + (0.534 * pixel.G) + (0.131 * pixel.B)));
                        copyImage.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                }

                pictureBox2.Image = copyImage;
                processedImage = copyImage;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (processedImage != null)
            {
                Graphics image = Graphics.FromImage(processedImage);
                string downloadsPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads"
                );

                string filePath = Path.Combine(downloadsPath, "processed_image.png");
                int counter = 0;
                while (File.Exists(filePath))
                {

                    counter++;
                    filePath = Path.Combine(downloadsPath, $"processed_image_{counter}.png");

                }
                processedImage.Save(filePath, ImageFormat.Png);
                MessageBox.Show($"Saved Image at: {filePath}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox3.Image != null && pictureBox4.Image != null)
            {
                Color mygreen = Color.FromArgb(0, 255, 0);
                int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
                int threshold = 25;
                Bitmap resultImage = new Bitmap(imageB.Width, imageB.Height);

                for (int i = 0; i < imageB.Width; i++)
                {
                    for (int j = 0; j < imageB.Height; j++)
                    {
                        Color pixel = imageB.GetPixel(i, j);
                        Color backpixel = imageA.GetPixel(i, j);
                        int grey = (pixel.R + pixel.G + pixel.B) / 3;
                        int subtractValue = Math.Abs(grey - greygreen);
                        if (subtractValue < threshold && pixel.G > pixel.R && pixel.G > pixel.B)
                            resultImage.SetPixel(i, j, backpixel);
                        else
                            resultImage.SetPixel(i, j, pixel);

                    }
                }

                pictureBox5.Image = resultImage;
            }
        }

        private void webCamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isVideo)
            {
                pictureBox1.Image = null;
            }

            webcam = DeviceManager.GetAllDevices();
            if (webcam.Length > 0)
            {
                isVideo = true;
                Device device = webcam[0];
                device.ShowWindow(pictureBox1);
                pictureBox1.Refresh();
            }
            else
            {
                MessageBox.Show("No webcam found");
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (button6.Text == "Picture" && webcam != null)
            {
                try
                {
                    device.Sendmessage();

                    if (Clipboard.ContainsImage())
                    {
                        Image snapshot = Clipboard.GetImage();
                        pictureBox1.Image = new Bitmap(snapshot, pictureBox1.Width, pictureBox1.Height);
                    }

                    device.Stop();
                    device = null;

                    button6.Text = "Display Webcam";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error capturing snapshot: " + ex.Message);
                }
                return;
            }

            if (!isVideo)
            {
                pictureBox1.Image = null;
            }

            webcam = DeviceManager.GetAllDevices();
            if (webcam.Length > 0)
            {
                isVideo = true;
                device = webcam[0];
                device.ShowWindow(pictureBox1);
                pictureBox1.Refresh();

                button6.Text = "Picture";
            }
            else
            {
                MessageBox.Show("No webcam found");
            }

        }
        public static bool Smooth(Bitmap b, int nWeight)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = nWeight;
            m.Factor = nWeight + 8;
            return BitmapFilter.Conv3x3(b, m);
        }
        private void smoothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) { return; }
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            if (Smooth(originalImage, 1))
                pictureBox2.Image = originalImage;
            else
                MessageBox.Show($"image Processing unsuccessfull");

        }
        public static bool Gaussian_Blur(Bitmap b, int nWeight)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 2;
            m.Pixel = nWeight; // 4
            m.Factor = 16;
            return BitmapFilter.Conv3x3(b, m);
        }
        private void gaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) { return; }
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            if (Gaussian_Blur(originalImage, 4))
                pictureBox2.Image = originalImage;
            else
                MessageBox.Show($"image Processing unsuccessfull");
        }
        public static bool Sharpen(Bitmap b, int nWeight)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(0);
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = -2;
            m.Pixel = nWeight; // 11
            m.Factor = 3;
            return BitmapFilter.Conv3x3(b, m);
        }
        private void sharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) { return; }
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            if (Sharpen(originalImage, 11))
                pictureBox2.Image = originalImage;
            else
                MessageBox.Show($"image Processing unsuccessfull");
        }

        public static bool Mean_Removal(Bitmap b, int nWeight)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(-1);
            m.Pixel = nWeight; // 9
            m.Factor = 1;
            return BitmapFilter.Conv3x3(b, m);
        }
        private void meanRemovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) { return; }
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            if (Mean_Removal(originalImage, 9))
                pictureBox2.Image = originalImage;
            else
                MessageBox.Show($"image Processing unsuccessfull");
        }

        public static bool Emboss_Laplascian(Bitmap b, int nWeight)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(-1);
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 0;
            m.Pixel = nWeight; // 4
            m.Factor = 1;
            m.Offset = 127;
            return BitmapFilter.Conv3x3(b, m);
        }
        private void embossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) { return; }
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            if (Emboss_Laplascian(originalImage, 4))
                pictureBox2.Image = originalImage;
            else
                MessageBox.Show($"image Processing unsuccessfull");
        }

        public static bool Horz_Vertical(Bitmap b, int nWeight)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(0);
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = -1;
            m.Pixel = nWeight; // 4
            m.Factor = 1;
            m.Offset = 127;
            return BitmapFilter.Conv3x3(b, m);
        }
        private void horzVertToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) { return; }
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            if (Horz_Vertical(originalImage, 4))
                pictureBox2.Image = originalImage;
            else
                MessageBox.Show($"image Processing unsuccessfull");
        }

        public static bool All_Directions(Bitmap b, int nWeight)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(-1);
            m.Pixel = nWeight; // 8
            m.Factor = 1;
            m.Offset = 127;
            return BitmapFilter.Conv3x3(b, m);
        }
        private void allDirectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) { return; }
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            if (All_Directions(originalImage, 8))
                pictureBox2.Image = originalImage;
            else
                MessageBox.Show($"image Processing unsuccessfull");
        }

        public static bool Lossy(Bitmap b, int nWeight)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.TopMid = m.MidLeft = m.MidRight = m.BottomLeft = m.BottomRight = -2;
            m.Pixel = nWeight; // 4
            m.Factor = 1;
            m.Offset = 127;
            return BitmapFilter.Conv3x3(b, m);
        }

        private void lossyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) { return; }
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            if (Lossy(originalImage, 4))
                pictureBox2.Image = originalImage;
            else
                MessageBox.Show($"image Processing unsuccessfull");
        }

        public static bool Horizontal(Bitmap b, int nWeight)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(0);
            m.MidLeft = m.MidRight = -1;
            m.Pixel = nWeight; // 2
            m.Factor = 1;
            m.Offset = 127;
            return BitmapFilter.Conv3x3(b, m);
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) { return; }
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            if (Horizontal(originalImage, 2))
                pictureBox2.Image = originalImage;
            else
                MessageBox.Show($"image Processing unsuccessfull");
        }

        public static bool Vertical(Bitmap b, int nWeight)
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(0);
            m.TopMid = -1;
            m.BottomMid = -1;
            m.Pixel = nWeight; // 0
            m.Factor = 1;
            m.Offset = 127;
            return BitmapFilter.Conv3x3(b, m);
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) { return; }
            Bitmap originalImage = new Bitmap(pictureBox1.Image);
            if (Vertical(originalImage, 0))
                pictureBox2.Image = originalImage;
            else
                MessageBox.Show($"image Processing unsuccessfull");
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void horzVertToolStripMenuItem_Click(object sender, EventArgs e) { }
    }
}
