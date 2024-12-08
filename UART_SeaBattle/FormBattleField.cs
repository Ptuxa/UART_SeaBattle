using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using UART_SeaBattle.Structs;

namespace UART_SeaBattle
{
    public partial class FormBattleField : Form
    {
        private const float ANGLE_ROTATION = 10f; // Шаг поворота
        private const int TABLE_LINE_THICKNESS = 1;
        private const int TABLE_LOCATION_X = 2;
        private const int TABLE_LOCATION_Y = 50;
        private const int TABLE_ROWS_AMOUNT = 7; // Количество строк таблицы
        private const int TABLE_COLS_AMOUNT = 10; // Количество колонок таблицы
        private const int TABLE_CELL_SIZE = 50; // Размер клетки    
        private const int GUN_LOCATION_X = 225;
        private const int GUN_LOCATION_Y = 430;
        private const int PROJECTILE_SIZE = 5;
        private const float PROJECTILE_SPEED = 10f;

        private readonly Color TABLE_CELL_COLOR = Color.LightBlue;
        private readonly Color TABLE_LINE_COLOR = Color.Black;
        private readonly Color PROJECTILE_COLOR = Color.Red;
        private readonly Color SHIP_COLOR = Color.Green;
        private readonly Image originalImage = Image.FromFile("..\\..\\..\\Properties\\Gun.png");

        private ShipCoordinates[] shipsCoordinates = new ShipCoordinates[]
        {
            new ShipCoordinates
            {
                FirstSection = new Point(1, 1),
                LastSection = new Point(3, 1)
            },
            new ShipCoordinates
            {
                FirstSection = new Point(5, 3),
                LastSection = new Point(8, 3)
            },
            new ShipCoordinates
            {
                FirstSection = new Point(0, 6),
                LastSection = new Point(0, 6)
            }
        };

        struct ShipPosition
        {
            public XYPosition LeftTop;
            public XYPosition LeftBottom;
            public XYPosition RightTop;
            public XYPosition RightBottom;

            public ShipPosition(XYPosition leftTop, XYPosition leftBottom, XYPosition rightTop, XYPosition rightBottom)
            {
                LeftTop = leftTop;
                LeftBottom = leftBottom;
                RightTop = rightTop;
                RightBottom = rightBottom;
            }
        }

        private ShipPosition[] shipsPositions;
        //private float currentAngle = 0f;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private PointF projectilePosition;
        private float projectileAngle = 0;
        private bool projectileInFlight = false;
        private float currentAngle = 90f; // Начальная ориентация вверх


        public FormBattleField()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Устранение мерцания
            this.KeyPreview = true; // Для обработки клавиш

            this.Paint += DrawGame;
            this.KeyDown += HandleKeyPress;

            timer.Interval = 30; // Таймер для движения снаряда
            timer.Tick += UpdateProjectilePosition;

            shipsPositions = shipsCoordinates.Select(c => CalculateShipPosition(c)).ToArray();
            this.ClientSize = new Size(TABLE_LOCATION_X + TABLE_CELL_SIZE * TABLE_COLS_AMOUNT + TABLE_LINE_THICKNESS + TABLE_LOCATION_X, TABLE_LOCATION_Y + TABLE_CELL_SIZE * TABLE_ROWS_AMOUNT + TABLE_LINE_THICKNESS + TABLE_LOCATION_Y + 200);
        }

        private void ShootProjectile()
        {
            // Перевод угла в радианы
            float angleRad = (180 - currentAngle) * (float)Math.PI / 180;

            // Координаты центра пушки с учетом смещения
            float centerX = GUN_LOCATION_X + originalImage.Width / 2;
            float centerY = GUN_LOCATION_Y + originalImage.Height / 2;

            // Длина дула (от центра пушки до конца, с учетом смещения дула)
            float barrelLength = originalImage.Height / 2;

            // Вычисление координат конца дула пушки
            float gunTipX = centerX + (float)Math.Cos(angleRad) * barrelLength; // Косинус для горизонтальной оси (X)
            float gunTipY = centerY - (float)Math.Sin(angleRad) * barrelLength; // Синус для вертикальной оси (Y)

            // Угол и начальная позиция снаряда
            projectileAngle = 180 - currentAngle;
            projectilePosition = new PointF(gunTipX, gunTipY);
            projectileInFlight = true;
            timer.Start(); // Запуск таймера для движения снаряда
        }

        private void DrawGun(Graphics g)
        {
            // Сохраняем текущую трансформацию
            Matrix oldTransform = g.Transform;

            // Центр вращения пушки с учётом смещения
            g.TranslateTransform(GUN_LOCATION_X + originalImage.Width / 2,
                                 GUN_LOCATION_Y + originalImage.Height / 2);

            // Поворот пушки на текущий угол
            g.RotateTransform(currentAngle);

            // Перемещение обратно, чтобы нарисовать пушку
            g.TranslateTransform(-originalImage.Width / 2, -originalImage.Height / 2);

            // Рисуем саму пушку
            g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);

            // Восстанавливаем старую трансформацию
            g.Transform = oldTransform;
        }

        private void UpdateProjectilePosition(object? sender, EventArgs e)
        {
            // Переводим угол в радианы
            float angleRad = projectileAngle * (float)Math.PI / 180;

            // Вычисляем движение снаряда с учетом угла
            // Учитываем инвертированную ось Y (в Windows Forms ось Y направлена вниз)
            projectilePosition.X += (float)Math.Cos(angleRad) * PROJECTILE_SPEED; // Движение по оси X
            projectilePosition.Y -= (float)Math.Sin(angleRad) * PROJECTILE_SPEED; // Движение по оси Y

            // Проверка на столкновение с кораблем или выход за границы
            if (CheckCollisionWithShip() || CheckCollisionWithBounds())
            {
                projectileInFlight = false; // Останавливаем движение снаряда
                timer.Stop(); // Останавливаем таймер
            }

            // Перерисовываем форму для обновления положения снаряда
            Invalidate();
        }

        private bool CheckCollisionWithBounds()
        {
            return projectilePosition.X < TABLE_LOCATION_X ||
                   projectilePosition.X > TABLE_LOCATION_X + TABLE_COLS_AMOUNT * TABLE_CELL_SIZE ||
                   projectilePosition.Y < TABLE_LOCATION_Y;
        }

        private bool CheckCollisionWithShip()
        {
            foreach (var ship in shipsPositions)
            {
                if (projectilePosition.X + PROJECTILE_SIZE / 2 >= ship.LeftTop.X &&
                    projectilePosition.X - PROJECTILE_SIZE / 2 <= ship.RightBottom.X &&
                    projectilePosition.Y + PROJECTILE_SIZE / 2 >= ship.LeftTop.Y &&
                    projectilePosition.Y - PROJECTILE_SIZE / 2 <= ship.RightBottom.Y)
                {
                    RemoveShip(ship);
                    return true;
                }
            }
            return false;
        }

        private void RemoveShip(ShipPosition ship)
        {
            shipsPositions = shipsPositions.Where(s => !s.Equals(ship)).ToArray();
        }

        private void DrawGame(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawTable(g);
            DrawShips(g);
            DrawGun(g);
            if (projectileInFlight)
            {
                DrawProjectile(g);
            }
        }

        private void DrawTable(Graphics g)
        {
            using (Brush cellBrush = new SolidBrush(TABLE_CELL_COLOR))
            using (Pen linePen = new Pen(TABLE_LINE_COLOR, TABLE_LINE_THICKNESS))
            {
                for (int row = 0; row < TABLE_ROWS_AMOUNT; row++)
                {
                    for (int col = 0; col < TABLE_COLS_AMOUNT; col++)
                    {
                        int x = TABLE_LOCATION_X + col * TABLE_CELL_SIZE;
                        int y = TABLE_LOCATION_Y + row * TABLE_CELL_SIZE;

                        g.FillRectangle(cellBrush, x, y, TABLE_CELL_SIZE, TABLE_CELL_SIZE);
                        g.DrawRectangle(linePen, x, y, TABLE_CELL_SIZE, TABLE_CELL_SIZE);
                    }
                }
            }
        }

        private void DrawShips(Graphics g)
        {
            using (Brush shipBrush = new SolidBrush(SHIP_COLOR))
            {
                foreach (var ship in shipsPositions)
                {
                    g.FillRectangle(shipBrush,
                        (float)ship.LeftTop.X, (float)ship.LeftTop.Y,
                        (float)(ship.RightBottom.X - ship.LeftTop.X),
                        (float)(ship.RightBottom.Y - ship.LeftTop.Y));
                }
            }
        }

        private void DrawProjectile(Graphics g)
        {
            using (Brush projectileBrush = new SolidBrush(PROJECTILE_COLOR))
            {
                g.FillEllipse(projectileBrush,
                    projectilePosition.X - PROJECTILE_SIZE / 2,
                    projectilePosition.Y - PROJECTILE_SIZE / 2,
                    PROJECTILE_SIZE, PROJECTILE_SIZE);
            }
        }

        private void HandleKeyPress(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                currentAngle -= ANGLE_ROTATION;
            }
            else if (e.KeyCode == Keys.Right)
            {
                currentAngle += ANGLE_ROTATION;
            }
            else if (e.KeyCode == Keys.Space && !projectileInFlight)
            {
                ShootProjectile();
            }

            Invalidate(); // Перерисовка
        }

        private ShipPosition CalculateShipPosition(ShipCoordinates coordinates)
        {
            int left = TABLE_LOCATION_X + coordinates.FirstSection.X * TABLE_CELL_SIZE;
            int top = TABLE_LOCATION_Y + coordinates.FirstSection.Y * TABLE_CELL_SIZE;
            int right = TABLE_LOCATION_X + (coordinates.LastSection.X + 1) * TABLE_CELL_SIZE;
            int bottom = TABLE_LOCATION_Y + (coordinates.LastSection.Y + 1) * TABLE_CELL_SIZE;

            return new ShipPosition(
                new XYPosition(left, top),
                new XYPosition(left, bottom),
                new XYPosition(right, top),
                new XYPosition(right, bottom));
        }
    }
}
