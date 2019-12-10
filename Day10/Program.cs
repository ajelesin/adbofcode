namespace Day10
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            F1();
        }

        static void F1()
        {
            var points = new List<(int, int)>();

            var lines = File.ReadAllLines("..\\..\\..\\input.txt");
            var segments = new Dictionary<(int, int), HashSet<(double, double, double)>>();

            for (var i = 0; i < lines.Length; i += 1)
            {
                var line = lines[i];
                for (var j = 0; j < line.Length; j += 1)
                {
                    if (line[j] == '#')
                    {
                        points.Add((i, j));
                    }
                }
            }

            foreach (var point1 in points)
            {
                foreach (var point2 in points)
                {
                    if (point1 == point2) continue;

                    double A = point1.Item2 - point2.Item2;
                    double B = point2.Item1 - point1.Item1;
                    double C = point1.Item1 * point2.Item2 - point2.Item1 * point1.Item2;

                    var D = 1.0 / (Math.Sqrt(A * A + B * B));
                    if (C < 0) D = Math.Abs(D);
                    if (C > 0) D = 0.0 - Math.Abs(D);

                    A = Math.Round(A / D, 1);
                    B = Math.Round(B / D, 1);
                    C = Math.Round(C / D, 1);
                    
                    if (!segments.ContainsKey(point1))
                    {
                        segments.Add(point1, new HashSet<(double, double, double)>());
                    }

                    segments[point1].Add((A, B, C));

                }
            }

            foreach (var c in segments)
            {
                Console.WriteLine($"{c.Key} : [{c.Value.Count}]");
                //Console.WriteLine($"{c.Key} : [{string.Join(',', c.Value)}]");
            }
        }
    }
}
