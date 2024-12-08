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

        public ShipSection(XYPosition position)
        {
            Position = position;
            IsDestroyed = false;
        }
    }
}
