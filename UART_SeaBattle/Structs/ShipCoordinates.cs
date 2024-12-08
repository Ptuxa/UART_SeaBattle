using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UART_SeaBattle.Structs
{
    public struct ShipCoordinates
    {
        public Point FirstSection;
        public Point LastSection;

        public ShipCoordinates(Point firstSection, Point lastSection)
        {
            FirstSection = firstSection;
            LastSection = lastSection;
        }
    }
}
