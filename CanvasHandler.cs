using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using Shapes = System.Windows.Shapes;

//namespace Aliases for more descriptive code
using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;
using System.Windows.Media;

namespace InteractiveShortestPathAlgorithms
{
    internal class CanvasHandler
    {
        public static Canvas GetNewCanvas(string name)
        {
            return InitalizeCanvas(name);
        }



        private static Canvas InitalizeCanvas(string name)
        {
            var newCanvas = new Canvas();
            newCanvas.Name = name;
            newCanvas.Height = GlobalProperties.CanvasProperties.HEIGHT;
            newCanvas.Width = GlobalProperties.CanvasProperties.WIDTH;
            newCanvas.Margin = new Thickness(GlobalProperties.CanvasProperties.MARGINLEFT, GlobalProperties.CanvasProperties.MARGINTOP,
            GlobalProperties.CanvasProperties.MARGINRIGHT, GlobalProperties.CanvasProperties.MARGINBOTTOM);
            newCanvas.HorizontalAlignment = HorizontalAlignment.Center;
            newCanvas.VerticalAlignment = VerticalAlignment.Top;
            // attach the correct event handler
            switch(name)
            {
                case "Dijkstra":
                    newCanvas.MouseLeftButtonDown += Dijkstra.CanvasMouseLeftButtonDownClick;
                    newCanvas.MouseRightButtonDown += Dijkstra.CanvasMouseRightButtonDownClick;
                    break;
                case "AStar":
                    newCanvas.MouseLeftButtonDown += AStar.CanvasMouseLeftButtonDownClick;
                    newCanvas.MouseRightButtonDown += AStar.CanvasMouseRightButtonDownClick;
                    break;
            }

            return newCanvas;
        }

        public static void RedrawCanvas(Canvas canvas)
        {
            canvas.UpdateLayout();
        }
        public static Canvas BordersToCanvas(Canvas canvas, List<gridPoint> points)
        {
            
            var borderThickness = new Thickness(GlobalProperties.CanvasProperties.UNIFORMBORDERTHICKNESS);
            var brush = new SolidColorBrush();
            brush.Color = ColourHelper.ToSWMColor(SDColor.FromArgb(255, 0, 0, 0));

            for (int i = 0; i < points.Count; i++)
            {
                Border border = new Border();
                border.BorderThickness = borderThickness;
                border.BorderBrush = brush;
                border.Width = GlobalProperties.POINTWIDTH;
                border.Height = GlobalProperties.POINTWIDTH;
                Canvas.SetLeft(border, points[i].GetX());
                Canvas.SetTop(border, points[i].GetY());
                canvas.Children.Add(border);
            }
            return canvas;
        }
        public static Canvas ShapesToCanvas(List<gridPoint> points, List<Shapes.Rectangle> shapes,Canvas canvas)
        {
            for (int i = 0;i < points.Count; i++)
            {
                Canvas.SetLeft(shapes[i], points[i].GetX());
                Canvas.SetTop(shapes[i], points[i].GetY());
                canvas.Children.Add(shapes[i]);
            }
            return canvas;
        }
    }
}
