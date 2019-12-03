namespace Day01
{
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            F2();
        }

        public static void F2()
        {
            var total = 0;
            var lines = File.ReadAllLines("..\\..\\..\\input.txt");


            foreach (var line in lines)
            {
                var m = int.Parse(line);
                var f = FuelSum(m);
                total += f;
            }

            Console.WriteLine(total);
        }

        public static int FuelSum(int moduleMass)
        {
            var total = 0;
            var v = moduleMass;

            while ((v = v / 3 - 2) > 0)
            {
                total += v;
            }

            return total;
        }

        public static void F1()
        {
            var total = 0;
            var lines = File.ReadAllLines("..\\..\\..\\input.txt");


            foreach (var line in lines)
            {
                var m = int.Parse(line);
                var f = m / 3 - 2;
                total += f;
            }

            Console.WriteLine(total);
        }
    }
}
