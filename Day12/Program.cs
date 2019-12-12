using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    class Program
    {
        static (int X, int Y, int Z) Point(int x, int y, int z) => (x, y, z);
        static ((int X, int Y, int Z) Position, (int X, int Y, int Z) Velocity) Moon((int x, int y, int z) p, (int x, int y, int z) v) => (p, v);
        static void Print<T>(IEnumerable<T> enumerable) => Console.Write(string.Join(',', enumerable));

        static void Main(string[] args)
        {
            F1();
        }

        static void F1()
        {
            var orig = File.ReadAllLines("..\\..\\..\\input.txt")
                .Select(o => o.Trim('>'))
                .Select(o => o.Split(','))
                .Select(o => new[] { o[0].Split('=')[1], o[1].Split('=')[1], o[2].Split('=')[1] })
                .Select(o => new[] { new[] { int.Parse(o[0]), int.Parse(o[1]), int.Parse(o[2]) }, new[] { 0, 0, 0 } })
                .ToList();

            // position, velocity

            var times = new long[3];
            
            for (var t = 0; t < 3; t += 1)
            {
                long time = 0;
                var visited = new HashSet<(int,int,int,int,int,int,int,int)>();

                var moons = orig
                    .Select(o => new int[2][] { new int[3] { o[0][0], o[0][1], o[0][2] }, new int[3] { o[1][0], o[1][1], o[1][2] } })
                    .ToList();

                while (true)
                {
                    for (var i = 0; i < moons.Count; i += 1)
                    {
                        for (var j = i + 1; j < moons.Count; j += 1)
                        {
                            var m1 = moons[i];
                            var m2 = moons[j];

                            for (var coord = 0; coord < 3; coord += 1)
                            {

                                if (m1[0][coord] > m2[0][coord])
                                {
                                    m1[1][coord] -= 1;
                                    m2[1][coord] += 1;
                                }
                                else if (m1[0][coord] < m2[0][coord])
                                {
                                    m1[1][coord] += 1;
                                    m2[1][coord] -= 1;
                                }
                            }
                        }
                    }

                    for (var i = 0; i < moons.Count; i += 1)
                    {
                        var m = moons[i];
                        for (var coord = 0; coord < 3; coord += 1)
                        {
                            m[0][coord] += m[1][coord];
                        }
                    }



                    var s = (
                        moons[0][0][t], moons[1][0][t], moons[2][0][t], moons[3][0][t],
                        moons[0][1][t], moons[1][1][t], moons[2][1][t], moons[3][1][t]
                        );

                    if (visited.Contains(s))
                    {
                        times[t] = time;
                        break;
                    }
                    else
                    {
                        visited.Add(s);
                    }
                    time += 1;
                }
            }

            //var P = moons.Select(o => o[0]).Select(o => o.Sum(o => Math.Abs(o))).ToArray();
            //var K = moons.Select(o => o[1]).Select(o => o.Sum(o => Math.Abs(o))).ToArray();
            //var E = Enumerable.Zip(P, K, (p, k) => p * k).Sum();

            Console.WriteLine(lcm(lcm(times[0], times[1]), times[2]));
        }



        static long Gcd(long a, long b)
        {
            if (b == 0) return a;
            else return Gcd(b, a % b);
        }

        static long lcm(long a, long b)
        {
            return Math.Abs(a * b) / Gcd(a, b);
        }
    }
}
