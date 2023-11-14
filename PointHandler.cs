using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//namespace Aliases for more descriptive code
using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;
using Shapes = System.Windows.Shapes;
using static InteractiveShortestPathAlgorithms.gridPoint;

namespace InteractiveShortestPathAlgorithms
{
    internal class PointHandler
    {
        public PointHandler() 
        {
            
        }

        public static List<gridPoint> GetPoints()// default point list, with default values
        {
            List<gridPoint> points = new List<gridPoint>();
            // CREATES COLUMN BY COLUMN LEFT TO RIGHT
            for (int i = 0;i < GlobalProperties.NUMPERROW; i++)
            {
                for(int j = 0;j < GlobalProperties.NUMPERCOLUMN; j++)
                {
                    var newPoint = new gridPoint(i * GlobalProperties.POINTWIDTH, j * GlobalProperties.POINTHEIGHT,GlobalProperties.EMPTYCOLOR);
                    points.Add(newPoint);
                }
            }
            return points;
        }

        public static List<gridPoint> ChangePointState(List<gridPoint>points,Point point, PointState desiredState) //see point
        {
            foreach(gridPoint p in points)
            {
                if (p.GetX() > point.X - GlobalProperties.POINTWIDTH && p.GetX() < p.GetX() + GlobalProperties.POINTWIDTH &&
                    p.GetY() > point.Y - GlobalProperties.POINTHEIGHT && p.GetY() < p.GetY() + GlobalProperties.POINTHEIGHT) //Idk why i need to -the width/height in checks but it works
                {
                    // check if current state == desired state to reduce operations needed and speee dup
                    p.SetPointState(desiredState);
                    
                    switch(desiredState)
                    {
                        case PointState.Empty:
                            p.updateColor(GlobalProperties.EMPTYCOLOR);
                        break;
                        case PointState.Blocked:
                            p.updateColor(GlobalProperties.BLOCKEDCOLOR); 
                        break;
                        case PointState.StartPoint:
                            p.updateColor(GlobalProperties.STARTNODE);
                            break;
                        case PointState.EndPoint:
                            p.updateColor(GlobalProperties.ENDNODE);
                            break;
                        case PointState.Selected:
                            p.updateColor(GlobalProperties.PATHNODE);
                            break;
                    }
                    break;
                }
            }
            return points;
        }

        public static bool containsStartNode(List<gridPoint> points)
        {
            foreach(gridPoint p in points)
            {
                if (p.GetPointStartPointState())
                    return true;
            }
            return false;
        }

        public static bool containsEndNode(List<gridPoint> points)
        {
            foreach (gridPoint p in points)
            {
                if (p.GetPointEndPointState())
                    return true;
            }
            return false;
        }

    }
}
