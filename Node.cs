using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveShortestPathAlgorithms
{
    internal class Node
    {
        [Flags]
        public enum VisitedState // visited = 0x00000000, selected = 0x00001111, discared = 0x11110000, blocked = 0x11111111
        {
            Visited = 0x00000000,
            Unvisited = 0x11111111
        }

        public int X; // for mapping to points/shapes lists purposes
        public int Y; // for mapping to points/shapes lists purposes
        public double currentscore = float.MaxValue; // defaults to float.max to represent infinity 
        public bool currentNode;

        public VisitedState nodeVisitState;
        public gridPoint.PointState nodePointStatus; // get variable that will store what the node is (used so algorithm doesnt try to path through blocked)


        public Node()
        {
            nodeVisitState = VisitedState.Unvisited;
        }

        public bool GetCurrentNode() => currentNode;


    }
}
