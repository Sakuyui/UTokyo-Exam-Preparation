﻿using System;
using System.Collections.Generic;
using System.Linq;
using UTokyo.Math;

namespace UTokyo.Geometry
{
    public abstract class Point : Vector<double>
    {
       
    }
    public class Point2D : Vector<double>
    {
        public double X { get => this[0];
            set => this[0] = value; }
        public double Y { get => this[1]; private set => this[1] = value; }

        public Point2D(double x, double y) : base(new[]{x,y})
        {
        }

        public static implicit operator Point2D(Vector<object> vector)
        {
            if(vector.Count > 2)
                throw new ArithmeticException();
            return new Point2D((double)vector[0], (double)vector[1]);
        }
        
        public static double Cross(Point2D p1, Point2D p2)
        {
            return p1.X * p2.Y - p1.Y * p2.X;
        }

        public static double Dot(Point2D p1, Point2D p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        public static bool IsOrthogonal(Point2D p1, Point2D p2, Point2D q1, Point2D q2)
        {
            var v1 = p2 - p1;
            var v2 = q2 - q1;
            return 0.Equals(v1 * v2);
        }
        public static bool IsOrthogonal(Vector<double> v1, Vector<double> v2)
        {
            return 0.Equals(v1 * v2);
        }

        public static Point2D Project(Segment2D seg, Point2D p)
        {
            var baseV = seg.P2 - seg.P1;
            var r = (double)((p - seg.P1) * baseV) /
                    System.Math.Sqrt(baseV.Select(e => (double)e * (double) e).Sum());
            return seg.P1 + baseV * r;
        }
        
        
        public static Point2D Project(Point2D b1, Point2D b2, Point2D p)
        {
            Segment2D s = new Segment2D(b1, b2);
            return Project(s, p);
        }

        public static int Ccw(Vector<double> v1, Vector<double> v2)
        {
            return 1;
        }
    }
    public class Point3D : Vector<double>
    {
        public double X { get => this[0]; private set => this[0] = value; }
        public double Y { get => this[1]; private set => this[1] = value; }
        public double Z { get => this[2]; private set => this[2] = value; }
        public Point3D(double x, double y, double z) : base(new[]{x, y, z})
        {
        }
    }
    
    public class Point4D : Vector<double>
    {
        public double X { get => this[0]; private set => this[0] = value; }
        public double Y { get => this[1]; private set => this[1] = value; }
        public double Z { get => this[2]; private set => this[2] = value; }
        public double W { get => this[3]; private set => this[3] = value; }
        
        public Point4D(double x, double y,double z, double w) : base(new[]{x,y,z,w})
        {
        }
    }

    public class Line2D
    {
        public Point2D P1;
        public Point2D P2;

        public Line2D(Point2D p1, Point2D p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }

    public class Segment2D
    {
        public Point2D P1;
        public Point2D P2;

        public Segment2D(Point2D p1, Point2D p2)
        {
            P1 = p1;
            P2 = p2;
        }
        
        
    }
}