using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;

namespace InteractiveShortestPathAlgorithms
{
    public static class ColourHelper
    {
        public static SWMColor ToSWMColor(this SDColor color) => SWMColor.FromArgb(color.A, color.R, color.G, color.B);
        public static SDColor ToSDColor(this SWMColor color) => SDColor.FromArgb(color.A, color.R, color.G, color.B);
    }
}
