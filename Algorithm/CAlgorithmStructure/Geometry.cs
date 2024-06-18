using Microsoft.SqlServer.Server;

namespace Algorithm.CAlgorithmStructure
{
    public class Geometry
    {



        
       
    }


    
    //哈尔变换
    public class Circle
    {
        public int R;
        public Point Center;

        public Circle(int r, Point center)
        {
            R = r;
            Center = center;
        }
    }
    public class Segment
    {
        public Point P1;
        public Point P2;
        public Segment(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }

    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point((int x, int y) position)
        {
            var (x, y) = position;
            X = x;
            Y = y;
        }

        public double Norm()
        {
            return X * X + Y * Y;
        }
        
        
        
        
        public static implicit operator Point((int, int) pos)
        {
            return new Point(pos);
        }


        public static Point operator +(Point p1, Point p2)
        {
            return (p1.X + p2.X, p1.Y + p2.Y);
        }
        public static Point operator -(Point p1, Point p2)
        {
            return (p1.X - p2.X, p1.Y - p2.Y);
        }
        public static Point operator *(Point p1, int c)
        {
            return (p1.X * c, p1.Y * c);
        }
        public static Point operator /(Point p1, int c)
        {
            return (p1.X / c, p1.Y / c);
        }
        
    }
}