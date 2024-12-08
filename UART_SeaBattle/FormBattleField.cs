using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UART_SeaBattle
{
    public partial class FormBattleField : Form
    {
        private Image originalImage; // Исходное изображение
        private float currentAngle = 90f; // Текущий угол поворота
        private const float ANGLE_ROTATION = 10f; // Шаг поворота

        public FormBattleField()
        {
            InitializeComponent();
            this.KeyPreview = true; // Обрабатываем клавиши на уровне формы

            // Загрузка изображения в оригинал
            originalImage = Image.FromFile("..\\..\\..\\Properties\\Gun.png"); // Укажите путь к изображению

            pctrbxGun.Image = (Image)originalImage.Clone(); // Устанавливаем копию ориги

            this.RotateImage(currentAngle);
        }

        private void FormBattleField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                currentAngle += ANGLE_ROTATION; // Увеличиваем угол для поворота вправо
            }
            else if (e.KeyCode == Keys.Left)
            {
                currentAngle -= ANGLE_ROTATION; // Уменьшаем угол для поворота влево
            }

            RotateImage(currentAngle); // Выполняем поворот
        }

        private void RotateImage(double currentAngle)
        {
            if (originalImage == null) return;

            // Создаем новый Bitmap с оригинальным размером
            Bitmap rotatedBitmap = new Bitmap(originalImage.Width, originalImage.Height);
            rotatedBitmap.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedBitmap))
            {
                // Устанавливаем высокое качество отрисовки
                g.Clear(Color.Transparent);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Поворот
                g.TranslateTransform(originalImage.Width / 2, originalImage.Height / 2); // Центр изображения
                g.RotateTransform((float) currentAngle); // Поворачиваем на текущий угол
                g.TranslateTransform(-originalImage.Width / 2, -originalImage.Height / 2); // Возврат в исходное положение

                g.DrawImage(originalImage, new Point(0, 0)); // Рисуем оригинал
            }

            // Обновляем PictureBox
            pctrbxGun.Image = rotatedBitmap;
        }
    }
}
