namespace Day04
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            int total = 0;
            var r = Console.ReadLine().Split('-').Select(int.Parse).ToArray();

            for (var p = r[0]; p <= r[1]; p += 1)
            {
                var pass = p.ToString();
                if (!Check2(pass)) continue;
                if (!CheckInc(pass)) continue;

                total += 1;
            }

            Console.WriteLine(total);
        }

        static bool Check2(string pass)
        {
            var d = new Dictionary<string, int>();
            
            for (var i = 0; i < pass.Length - 1; i += 1)
            {
                if (pass[i] != pass[i + 1]) continue;
                var k = pass.Substring(i, 2);
                if (!d.ContainsKey(k)) d.Add(k, 1);
                else d[k] += 1;
            }

            foreach (var k in d.Keys)
            {
                if (d[k] == 1)
                    return true;
            }

            return false;
        }

        static bool CheckInc(string pass)
        {
            for (var i = 1; i < pass.Length; i += 1)
            {
                if (pass[i - 1] > pass[i])
                    return false;
            }

            return true;
        }
    }
}
