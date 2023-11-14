using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace InteractiveShortestPathAlgorithms
{
    internal class NodeHandler
    {

        public static List<Node> GenerateNodes(int count)
        {
            var newNodes = new List<Node>();
            for (int i = 0; i < count; i++)
            {
                var newNode = new Node();
                newNodes.Add(newNode);
            }
            return newNodes;
        }

        public static List<Node> dMapPointsToNodes(List<gridPoint> points, List<Node> nodes)
        {
            for (int i = 0; i < points.Count; i++)
            {
                nodes[i].nodePointStatus = points[i].GetPointState();
                nodes[i].X = points[i].GetX();
                nodes[i].Y = points[i].GetY();
                if (points[i].GetPointStartPointState())
                {
                    nodes[i].currentscore = 0;
                }

            }
            return nodes;
        }

        public static List<gridPoint> dMapNodesToPoints(List<gridPoint> points, List<Node> nodes) // change values of points that were modified in algorith to the corresponding ones contained in nodess
        {


            foreach (var node in nodes)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    if(node.X == points[i].GetX() && node.Y == points[i].GetY())
                    {
                        Point clickPosition = new Point(points[i].GetX(), points[i].GetY());
                        if (node.GetCurrentNode() == true
                         && node.nodePointStatus != gridPoint.PointState.EndPoint &&
                         node.nodePointStatus != gridPoint.PointState.StartPoint)
                        {
                            points = PointHandler.ChangePointState(points, clickPosition, gridPoint.PointState.Selected);
                        }
                        else
                        {
                            points = PointHandler.ChangePointState(points, clickPosition, points[i].GetPointState());
                        }
                    }
                }
            }
            return points;
        }
    }
}