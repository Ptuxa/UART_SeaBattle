using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UART_SeaBattle.Structs
{
    public struct ShipSection
    {
        public XYPosition Position;
        public bool IsDestroyed;
        public int Speed;

        public ShipSection(XYPosition position, int speed)
        {
            Position = position;
            IsDestroyed = false;
            Speed = speed;
        }

        public void Move()
        {
            Position.X += Speed; // Перемещаем секцию корабля
        }
    }
}
