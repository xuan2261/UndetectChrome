using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGmail
{
   public class Helper
    {
        public static int getWidthScreen;
        public static int getHeightScreen;
        public static Point GetPointFromIndexPosition(int indexPos, int maxApp = 18)
        {
            Point location = new Point();
            int widthWindowChrome = (2 * getWidthScreen) / maxApp;
            int totalAppPerLine = maxApp / 2;
            while (indexPos > 17)
            {
                indexPos -= 18;
            }
            if (indexPos <= totalAppPerLine - 1)
            {
                location.Y = 0;
            }
            else if (indexPos < maxApp)
            {
                location.Y = getHeightScreen / 2;
                indexPos -= totalAppPerLine;
            }
            location.X = (indexPos) * (widthWindowChrome);
            return location;
        }
    }
}
