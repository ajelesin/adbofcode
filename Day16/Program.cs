﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            var signal = File.ReadAllLines("..\\..\\..\\input.txt")[0]
                .Select(o => o - 48)
                .ToList();

            var signal2 = Enumerable.Range(0, 10_000).SelectMany(o => signal).ToList();
            var offest2 = Convert.ToInt32(string.Join("", signal2.GetRange(0, 7)));

            var basePattern = new[] { 0, 1, 0, -1 };
            int offset = 0;

            var result = Enumerable.Range(0, signal.Count).ToList(); ;

            var patterns = new List<List<int>>();

            for (var i = 0; i < signal.Count; i += 1)
            {
                var pattern = new List<int>();
                var repeated = i + 1;

                foreach (var p in basePattern)
                {
                    for (var r = 0; r < repeated; r += 1)
                    {
                        pattern.Add(p);
                    }
                }

                pattern.Add(pattern[0]);
                pattern.RemoveAt(0);

                var j = 0;
                if (pattern.Count < signal.Count)
                {
                    while (pattern.Count < signal.Count)
                    {
                        pattern.Add(pattern[j]);
                        j += 1;
                    }
                }
                else
                {
                    pattern = pattern.GetRange(0, signal.Count);
                }

                patterns.Add(pattern);
            }

            for (var phase = 0; phase < 100; phase += 1)
            {
                for (var index = 0; index < signal.Count; index += 1)
                {
                    var newVal = Enumerable.Zip(signal, patterns[index], (a, b) => a * b).Sum();

                    newVal = Math.Abs(newVal) % 10;
                    result[index] = newVal;
                }

                var temp = signal;
                signal = result;
                result = temp;
            }

            Console.WriteLine($"the signal: " + string.Join("", signal.GetRange(offset, 8)));
            Console.ReadLine();

            signal2 = signal2.GetRange(offest2, signal2.Count - offest2);
            for (var phase = 0; phase < 100; phase += 1)
            {
                for (var i = signal2.Count - 2; i >= 0; i -= 1)
                {
                    signal2[i] = Math.Abs(signal2[i + 1] + signal2[i]) % 10;
                }
            }

            Console.WriteLine($"this signal is " + string.Join("", signal2.GetRange(0, 8)));
            Console.ReadLine();
        }
    }
}
