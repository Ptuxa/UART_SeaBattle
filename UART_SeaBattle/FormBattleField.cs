using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using UART_SeaBattle.HandlerClasses;
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
            new ShipCoordinates { FirstSection = new Point(1, 1), LastSection = new Point(3, 1) },
            new ShipCoordinates { FirstSection = new Point(5, 3), LastSection = new Point(8, 3) },
            new ShipCoordinates { FirstSection = new Point(0, 6), LastSection = new Point(0, 6) }
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

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private PointF projectilePosition;
        private float projectileAngle = 0;
        private bool projectileInFlight = false;
        private float currentAngle = 90f; // Начальная ориентация вверх
        private List<ShipSection[]> shipsSections; // Используем список секций
        private SerialPortHandler serialHandler;

        private SerialPortHandler InitializeSerialHandler()
        {
            SerialPortHandler serialHandler = new SerialPortHandler("COM3", 115200); // Укажите правильный порт
            serialHandler.OnCommandReceived = HandleCommand;

            return serialHandler;
        }

        private void HandleCommand(byte[] bytes)
        {
            if (bytes.Length == 3 && bytes[0] == 0xAA && bytes[1] == 0xBB)
            {                
                switch (bytes[2])
                {
                    case 0x1:
                        currentAngle -= ANGLE_ROTATION;
                        break;
                    case 0x2:
                        currentAngle += ANGLE_ROTATION;
                        break;
                    case 0x3:
                        if (!projectileInFlight) ShootProjectile();
                        break;
                }

                Invalidate(); // Перерисовка
            }            
        }

        public FormBattleField()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Устранение мерцания
            this.KeyPreview = true; // Для обработки клавиш

            this.Paint += DrawGame;
            this.KeyDown += HandleKeyPress;

            timer.Interval = 30; // Таймер для движения снаряда
            timer.Tick += UpdateProjectilePosition;

            shipsSections = InitializeShips(); // Инициализируем корабли и их секции

            this.ClientSize = new Size(TABLE_LOCATION_X + TABLE_CELL_SIZE * TABLE_COLS_AMOUNT + TABLE_LINE_THICKNESS + TABLE_LOCATION_X,
                                       TABLE_LOCATION_Y + TABLE_CELL_SIZE * TABLE_ROWS_AMOUNT + TABLE_LINE_THICKNESS + TABLE_LOCATION_Y + 200);

            serialHandler = InitializeSerialHandler();
        }

        private List<ShipSection[]> InitializeShips()
        {
            var sections = new List<ShipSection[]>();

            foreach (var shipCoordinates in shipsCoordinates)
            {
                sections.Add(GetShipSections(shipCoordinates));
            }

            return sections;
        }

        private bool CheckCollisionWithShip()
        {
            for (int i = 0; i < shipsSections.Count; i++)
            {
                

                for (int j = 0; j < shipsSections[i].Length; j++)
                {
                    ShipSection shipSection = shipsSections[i][j];

                    if (projectilePosition.X + PROJECTILE_SIZE / 2 >= shipSection.Position.X &&
                        projectilePosition.X - PROJECTILE_SIZE / 2 <= shipSection.Position.X + TABLE_CELL_SIZE &&
                        projectilePosition.Y + PROJECTILE_SIZE / 2 >= shipSection.Position.Y &&
                        projectilePosition.Y - PROJECTILE_SIZE / 2 <= shipSection.Position.Y + TABLE_CELL_SIZE)
                    {
                        shipsSections[i][j].IsDestroyed = true;

                        RemoveShip(shipsSections[i]);

                        return true;
                    }
                }
            }
            return false;
        }

        private void RemoveShip(ShipSection[] shipSections)
        {
            // Удаляем корабль, если все его секции уничтожены
            if (shipSections.All(section => section.IsDestroyed))
            {
                this.shipsSections.Remove(shipSections); // Удаляем корабль из списка
            }
        }

        private void ShootProjectile()
        {
            float angleRad = (180 - currentAngle) * (float)Math.PI / 180;
            float centerX = GUN_LOCATION_X + originalImage.Width / 2;
            float centerY = GUN_LOCATION_Y + originalImage.Height / 2;
            float barrelLength = originalImage.Height / 2;

            float gunTipX = centerX + (float)Math.Cos(angleRad) * barrelLength;
            float gunTipY = centerY - (float)Math.Sin(angleRad) * barrelLength;

            projectileAngle = 180 - currentAngle;
            projectilePosition = new PointF(gunTipX, gunTipY);
            projectileInFlight = true;
            timer.Start();
        }

        private void DrawGun(Graphics g)
        {
            Matrix oldTransform = g.Transform;
            g.TranslateTransform(GUN_LOCATION_X + originalImage.Width / 2, GUN_LOCATION_Y + originalImage.Height / 2);
            g.RotateTransform(currentAngle);
            g.TranslateTransform(-originalImage.Width / 2, -originalImage.Height / 2);
            g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);
            g.Transform = oldTransform;
        }

        private void UpdateProjectilePosition(object? sender, EventArgs e)
        {
            float angleRad = projectileAngle * (float)Math.PI / 180;
            projectilePosition.X += (float)Math.Cos(angleRad) * PROJECTILE_SPEED;
            projectilePosition.Y -= (float)Math.Sin(angleRad) * PROJECTILE_SPEED;

            if (CheckCollisionWithShip() || CheckCollisionWithBounds())
            {
                projectileInFlight = false;
                timer.Stop();
            }

            Invalidate();
        }

        private bool CheckCollisionWithBounds()
        {
            return projectilePosition.X < TABLE_LOCATION_X || projectilePosition.X > TABLE_LOCATION_X + TABLE_COLS_AMOUNT * TABLE_CELL_SIZE ||
                   projectilePosition.Y < TABLE_LOCATION_Y;
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
            using (Brush destroyedBrush = new SolidBrush(PROJECTILE_COLOR))
            {
                foreach (var shipSections in shipsSections)
                {
                    foreach (var section in shipSections)
                    {
                        Brush currentBrush = section.IsDestroyed ? destroyedBrush : shipBrush;
                        g.FillRectangle(currentBrush,
                            (float) section.Position.X, (float) section.Position.Y,
                            TABLE_CELL_SIZE, TABLE_CELL_SIZE);
                    }
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

        private ShipSection[] GetShipSections(ShipCoordinates coordinates)
        {
            var sections = new List<ShipSection>();

            for (int x = coordinates.FirstSection.X; x <= coordinates.LastSection.X; x++)
            {
                for (int y = coordinates.FirstSection.Y; y <= coordinates.LastSection.Y; y++)
                {
                    sections.Add(new ShipSection(new XYPosition(
                        TABLE_LOCATION_X + x * TABLE_CELL_SIZE,
                        TABLE_LOCATION_Y + y * TABLE_CELL_SIZE
                    )));
                }
            }

            return sections.ToArray();
        }
    }
}
