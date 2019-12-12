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
        static void Print<T>(IEnumerable<T> enumerable) => Console.WriteLine(string.Join(',', enumerable));

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

            int time = 0;
            while (true)
            {
                for (var i = 0;i < moons.Count; i+= 1)
                {
                    for (var j = 0; j < moons.Count; j += 1)
                    {
                        if (i == j) continue;

                        var m1 = moons[i];
                        var m2 = moons[j];

                        for (var coord = 0; coord < 3; coord += 1)
                        {
                            if (m1[0][coord] > m2[0][coord])
                            {
                                m1[1][coord] -= 1;
                                m2[1][coord] += 1;
                            }
                        }

                    }
                }
            }


        }
    }
}
