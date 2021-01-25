using TokyoU.Math;

namespace TokyoU.Geometry
{
    public abstract class Point : Vector<double>
    {
        
    }
    public class Point2D : Vector<double>
    {
        public double X { get => this[0]; private set => this[0] = value; }
        public double Y { get => this[1]; private set => this[1] = value; }

        public Point2D(double x, double y) : base(new[]{x,y})
        {
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