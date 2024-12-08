using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UART_SeaBattle
{
    public partial class FormBattleField : Form
    {
        private Image originalImage; // �������� �����������
        private float currentAngle = 90f; // ������� ���� ��������
        private const float ANGLE_ROTATION = 10f; // ��� ��������

        public FormBattleField()
        {
            InitializeComponent();
            this.KeyPreview = true; // ������������ ������� �� ������ �����

            // �������� ����������� � ��������
            originalImage = Image.FromFile("..\\..\\..\\Properties\\Gun.png"); // ������� ���� � �����������

            pctrbxGun.Image = (Image)originalImage.Clone(); // ������������� ����� �����

            this.RotateImage(currentAngle);
        }

        private void FormBattleField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                currentAngle += ANGLE_ROTATION; // ����������� ���� ��� �������� ������
            }
            else if (e.KeyCode == Keys.Left)
            {
                currentAngle -= ANGLE_ROTATION; // ��������� ���� ��� �������� �����
            }

            RotateImage(currentAngle); // ��������� �������
        }

        private void RotateImage(double currentAngle)
        {
            if (originalImage == null) return;

            // ������� ����� Bitmap � ������������ ��������
            Bitmap rotatedBitmap = new Bitmap(originalImage.Width, originalImage.Height);
            rotatedBitmap.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedBitmap))
            {
                // ������������� ������� �������� ���������
                g.Clear(Color.Transparent);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // �������
                g.TranslateTransform(originalImage.Width / 2, originalImage.Height / 2); // ����� �����������
                g.RotateTransform((float) currentAngle); // ������������ �� ������� ����
                g.TranslateTransform(-originalImage.Width / 2, -originalImage.Height / 2); // ������� � �������� ���������

                g.DrawImage(originalImage, new Point(0, 0)); // ������ ��������
            }

            // ��������� PictureBox
            pctrbxGun.Image = rotatedBitmap;
        }
    }
}
