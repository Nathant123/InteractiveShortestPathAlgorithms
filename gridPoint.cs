using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDColor = System.Drawing.Color;
using SWMColor = System.Windows.Media.Color;

namespace InteractiveShortestPathAlgorithms
{
    internal struct PointColor
    {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public byte Alpha { get; }
    }

    internal class gridPoint
    {
        [Flags]
        public enum PointState // empty = 0x00000000, selected = 0x00001111, discared = 0x11110000, blocked = 0x11111111, st
        {
            Empty = 0x00000000,
            StartPoint = 0x00000011,
            EndPoint = 0x00000111,
            Selected = 0x00001111,
            Discarded = 0x11110000,
            Blocked = 0x11111111
        }

        private PointState _state = PointState.Empty;
        private Color _PointColor { get; set; }
        private int X { get; }
        private int Y { get; }
        private bool isStart = false;
        private bool isEnd = false;

        public gridPoint(int x, int y, SWMColor color, PointState state = PointState.Empty)
        {
            _PointColor = ColourHelper.ToSDColor(color);
            X = x;
            Y = y;
            _state = state;
        }
        public gridPoint(int x, int y, SDColor color, PointState state = PointState.Empty)
        {
            _PointColor = color;
            X = x;
            Y = y;
            _state = state;
        }
        public void updateColor(SWMColor color) => _PointColor = ColourHelper.ToSDColor(color);
        public void updateColor(SDColor color) => _PointColor = color;

        public bool GetPointStartPointState() => isStart;
        public bool GetPointEndPointState() => isEnd;
        public int GetX() => X;
        public int GetY() => Y;
        public Color GetColor() => _PointColor;
        public PointState GetPointState() => _state;
        public void SetPointState(PointState newState)
        {
            if (_state == newState)
                return;

            if (newState == PointState.StartPoint)
                isStart = true;
            else if (newState == PointState.EndPoint)
                isEnd = true;
            else
            { 
                isStart = false; isEnd = false; 
            }
            
            _state = newState;
        }     
    }
}
