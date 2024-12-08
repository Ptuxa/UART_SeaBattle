using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UART_SeaBattle.Structs
{
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
}
