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
    internal class ShapesHandler
    {
        private static SolidColorBrush brush;
        public static List<Shapes.Rectangle> GetShapes(List<gridPoint> points)// default point list, with default values
        {
            List<Shapes.Rectangle> shapes = new List<Shapes.Rectangle>();
            foreach (var point in points)
            {
                brush = new SolidColorBrush(ColourHelper.ToSWMColor(point.GetColor()));
                var newRectangle = new Shapes.Rectangle();
                newRectangle.Width = GlobalProperties.POINTWIDTH;
                newRectangle.Height = GlobalProperties.POINTWIDTH;
                newRectangle.Fill = brush;
                shapes.Add(newRectangle);
            }
            return shapes;
        }

        public static List<Shapes.Rectangle> UpdateShapes(List<gridPoint> points, List<Shapes.Rectangle> shapes)
        {
            List<Shapes.Rectangle> updatedShapes = new List<Shapes.Rectangle>();
            for (int i = 0; i < shapes.Count;i++)
            {
                brush = new SolidColorBrush(ColourHelper.ToSWMColor(points[i].GetColor()));
                shapes[i].Fill = brush;
                updatedShapes.Add(shapes[i]);
            }
            return updatedShapes;
        }
    }
}
