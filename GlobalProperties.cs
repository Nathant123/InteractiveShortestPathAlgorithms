using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//namespace Aliases for more descriptive code
using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;
using Shapes = System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Shapes;

namespace InteractiveShortestPathAlgorithms
{
    public class GlobalProperties
    {

        public const int POINTHEIGHT = 40;
        public const int POINTWIDTH = 40;

        public const int TOTALPOINTS = POINTHEIGHT * POINTWIDTH;
        public const int NUMPERROW = CanvasProperties.WIDTH / POINTWIDTH;
        public const int NUMPERCOLUMN = CanvasProperties.HEIGHT / POINTWIDTH;

        public static SDColor BLOCKEDCOLOR = SDColor.DarkRed;
        public SolidColorBrush BLOCKEDBRUSH = new SolidColorBrush(BLOCKEDCOLOR.ToSWMColor());

        public static SDColor EMPTYCOLOR = SDColor.White;
        public SolidColorBrush EMPTYBRUSH = new SolidColorBrush(BLOCKEDCOLOR.ToSWMColor());

        public static SDColor STARTNODE = SDColor.LawnGreen;
        public SolidColorBrush STARTBRUSH = new SolidColorBrush(STARTNODE.ToSWMColor());

        public static SDColor ENDNODE = SDColor.Orange;
        public SolidColorBrush ENDBRUSH = new SolidColorBrush(ENDNODE.ToSWMColor());

        public static SDColor PATHNODE = SDColor.LightSkyBlue;
        public SolidColorBrush PATHBRUSH = new SolidColorBrush(PATHNODE.ToSWMColor());

        public struct CanvasProperties
        {
            public const int HEIGHT = 800;
            public const int WIDTH = 1200;

            public const int MARGINLEFT = 0;
            public const int MARGINTOP = 150;
            public const int MARGINRIGHT = 0;
            public const int MARGINBOTTOM = 0;

            public const int UNIFORMBORDERTHICKNESS = 2;

        }
    }
}
