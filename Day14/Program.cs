using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day14
{
    class Program
    {
        static (string Elem, int Quan) Elemnent(string e, int q) => (e, q);

        static void Main(string[] args)
        {
            F1();
        }

        static void F1()
        {
            var lines = File.ReadAllLines("..\\..\\..\\input.txt");

            var reactions = new Dictionary<string, (int, List<(string, int)>)>();
            var needed = new Dictionary<string, int>();

            Read(lines, reactions);

            long totalOre = 1_000_000_000_000;

            needed.Add("FUEL", 1);
            Processing(needed, reactions);

            Console.WriteLine($"{needed["ORE"]:n0}");


            var stocks = new Dictionary<string, long> { { "ORE", totalOre } };

            Func<string, long, bool> tryMake = null;
            tryMake = (target, amount) =>
            {
                var reaction = reactions[target];
                var runs = (long)Math.Ceiling(amount / (double)reaction.Item1);
                if (reaction.Item2.Any(input => GetStock(input.Item1, stocks) < runs * input.Item2 && input.Item1 == "ORE"))
                {
                    return false;
                }

                var stockBackup = stocks.ToDictionary(a => a.Key, a => a.Value);

                while (reaction.Item2.Any(input => GetStock(input.Item1, stocks) < runs * input.Item2))
                {
                    var mustMake = reaction.Item2.First(input => GetStock(input.Item1, stocks) < runs * input.Item2);
                    var need = runs * mustMake.Item2 - GetStock(mustMake.Item1, stocks);
                    if (!tryMake(mustMake.Item1, need))
                    {
                        stocks = stockBackup;
                        return false;
                    }
                }

                foreach (var input in reaction.Item2)
                {
                    AddStock(input.Item1, -runs * input.Item2, stocks);
                }

                AddStock(target, runs * reaction.Item1, stocks);

                return true;
            };

            var mf = 1_000_000;
            while (mf > 0)
            {
                while (tryMake("FUEL", mf)) { }
                mf /= 10;
            }

            Console.WriteLine($"{stocks["FUEL"]:n0}");
        }

        static void Processing(Dictionary<string, int> needed, Dictionary<string, (int, List<(string, int)>)> chemicals)
        {
            while (needed.Any(kvp => kvp.Key != "ORE" && kvp.Value > 0))
            {
                var curr = needed.First(kvp => kvp.Key != "ORE" && kvp.Value > 0);

                var reaction = chemicals[curr.Key];
                needed[curr.Key] -= reaction.Item1;

                foreach (var input in reaction.Item2)
                {
                    if (needed.ContainsKey(input.Item1))
                    {
                        needed[input.Item1] += input.Item2;
                    }
                    else
                    {
                        needed.Add(input.Item1, input.Item2);
                    }
                }
            }
        }

        static void Read(string[] lines, Dictionary<string, (int, List<(string, int)>)> chemicals)
        {
            foreach (var line in lines)
            {
                var t = line.Split(" => ");
                var t1 = t[1].Split(' ');

                var els = new List<(string, int)>();

                chemicals.Add(t1[1], (int.Parse(t1[0]), els));

                foreach (var el in t[0].Split(", "))
                {
                    var el1 = el.Split(' ');
                    els.Add((el1[1], int.Parse(el1[0])));
                }
            }
        }

        static long GetStock(string che, Dictionary<string, long> stocks)
        {
            return stocks.ContainsKey(che) ? stocks[che] : 0;
        }

        static void AddStock(string che, long am, Dictionary<string, long> stocks)
        {
            if (stocks.ContainsKey(che)) stocks[che] += am;
            else stocks.Add(che, am);
        }
    }
}
