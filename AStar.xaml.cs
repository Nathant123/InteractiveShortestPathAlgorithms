using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

//namespace Aliases for more descriptive code
using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;
using Shapes = System.Windows.Shapes;
using System.Windows.Input;

namespace InteractiveShortestPathAlgorithms
{
    /// <summary>
    /// Interaction logic for AStar.xaml
    /// </summary>
    public partial class AStar : Page
    {

        static Canvas AStarCanvas;
        static List<gridPoint> aPoints;
        static List<Shapes.Rectangle> aShapes;
        int testVal = 0;
        

        public AStar()
        {
            InitializeComponent();
            IntializeCanvas();
        }

        public void IntializeCanvas()
        {
            AStarCanvas = CanvasHandler.GetNewCanvas("AStar");
            AStarGrid.Children.Add(AStarCanvas);
            aPoints = PointHandler.GetPoints();
            aShapes = ShapesHandler.GetShapes(aPoints);
            CanvasHandler.ShapesToCanvas(aPoints, aShapes, AStarCanvas);
            CanvasHandler.BordersToCanvas(AStarCanvas, aPoints);
            CanvasHandler.RedrawCanvas(AStarCanvas);
        }
        
        public static void UpdateCanvas()
        {
            ShapesHandler.UpdateShapes(aPoints, aShapes);
            CanvasHandler.RedrawCanvas(AStarCanvas);
        }

        private void btnBackToHomePage_Click(object sender, RoutedEventArgs e)
        {
                var currentWindow = (MainWindow)Window.GetWindow(this); // casted to access eventhandler
                if (currentWindow != null)// if it exists
                    currentWindow.NavigationEventHandler("HomePage");
        }

        private void btnPerform_Algorithm_Click(object sender, RoutedEventArgs e)
        {
            //aPoints[testVal]._PointColor = SDColor.FromArgb(255, 0, 255, 0);
            //testVal++;
            UpdateCanvas();
        }

        public static void CanvasMouseLeftButtonDownClick(object sender, MouseButtonEventArgs e)
        {
            // Handle the mouse click here
            //Canvas clickedCanvas = (Canvas)sender;

            Point clickPosition = e.GetPosition(AStarCanvas);
            PointHandler.ChangePointState(aPoints, clickPosition, gridPoint.PointState.Blocked);
            UpdateCanvas();
        }
        public static void CanvasMouseRightButtonDownClick(object sender, MouseButtonEventArgs e)
        {
            // Handle the mouse click here
            //Canvas clickedCanvas = (Canvas)sender;

            Point clickPosition = e.GetPosition(AStarCanvas);
            PointHandler.ChangePointState(aPoints, clickPosition, gridPoint.PointState.Empty);
            UpdateCanvas();
        }
    }
}