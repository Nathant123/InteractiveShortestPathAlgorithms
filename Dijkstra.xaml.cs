using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

//namespace Aliases for more descriptive code
using Shapes = System.Windows.Shapes;
using System.Windows.Input;
using System;

namespace InteractiveShortestPathAlgorithms
{
    /// <summary>
    /// Interaction logic for Dijkstra.xaml
    /// </summary>
    public partial class Dijkstra : Page
    {
        static Canvas DijkstraCanvas;
        static List<gridPoint> dPoints;
        static List<Shapes.Rectangle> dShapes;

        public Dijkstra()
        {
            InitializeComponent();
            IntializeCanvas();
        }

        public void IntializeCanvas()
        {
            DijkstraCanvas = CanvasHandler.GetNewCanvas("Dijkstra");
            DijkstaGrid.Children.Add(DijkstraCanvas);
            dPoints = PointHandler.GetPoints();
            dShapes = ShapesHandler.GetShapes(dPoints);
            CanvasHandler.ShapesToCanvas(dPoints, dShapes, DijkstraCanvas);
            CanvasHandler.BordersToCanvas(DijkstraCanvas, dPoints);
            CanvasHandler.RedrawCanvas(DijkstraCanvas);
        }

        public static void UpdateCanvas()
        {
            ShapesHandler.UpdateShapes(dPoints, dShapes);
            CanvasHandler.RedrawCanvas(DijkstraCanvas);
        }

        private void btnBackToHomePage_Click(object sender, RoutedEventArgs e)
        {
            {
                var currentWindow = (MainWindow)Window.GetWindow(this); // casted to access eventhandler
                if (currentWindow != null)// if it exists
                    currentWindow.NavigationEventHandler("HomePage");
            }
        }

        private void btnPerform_Algorithm_Click(object sender, RoutedEventArgs e)
        {
            if(PointHandler.containsStartNode(dPoints) == false || PointHandler.containsEndNode(dPoints) == false)
            {
                MessageBox.Show("You must have selected both a Start node and an End node");
                return; 
            }
            // Generate nodes;
            var nodes = NodeHandler.GenerateNodes(dPoints.Count); // pass in count of points to generate correct amount of nodes
            nodes = NodeHandler.dMapPointsToNodes(dPoints, nodes);

            DijkstraAlgorithm(nodes);
            UpdateCanvas();
        }
        private void btnResetGrid_Click(object sender, RoutedEventArgs e)
        {
            dPoints = PointHandler.resetPointsState(dPoints);
            UpdateCanvas();
        }

        public static void CanvasMouseLeftButtonDownClick(object sender, MouseButtonEventArgs e) // sets square to blocked
        {
            // Handle the mouse click here

            Point clickPosition = e.GetPosition(DijkstraCanvas);

            if (PointHandler.containsStartNode(dPoints) == false)
                PointHandler.ChangePointState(dPoints, clickPosition, gridPoint.PointState.StartPoint);
            else if (PointHandler.containsEndNode(dPoints) == false)
                PointHandler.ChangePointState(dPoints, clickPosition, gridPoint.PointState.EndPoint);
            else PointHandler.ChangePointState(dPoints, clickPosition, gridPoint.PointState.Blocked);

            UpdateCanvas();
        }

        public static void CanvasMouseRightButtonDownClick(object sender, MouseButtonEventArgs e)
        {
            // Handle the mouse click here
            Point clickPosition = e.GetPosition(DijkstraCanvas);
            PointHandler.ChangePointState(dPoints, clickPosition, gridPoint.PointState.Empty);
            UpdateCanvas();
        }

        private void DijkstraAlgorithm(List<Node> nodes)
        {
            bool solved = false;

            //unvisited set
            var UnivistedSet = new List<Node>();
            var VisitedSet = new List<Node>();

            foreach (var node in nodes)
            {
                if (node.nodeVisitState == Node.VisitedState.Unvisited)
                {
                    if (node.currentscore == 0)
                        node.currentNode = true;
                    UnivistedSet.Add(node);
                }
            }

            while (solved == false)
            {
                var currentNode = GraphHandler.GetCurrentNode(UnivistedSet);
                VisitedSet.Add(currentNode);
                GraphHandler.RemoveCurrentFromUnivistedSet(UnivistedSet);

                var neighbours = GraphHandler.GetUnvisitedNeighbours(currentNode, UnivistedSet);
                neighbours = NeighbourCheck(currentNode, neighbours);
                
                solved = GraphHandler.CurrentIsEndNode(currentNode);

                if(solved == true)
                {
                    var Path = GraphHandler.generatePathCandidates(VisitedSet);
                    Path = selectPath(Path);

                    NodeHandler.dMapNodesToPoints(dPoints, VisitedSet);
                    UpdateCanvas();
                    MessageBox.Show("it is solved!, drawing path to screen");
                    break;
                }
                GraphHandler.updateUnvisitedSet(UnivistedSet, neighbours);
                
                if(GraphHandler.stillPotentiallySolvable(UnivistedSet) == false)
                {
                    MessageBox.Show("Not solvable");
                    break;
                }
                
                UnivistedSet = GraphHandler.calculateNewCurrentNode(UnivistedSet, VisitedSet);

                // update graphics , will need to change to be more performant of only changing the current node each iteration
                NodeHandler.dMapNodesToPoints(dPoints, VisitedSet);
                UpdateCanvas();
            }
        }

        private static List<Node> NeighbourCheck(Node currentNode, List<Node> neighbours)
        {
            Func<Node, Node, double> CalculateEuclidianDistance = (currentNode, neighbour) => Math.Sqrt(Math.Pow((neighbour.X - currentNode.X), 2) + Math.Pow((neighbour.Y - currentNode.Y), 2));

            for (int i = 0; i < neighbours.Count; i++)
            {
                // if neighbour is blocker, dont calcualte distance.
                if (neighbours[i].nodePointStatus == gridPoint.PointState.Blocked)
                    continue;
                // CALCULATE NEW SCORE IF NOT VISITED, score is calculated through euclidan distance of, X & Y
                if (neighbours[i].nodeVisitState == Node.VisitedState.Unvisited)
                {
                    neighbours[i].currentscore = CalculateEuclidianDistance(currentNode, neighbours[i]) + currentNode.currentscore;
                    neighbours[i].heuristicScore = CalculateEuclidianDistance(currentNode, neighbours[i]);
                    neighbours[i].nodeVisitState = Node.VisitedState.Visited;
                }
            }
            return neighbours;
        }

        private static List<Node> selectPath(List<Node> path)
        {

            bool Completed = false;
            var leftNode = new Node();
            var upNode = new Node();
            var rightNode = new Node();
            var downNode = new Node();

            Func<Node, Node> generateNeighbour = (currentNode) =>
            {
                foreach (Node node in path) // get the neigbouring nodes
                {
                    if (node.X == currentNode.X - GlobalProperties.POINTWIDTH && node.Y == currentNode.Y)
                        leftNode = node;
                    if (node.X == currentNode.X + GlobalProperties.POINTWIDTH && node.Y == currentNode.Y)
                        rightNode = node;
                    if (node.X == currentNode.X && node.Y == currentNode.Y - GlobalProperties.POINTHEIGHT)
                        downNode = node;
                    if (node.X == currentNode.X && node.Y == currentNode.Y + GlobalProperties.POINTHEIGHT)
                        upNode = node;
                }

                var lowestScore = leftNode;
                if (rightNode.currentscore < lowestScore.currentscore)
                    lowestScore = rightNode;
                if (downNode.currentscore < lowestScore.currentscore)
                    lowestScore = downNode;
                if (upNode.currentscore < lowestScore.currentscore)
                    lowestScore = upNode;

                return lowestScore;
            };

            var Path = new List<Node>();
            var currentNode = path[path.Count - 1]; // add end node, end node hsould always be last elemt
            Path.Add(currentNode);

            while (Completed != true)
            {
                currentNode = generateNeighbour(currentNode);
                Path.Add(currentNode);

                if (currentNode.nodePointStatus == gridPoint.PointState.StartPoint)
                    Completed = true;
            }

            foreach (Node node in Path)
            {
                if (node.nodePointStatus != gridPoint.PointState.StartPoint && node.nodePointStatus != gridPoint.PointState.EndPoint)
                {
                    node.nodePointStatus = gridPoint.PointState.Selected;
                }
            }
            return Path;
        }
    }
}
