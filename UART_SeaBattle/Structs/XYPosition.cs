using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UART_SeaBattle.Structs
{
    public struct XYPosition
    {
        public double X;
        public double Y;

        public XYPosition(double x, double y) 
        {
            X = x; 
            Y = y;
        }
    }
}
