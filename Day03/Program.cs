namespace Day03
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Point = System.ValueTuple<int, int>;

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("..\\..\\..\\input.txt");

            var wire1 = new List<Point> { };
            var wire2 = new List<Point> { };

            ParseLine(lines[0], wire1);
            ParseLine(lines[1], wire2);

            var intersections = wire1.Intersect(wire2);

            var dist = intersections.Min(a => Math.Abs(a.Item1) + Math.Abs(a.Item2));
            Console.WriteLine(dist);
        }

        static void ParseLine(string line, IList<Point> wire)
        {
            var pnt = new Point(0, 0);

            var dx = new Dictionary<char, int> { { 'U', 0 }, { 'D', 0 }, { 'L', -1 }, { 'R', 1 } };
            var dy = new Dictionary<char, int> { { 'U', 1 }, { 'D', -1 }, { 'L', 0 }, { 'R', 0 } };
            
            foreach (var step in line.Split(','))
            {
                var direction = step[0];
                var stepLength = int.Parse(step.Substring(1));

                while (stepLength-- > 0)
                {
                    pnt.Item1 += dx[direction];
                    pnt.Item2 += dy[direction];

                    wire.Add(pnt);
                }
            }
        }
    }
}
