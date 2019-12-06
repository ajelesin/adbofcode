namespace Day06
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        static readonly Action<string> print = Console.Write;
        static readonly Func<string> read = Console.ReadLine;

        static void Main(string[] args)
        {
            F2();
        }

        static void F1()
        {
            var data = File.ReadAllLines("..\\..\\..\\input.txt");

            var map = new Dictionary<string, List<string>>();
            var orbits = new Dictionary<string, int>();

            foreach (var line in data)
            {
                var tokens = line.Split(')');
                if (map.ContainsKey(tokens[0]))
                {
                    map[tokens[0]].Add(tokens[1]);
                }
                else
                {
                    map.Add(tokens[0], new List<string> { tokens[1] });
                }
            }

            foreach (var v in map.Keys)
            {
                if (orbits.ContainsKey(v)) continue;

                var count = Count(map, v);
                orbits.Add(v, count);
            }

            foreach (var v in map.Keys)
            {
                print($"{v} -> [{string.Join(',', map[v])}] -> {orbits[v]}\n");
            }

            print($"Total: {orbits.Values.Sum()}");
        }

        static int Count(Dictionary<string, List<string>> map, string v)
        {
            var cnt = 0;
            foreach (var v1 in map[v])
            {
                cnt += Count(map, v1);
                cnt += 1;
            }

            return cnt;
        }

        static void F2()
        {
            var data = File.ReadAllLines("..\\..\\..\\input.txt");

            var map = new Dictionary<string, List<string>>();
            
            var visited = new HashSet<string>();
            var q = new Queue<string>();
            var parrents = new Dictionary<string, string>();

            foreach (var line in data)
            {
                var tokens = line.Split(')');
                Add(map, tokens[0], tokens[1]);
                Add(map, tokens[1], tokens[0]);
            }

            q.Enqueue("YOU");
            visited.Add("YOU");

            while (q.Count != 0)
            {
                var currentVertex = q.Dequeue();
                foreach (var v in map[currentVertex])
                {
                    if (!visited.Contains(v))
                    {
                        q.Enqueue(v);
                        visited.Add(v);
                        parrents.Add(v, currentVertex);
                    }
                }
            }

            var v1 = "SAN";
            var pathLength = 0;
            while(v1 != "YOU")
            {
                pathLength += 1;
                print($"{pathLength}: {v1} <- {parrents[v1]}\n");
                v1 = parrents[v1];
            }

            print($"{pathLength}");
        }

        static void Add(Dictionary<string, List<string>> map, string v1, string v2)
        {
            if (map.ContainsKey(v1))
            {
                map[v1].Add(v2);
            }
            else
            {
                map.Add(v1, new List<string> { v2 });
            }
        }
    }
}
