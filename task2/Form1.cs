using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;
        private Bitmap grayscaleImage1;
        private Bitmap grayscaleImage2;
        private Bitmap grayscaleImage3;

        public Form1()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // �������� ��������� �����������
                originalImage = new Bitmap(openFileDialog.FileName);

                originalPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                // ����������� ��������� �����������
                originalPictureBox.Image = originalImage;

                // �������������� ����������� � ������� ������
                ConvertToGrayscale();

                // ����������� ��������������� �����������
                grayscalePictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                grayscalePictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBoxDiff.SizeMode = PictureBoxSizeMode.Zoom;
                grayscalePictureBox1.Image = grayscaleImage1;
                grayscalePictureBox2.Image = grayscaleImage2;
                pictureBoxDiff.Image = grayscaleImage3;

                // ���������� ���������� �������������
                PlotIntensityHistograms();
            }
        }

        private void ConvertToGrayscale()
        {
            // �������� Bitmap-�������� ��� �������� ����������� �����������
            grayscaleImage1 = new Bitmap(originalImage.Width, originalImage.Height);
            grayscaleImage2 = new Bitmap(originalImage.Width, originalImage.Height);
            grayscaleImage3 = new Bitmap(originalImage.Width, originalImage.Height);

            // ����� �������� ��������� �����������
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    Color originalColor = originalImage.GetPixel(x, y);

                    // ���������� ������������� �� ������ ������� �� �����������
                    int intensity1 = (int)((0.299 * originalColor.R) + (0.587 * originalColor.G) + (0.114 * originalColor.B));

                    // ���������� ������������� �� ������ ������� �� �����������
                    int intensity2 = (originalColor.R + originalColor.G + originalColor.B) / 3;

                    // �������� ������ ����� � ���������� ��������������
                    Color grayscaleColor1 = Color.FromArgb(intensity1, intensity1, intensity1);
                    Color grayscaleColor2 = Color.FromArgb(intensity2, intensity2, intensity2);
                    Color diff = Color.FromArgb(Math.Abs(intensity2-intensity1), Math.Abs(intensity2 - intensity1), Math.Abs(intensity2-intensity1));
                    // ������ ����������� �������� � ��������������� �����������
                    grayscaleImage1.SetPixel(x, y, grayscaleColor1);
                    grayscaleImage2.SetPixel(x, y, grayscaleColor2);
                    grayscaleImage3.SetPixel(x, y, diff);
                }
            }
        }

        private void PlotIntensityHistograms()
        {
            // �������� ���������� ��� ����� ����������� �����������
            int[] histogram1 = new int[256];
            int[] histogram2 = new int[256];

            // ����� �������� ����������� ����������� � ������� �������� �������������
            for (int y = 0; y < grayscaleImage1.Height; y++)
            {
                for (int x = 0; x < grayscaleImage1.Width; x++)
                {
                    Color grayscaleColor1 = grayscaleImage1.GetPixel(x, y);
                    Color grayscaleColor2 = grayscaleImage2.GetPixel(x, y);

                    histogram1[grayscaleColor1.R]++;
                    histogram2[grayscaleColor2.R]++;
                }
            }

            intensityChart.Series[0].Points.Clear();
            intensityChart.Series[1].Points.Clear();

            for (int i = 0; i < 256; i++)
            {
                intensityChart.Series[0].Points.AddXY(i, histogram1[i]);
                intensityChart.Series[1].Points.AddXY(i, histogram2[i]);
            }
        }
    }
}
