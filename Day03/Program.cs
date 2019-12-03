namespace Day03
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            while (true)
            {
                var line = Console.ReadLine();
                var points = line.Split(' ').Select(int.Parse).ToArray();
                var inter = p.Intersect(points[0], points[1], points[2], points[3], points[4], points[5], points[6], points[7]);

                Console.WriteLine(inter);
            }       
        }

        struct Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X, Y;
        }

        void F1()
        {
            var lines = File.ReadAllLines("..\\..\\..\\input.txt");

            var startPoint = new Point(0, 0);

            var wire1 = new List<Point> { startPoint };
            var wire2 = new List<Point> { startPoint };

            var intersections = new List<Point>();

            ParseLine(lines[0], wire1);
            ParseLine(lines[1], wire2);

            IntersectWire(wire1, wire2, intersections);


        }

        void ParseLine(string line, IList<Point> wire)
        {
            foreach (var step in line.Split(','))
            {
                var direction = step[0];
                var stepLength = Convert.ToInt32(step[1]);

                var prevPoint = wire[wire.Count - 1];

                if (direction == 'U')
                {
                    IncWire(wire, 0, 1, stepLength);
                }
                else if (direction == 'D')
                {
                    IncWire(wire, 0, -1, stepLength);
                }
                else if (direction == 'L')
                {
                    IncWire(wire, -1, 0, stepLength);
                }
                else
                {
                    IncWire(wire, 1, 0, stepLength);
                }
            }
        }

        void IncWire(IList<Point> wire, int dx, int dy, int length)
        {
            while (length --> 0)
            {
                var lastPoint = wire[wire.Count - 1];
                var nextPoint = new Point(lastPoint.X + dx, lastPoint.Y + dy);
                wire.Add(nextPoint);
            }
        }

        void IntersectWire(IList<Point> wire1, IList<Point> wire2, IList<Point> intersections)
        {
            for (var i = 0; i < wire1.Count; i += 1)
            {
                for (var j = 0; j < wire2.Count; j += 1)
                {
                    if (EqPoint(wire1[i], wire2[j]))
                    {
                        intersections.Add(wire1[i]);
                    }
                }
            }
        }

        bool EqPoint(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        int Dist(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}
