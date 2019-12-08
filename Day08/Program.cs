namespace Day08
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
            var digits = File.ReadAllLines("..\\..\\..\\input.txt")[0]
                .Select(o => o - 48)
                .ToArray();

            int wide = 25;
            int tall = 6;
            int step = wide * tall;
            int steps = digits.Length / step;

            var layers = new List<int[]>();

            for (var k = 0; k < steps; k += 1)
            {
                var cp = k * step;
                layers.Add(new int[10]);

                for (var j = 0; j < tall; j += 1)
                {
                    for (var i = 0; i < wide; i += 1)
                    {
                        var ci = cp + (j * wide) + i;
                        var color = digits[ci];
                        layers[k][color] += 1;
                    }
                }
            }

            int layIndex = 0;
            int curMin = layers[layIndex][0];
            for (var i = 0; i < layers.Count; i += 1)
            {
                if (layers[i][0] < curMin)
                {
                    layIndex = i;
                    curMin = layers[i][0];
                }
            }

            Console.WriteLine(layers[layIndex][1] * layers[layIndex][2]);
        }

        static void F2()
        {
            var digits = File.ReadAllLines("..\\..\\..\\input.txt")[0]
                .Select(o => o - 48)
                .ToArray();

            int wide = 25;
            int tall = 6;
            int step = wide * tall;
            int steps = digits.Length / step;

            var layers = new List<int[]>();

            for (var k = 0; k < steps; k += 1)
            {
                var cp = k * step;
                layers.Add(digits.Skip(cp).Take(step).ToArray());
            }

            int[] total = new int[step];

            for (var i = 0; i < step; i += 1)
            {
                for (var j = 0;j < layers.Count; j += 1)
                {
                    if (layers[j][i] != 2)
                    {
                        total[i] = layers[j][i];
                        break;
                    }
                }
            }

            for (var i = 0; i < step; i += 1)
            {

                if (total[i] == 1)
                    Console.Write('*');
                else
                    Console.Write(' ');
                if ((i+1) % wide == 0)
                {
                    Console.WriteLine();
                }
            }

        }
    }
}
