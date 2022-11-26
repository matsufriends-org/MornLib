﻿namespace MornLib.Grid
{
    public readonly struct GridPos
    {
        public int X { get; }
        public int Y { get; }

        public GridPos(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static GridPos operator +(GridPos a, GridPos b)
        {
            return new GridPos(a.X + b.X, a.Y + b.Y);
        }

        public static GridPos operator -(GridPos a, GridPos b)
        {
            return new GridPos(a.X - b.X, a.Y - b.Y);
        }

        public static GridPos operator *(GridPos a, int b)
        {
            return new GridPos(a.X * b, a.Y * b);
        }

        public static GridPos operator /(GridPos a, int b)
        {
            return new GridPos(a.X / b, a.Y / b);
        }
    }
}