using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveShortestPathAlgorithms
{
    internal class GraphHandler
    {

        

        public static List<Node> generatePathCandidates(List<Node> visitedSet)
        {
            var Path = new List<Node>();
            foreach (Node node in visitedSet)
            {
                if (node.currentNode == true)
                    Path.Add(node);
            }
            return Path;
        }

        public static List<Node> calculateNewCurrentNode(List<Node> unvisitedNodes, List<Node> visited)
        {
            var newCurrentCandidates = new List<Node>();

            foreach (var node in unvisitedNodes)
            {
                if (node.currentscore < float.MaxValue && visitedContains(node, visited) == false)
                    newCurrentCandidates.Add(node);
            }

            // select new current with lowest score
            var newCurrent = new Node();
            for (int i = 0; i < newCurrentCandidates.Count; i++)
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

        public static bool visitedContains(Node node, List<Node> visited)
        {
            foreach (Node visitedNode in visited)
            {
                if (visitedNode.X == node.X && visitedNode.Y == node.Y)
                    return true;
            }
            return false;
        }

        public static bool stillPotentiallySolvable(List<Node> univistedNodes) // checks to see if the entire unvisited nodes set has score of infinity
        {
            var infinityCount = 0;
            foreach (var node in univistedNodes)
            {
                if (node.currentscore == float.MaxValue)
                    infinityCount++;
            }
            return infinityCount < univistedNodes.Count ? true : false;
        }

        public static List<Node> updateUnvisitedSet(List<Node> nodes, List<Node> neighbours)
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
        public static bool CurrentIsEndNode(Node currentNode) => currentNode.nodePointStatus == gridPoint.PointState.EndPoint ? true : false;



        public static List<Node> RemoveCurrentFromUnivistedSet(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.currentNode == true)
                {
                    nodes.Remove(node);
                    return nodes;
                }
            }
            return nodes;
        }
        public static Node GetCurrentNode(List<Node> nodes)
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

        public static List<Node> GetUnvisitedNeighbours(Node current, List<Node> nodes)
        {
            var neighbours = new List<Node>();
            foreach (var node in nodes)
            {
                // finding the neighbours of the current node and not adding blocked ones
                if (node.X == current.X - GlobalProperties.POINTWIDTH && node.Y == current.Y && neighbours.Count < 4
                    && node.nodePointStatus != gridPoint.PointState.Blocked)
                    neighbours.Add(node);
                else if (node.X == current.X + GlobalProperties.POINTWIDTH && node.Y == current.Y
                    && node.nodePointStatus != gridPoint.PointState.Blocked)
                    neighbours.Add(node);
                else if (node.Y == current.Y - GlobalProperties.POINTHEIGHT && node.X == current.X
                    && node.nodePointStatus != gridPoint.PointState.Blocked)
                    neighbours.Add(node);
                else if (node.Y == current.Y + GlobalProperties.POINTHEIGHT && node.X == current.X
                    && node.nodePointStatus != gridPoint.PointState.Blocked)
                    neighbours.Add(node);
            }
            return neighbours;
        }
        private static double CalculateEuclidianDistance(Node currentNode, Node otherNode) => Math.Sqrt(Math.Pow((otherNode.X - currentNode.X), 2) + Math.Pow((otherNode.Y - currentNode.Y), 2));

        public static double CalculateHeuristic(Node current, Node EndNode) => CalculateEuclidianDistance(current, EndNode);

    }
}
