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
            var moons = File.ReadAllLines("..\\..\\..\\input.txt")
                .Select(o => o.Trim('>'))
                .Select(o => o.Split(','))
                .Select(o => new[] { o[0].Split('=')[1], o[1].Split('=')[1], o[2].Split('=')[1] })
                .Select(o => new[] { new[] { int.Parse(o[0]), int.Parse(o[1]), int.Parse(o[2]) }, new[] { 0, 0, 0 } })
                .ToList();

            // position, velocity
            var orig = File.ReadAllLines("..\\..\\..\\input.txt")
                .Select(o => o.Trim('>'))
                .Select(o => o.Split(','))
                .Select(o => new[] { o[0].Split('=')[1], o[1].Split('=')[1], o[2].Split('=')[1] })
                .Select(o => new[] { new[] { int.Parse(o[0]), int.Parse(o[1]), int.Parse(o[2]) }, new[] { 0, 0, 0 } })
                .ToList();

            long time = 0;
            while (true)
            {
                for (var i = 0; i < moons.Count; i+= 1)
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

                time += 1;

                if (Compare(orig, moons))
                {
                    Console.Write($"\rFinished at step: {time:#,##0}");
                    Console.ReadLine();
                }

                if (time % 1_000_000 == 0)
                {
                    Console.Write($"\rStep: {time:#,##0}                  ");
                }
            }

            var P = moons.Select(o => o[0]).Select(o => o.Sum(o => Math.Abs(o))).ToArray();
            var K = moons.Select(o => o[1]).Select(o => o.Sum(o => Math.Abs(o))).ToArray();
            var E = Enumerable.Zip(P, K, (p, k) => p * k).Sum();

            Console.WriteLine(E);
        }

        static bool Compare(List<int[][]> orig, List<int[][]> compared)
        {
            for (var i = 0; i < orig.Count; i += 1)
            {
                for (var j = 0; j < 2; j += 1)
                {
                    for (var k = 0; k < 3; k += 1)
                    {
                        if (orig[i][j][k] != compared[i][j][k])
                            return false;
                    }
                }
            }

            return true;
        }
    }
}
