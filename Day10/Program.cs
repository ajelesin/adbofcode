namespace Day10
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            F2();
        }

        static void F1()
        {
            var points = new List<(int, int)>();
            var lines = File.ReadAllLines("..\\..\\..\\input.txt");

            for (var y = 0; y < lines.Length; y += 1)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x += 1)
                {
                    if (line[x] == '#')
                    {
                        points.Add((x, y));
                    }
                }
            }

            var d = new Dictionary<(int, int), HashSet<(int, int)>>();

            foreach (var p1 in points)
            {
                d.Add(p1, new HashSet<(int, int)>());

                foreach ( var p2 in points)
                {
                    if (p1 == p2) continue;

                    var dx = p1.Item1 - p2.Item1;
                    var dy = p1.Item2 - p2.Item2;

                    var gcd = Gcd(Math.Abs(dx), Math.Abs(dy));
                    var dp = (dx / gcd, dy / gcd);

                    d[p1].Add(dp);

                    //Console.WriteLine($"{p1} -> {p2} : {dp}");
                }
            }

            var max = int.MinValue;
            (int, int) p = (0, 0);
            foreach (var k in d.Keys)
            {
                if (d[k].Count > max)
                {
                    max = d[k].Count;
                    p = k;
                }

                //Console.WriteLine($"{k}: {string.Join(',', d[k])}");
            }

            Console.WriteLine(p);
            Console.WriteLine(max);

        }

        static int Gcd(int a, int b)
        {
            if (b == 0) return a;
            else return Gcd(b, a % b);
        }

        static double Angle(int x, int y)
        {
            return Math.Round((Math.Atan2(x, y) + 2 * Math.PI) % (2 * Math.PI), 10);
        }

        static void F2()
        {
            var points = new List<(int, int)>();
            var lines = File.ReadAllLines("..\\..\\..\\input.txt");

            for (var y = 0; y < lines.Length; y += 1)
            {
                var line = lines[y];
                for (var x = 0; x < line.Length; x += 1)
                {
                    if (line[x] == '#')
                    {
                        points.Add((x, y));
                    }
                }
            }

            var origPoint = (28, 29);
            var d = new Dictionary<double, List<(int x, int y, int d)>>();

            foreach (var p in points)
            {
                var a = Compare(origPoint, p);
                if (!d.ContainsKey(a))
                {
                    d.Add(a, new List<(int x, int y, int d)>());
                }

                d[a].Add((p.Item1, p.Item2, D(origPoint, p)));
            }

            var newd = new Dictionary<double, List<(int x, int y, int d)>>();
            foreach (var k in d.Keys)
            {
                newd.Add(k,  d[k].OrderBy(o => o.d).ToList());
            }

            (int x, int y, int d) el = (0, 0, 0);
            int i = 1;
            while (i < 200)
            {
                foreach (var k in newd.Keys.OrderBy(o => o))
                {
                    
                    el = newd[k][0];
                    newd[k].RemoveAt(0);
                    i += 1;
                    if (i == 200) break;
                }
            }

            Console.WriteLine($"{el}, {el.x * 100 + el.y}");

        }

        static int D((int, int) origPnt, (int, int) cmpPnt)
        {
            return (cmpPnt.Item1 - origPnt.Item1) * (cmpPnt.Item1 - origPnt.Item1) +
                (origPnt.Item2 - cmpPnt.Item2) * (origPnt.Item2 - cmpPnt.Item2);
        }

        static double Compare((int, int) origPoint, (int, int) comparedPoint)
        {
            var p = (comparedPoint.Item1 - origPoint.Item1, origPoint.Item2 - comparedPoint.Item2);

            return Angle(p.Item1, p.Item2);
        }
    }
}
