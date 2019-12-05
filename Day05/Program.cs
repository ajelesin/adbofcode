namespace Day05
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            F1();
        }

        static void F1()
        {
            var opcodes = File.ReadAllLines("..\\..\\..\\input.txt")[0]
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            var input = new Queue<int>();
            var output = new Queue<int>();

            input.Enqueue(5);

            var op = 0;
            var modes = new[] { 0, 0, 0 };

            while (op < opcodes.Length)
            {
                var opcode = opcodes[op] % 100;
                var joinedModes = opcodes[op] / 100;

                if (opcode == 99)
                {
                    break;
                }

                var i = 0;
                modes[0] = modes[1] = modes[2] = 0;
                while (joinedModes > 0)
                {
                    modes[i++] = joinedModes % 10;
                    joinedModes /= 10;
                }

                var i1 = modes[0] == 0 ? opcodes[op + 1] : op + 1;
                var i2 = modes[1] == 0 ? opcodes[op + 2] : op + 2;
                var i3 = modes[2] == 0 ? opcodes[op + 3] : op + 3;

                if (opcode == 1)
                {
                    opcodes[i3] = opcodes[i1] + opcodes[i2];
                    op += 4;
                }
                else if (opcode == 2)
                {
                    opcodes[i3] = opcodes[i1] * opcodes[i2];
                    op += 4;
                }
                else if (opcode == 3)
                {
                    opcodes[i1] = input.Dequeue();
                    op += 2;
                }
                else if (opcode == 4)
                {
                    output.Enqueue(opcodes[i1]);
                    op += 2;
                }
                else if (opcode == 5)
                {
                    op = opcodes[i1] != 0 ? opcodes[i2] : op + 3;
                }
                else if (opcode == 6)
                {
                    op = opcodes[i1] == 0 ? opcodes[i2] : op + 3;
                }
                else if (opcode == 7)
                {
                    opcodes[i3] = opcodes[i1] < opcodes[i2] ? 1 : 0;
                    op += 4;
                }
                else if (opcode == 8)
                {
                    opcodes[i3] = opcodes[i1] == opcodes[i2] ? 1 : 0;
                    op += 4;
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine(string.Join(",", output));

        }
    }
}
