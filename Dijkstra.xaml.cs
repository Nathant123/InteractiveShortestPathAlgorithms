using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

//namespace Aliases for more descriptive code
using Shapes = System.Windows.Shapes;
using System.Windows.Input;
using System;

// TO:DO clear screen

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
                if(node.nodeVisitState == Node.VisitedState.Unvisited)
                {
                    if(node.currentscore == 0)
                        node.currentNode = true;
                    UnivistedSet.Add(node);
                }
            }
            
            while(solved == false)
            {
                var currentNode = GetCurrentNode(UnivistedSet);
                VisitedSet.Add(currentNode);
                RemoveCurrentFromUnivistedSet(UnivistedSet);

                var neighbours = GetUnvisitedNeighbours(currentNode, UnivistedSet);
                neighbours = NeighbourCheck(currentNode, neighbours, currentNode.currentscore);
                
                solved = CurrentIsEndNode(currentNode);

                if(solved == true)
                {
                    var Path = generatePathCandidates(VisitedSet);
                    Path = selectPath(Path);

                    NodeHandler.dMapNodesToPoints(dPoints, VisitedSet);
                    UpdateCanvas();
                    MessageBox.Show("it is solved!, drawing path to screen");
                    break;
                }
                updateUnvisitedSet(UnivistedSet, neighbours);
                
                if(stillPotentiallySolvable(UnivistedSet) == false)
                {
                    MessageBox.Show("Not solvable");
                    break;
                }
                
                UnivistedSet = calculateNewCurrentNode(UnivistedSet, VisitedSet);

                // update graphics , will need to change to be more performant of only changing the current node each iteration
                NodeHandler.dMapNodesToPoints(dPoints, VisitedSet);
                UpdateCanvas();
            }
            Console.WriteLine();
        }

        private List<Node> generatePathCandidates(List<Node> visitedSet)
        {
            var Path = new List<Node>();
            foreach(Node node in visitedSet)
            {
                if (node.currentNode == true)
                    Path.Add(node);
            }
            return Path;
        }

        private List<Node> selectPath(List<Node> path)
        {

            bool Completed = false;
            var leftNode = new Node();
            var upNode = new Node();
            var rightNode = new Node();
            var downNode = new Node();

            Func<Node, Node> generateNeighbour = (currentNode) =>
            {
                foreach(Node node in path) // get the neigbouring nodes
                {
                    if(node.X == currentNode.X - GlobalProperties.POINTWIDTH && node.Y == currentNode.Y)
                        leftNode = node;
                    if (node.X == currentNode.X + GlobalProperties.POINTWIDTH && node.Y == currentNode.Y)
                        rightNode = node;
                    if (node.X == currentNode.X && node.Y == currentNode.Y - GlobalProperties.POINTHEIGHT)
                        downNode = node;
                    if (node.X == currentNode.X && node.Y == currentNode.Y + GlobalProperties.POINTHEIGHT)
                        upNode = node;
                }

                var lowestScore = leftNode;
                if(rightNode.currentscore < lowestScore.currentscore)
                    lowestScore = rightNode;
                if(downNode.currentscore < lowestScore.currentscore)
                    lowestScore = downNode;
                if (upNode.currentscore < lowestScore.currentscore)
                    lowestScore = upNode;

                return lowestScore;
            };

            var Path = new List<Node>();
            var currentNode = path[path.Count-1]; // add end node, end node hsould always be last elemt
            Path.Add(currentNode);

            while(Completed!=true)
            {
                currentNode = generateNeighbour(currentNode);
                Path.Add(currentNode);

                if (currentNode.nodePointStatus == gridPoint.PointState.StartPoint)
                    Completed = true;
            }

            foreach(Node node in Path)
            {
                if(node.nodePointStatus != gridPoint.PointState.StartPoint && node.nodePointStatus != gridPoint.PointState.EndPoint)
                {
                    node.nodePointStatus = gridPoint.PointState.Selected;
                }
            }
            return Path;
        }

        private List<Node> calculateNewCurrentNode(List<Node> unvisitedNodes, List<Node> visited)
        {
            var newCurrentCandidates = new List<Node>();
            
            foreach (var node in unvisitedNodes)
            {
                if(node.currentscore<float.MaxValue && visitedContains(node,visited) == false)
                    newCurrentCandidates.Add(node);
            }

            // select new current with lowest score
            var newCurrent = new Node();
            for (int i = 0;i<newCurrentCandidates.Count;i++)
            {
                if (newCurrentCandidates[i].currentscore < newCurrent.currentscore)
                    newCurrent = newCurrentCandidates[i];
            }

            // iterate over unvisted nodes and set the selected nodes currentNode value to TRUE

            foreach (var node in unvisitedNodes)
            {
                if (node.X == newCurrent.X && node.Y == newCurrent.Y) // make sure operating on correct node
                {
                    node.currentNode = true;
                    return unvisitedNodes;
                }
            }

            throw new ArgumentException("should always have a next node");
        }

        private bool visitedContains(Node node,List<Node> visited)
        {
            foreach(Node visitedNode in visited)
            {
                if(visitedNode.X == node.X && visitedNode.Y == node.Y)
                    return true;
            }
            return false;
        }

        private bool stillPotentiallySolvable(List<Node> univistedNodes) // checks to see if the entire unvisited nodes set has score of infinity
        {
            var infinityCount = 0;
            foreach (var node in univistedNodes)
            {
                if (node.currentscore == float.MaxValue)
                    infinityCount++;
            }
            return infinityCount < univistedNodes.Count ? true : false;
        }

        private List<Node> updateUnvisitedSet(List<Node> nodes,List<Node> neighbours)
        {
            foreach (var node in nodes)
            {
                foreach (var neighbor in neighbours)
                {
                    if (node.X == neighbor.X && node.Y == neighbor.Y)
                    {
                        node.currentscore = neighbor.currentscore;
                        break;
                    }
                }
            }
            return nodes;
        }
        private bool CurrentIsEndNode(Node currentNode)
        {
            return (currentNode.nodePointStatus == gridPoint.PointState.EndPoint) ? true : false;
        }
        private List<Node> NeighbourCheck(Node currentNode, List<Node> neighbours,double currentNodeCurrentScore)
        {
            Func<Node,Node,double> CalculateEuclidianDistance = (currentNode, neighbour) => Math.Sqrt(Math.Pow((neighbour.X - currentNode.X), 2) + Math.Pow((neighbour.Y - currentNode.Y), 2));

            for (int i = 0; i < neighbours.Count; i++)
            {
                // if neighbour is blocker, dont calcualte distance.
                if (neighbours[i].nodePointStatus == gridPoint.PointState.Blocked)
                    continue;
                // CALCULATE NEW SCORE IF NOT VISITED, score is calculated through euclidan distance of, X & Y
                if (neighbours[i].nodeVisitState == Node.VisitedState.Unvisited)
                {
                    neighbours[i].currentscore = CalculateEuclidianDistance(currentNode, neighbours[i]) + currentNodeCurrentScore;
                    neighbours[i].nodeVisitState = Node.VisitedState.Visited;
                }
            }
            return neighbours;
        }
        
        private List<Node> RemoveCurrentFromUnivistedSet(List<Node> nodes)
        {
            foreach(var node in nodes)
            {
                if(node.currentNode == true)
                {
                    nodes.Remove(node);
                    return nodes;
                }
            }
            return nodes;
        }
        private Node GetCurrentNode(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.currentNode == true)
                {
                    return node;
                }
            }
            throw new ArgumentException("No Current Node");
        }

        private List<Node> GetUnvisitedNeighbours(Node current,List<Node> nodes)
        {
            var neighbours = new List<Node>();
            foreach (var node in nodes)
            {
                // finding the neighbours of the current node and not adding blocked ones
                if (node.X == current.X-GlobalProperties.POINTWIDTH && node.Y == current.Y && neighbours.Count<4 
                    && node.nodePointStatus != gridPoint.PointState.Blocked)
                   neighbours.Add(node);
                else if(node.X==current.X+GlobalProperties.POINTWIDTH && node.Y == current.Y
                    && node.nodePointStatus != gridPoint.PointState.Blocked)
                    neighbours.Add(node);
                else if(node.Y == current.Y-GlobalProperties.POINTHEIGHT && node.X == current.X
                    && node.nodePointStatus != gridPoint.PointState.Blocked)
                    neighbours.Add(node);
                else if (node.Y == current.Y + GlobalProperties.POINTHEIGHT && node.X == current.X
                    && node.nodePointStatus != gridPoint.PointState.Blocked)
                    neighbours.Add(node);
            }
            return neighbours;
        }
    }
}
