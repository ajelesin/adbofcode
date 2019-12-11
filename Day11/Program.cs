using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            F1();
        }

        static void F1()
        {
            long i = 0;
            long[] opcodes = new long[100_000];

            foreach (long opcode in File.ReadAllLines("..\\..\\..\\input.txt")[0]
                .Split(',')
                .Select(long.Parse))
            {
                opcodes[i++] = opcode;
            }

            var input = new Queue<long>();
            var output = new Queue<long>();

            var colored = new Dictionary<(long, long), long>();
            (long, long) currentPoint = (0, 0);
            (int, int) currentVector = (0, 1);

            input.Enqueue(1);
            colored.Add(currentPoint, 1);
            
            long opPointer = 0, relativeBase = 0;
            do
            {
                var res = IntCode(opcodes, opPointer, relativeBase, input, output);
                opPointer = res.opcodePointer;
                relativeBase = res.relativeBase;

                var color = output.Dequeue();
                var direction = output.Dequeue();

                colored[currentPoint] = color;

                var d = direction == 0 ? 1 : -1;
                currentVector = (-currentVector.Item2 * d, currentVector.Item1 * d);

                currentPoint.Item1 += currentVector.Item1;
                currentPoint.Item2 += currentVector.Item2;

                if (!colored.ContainsKey(currentPoint))
                {
                    colored.Add(currentPoint, 0);
                    input.Enqueue(0);
                }
                else
                {
                    input.Enqueue(colored[currentPoint]);
                }
            }
            while (opPointer >= 0);

            //Console.WriteLine(colored.Count);

            for (var y = colored.Keys.Max(o => o.Item2); y >= colored.Keys.Min(o => o.Item2); y -= 1)
            {
                for (var x = colored.Keys.Min(o => o.Item1); x < colored.Keys.Max(o => o.Item1); x += 1)
                {
                    if (colored.ContainsKey((x, y)) && colored[(x, y)] == 1)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }

        static (long opcodePointer, long relativeBase) FakeCode(long[] opcodes, long opcodePointer, long relativeBase, Queue<long> input, Queue<long> output)
        {
            if (opcodePointer == 0)
            {
                input.Dequeue();
                output.Enqueue(1);
                output.Enqueue(0);
                return (1, 0);
            }

            if (opcodePointer == 1)
            {
                input.Dequeue();
                output.Enqueue(0);
                output.Enqueue(0);
                return (2, 0);
            }

            if (opcodePointer == 2)
            {
                input.Dequeue();
                output.Enqueue(1);
                output.Enqueue(0);
                return (3, 0);
            }

            if (opcodePointer == 3)
            {
                input.Dequeue();
                output.Enqueue(1);
                output.Enqueue(0);
                return (4, 0);
            }

            if (opcodePointer == 4)
            {
                input.Dequeue();
                output.Enqueue(0);
                output.Enqueue(1);
                return (5, 0);
            }

            if (opcodePointer == 5)
            {
                input.Dequeue();
                output.Enqueue(1);
                output.Enqueue(0);
                return (6, 0);
            }

            if (opcodePointer == 6)
            {
                input.Dequeue();
                output.Enqueue(1);
                output.Enqueue(0);
                return (-1, 0);
            }

            return (-1, 0);
        }

        static (long opcodePointer, long relativeBase) IntCode(long[] opcodes, long opcodePointer, long relativeBase, Queue<long> input, Queue<long> output)
        {
            long op = opcodePointer;
            var modes = new long[] { 0, 0, 0 };

            while (op < opcodes.Length)
            {
                long opcode = opcodes[op] % 100;
                long joinedModes = opcodes[op] / 100;

                if (opcode == 99)
                {
                    return (-1, relativeBase);
                }

                var i = 0;
                modes[0] = modes[1] = modes[2] = 0;
                while (joinedModes > 0)
                {
                    modes[i++] = joinedModes % 10;
                    joinedModes /= 10;
                }


                if (opcode == 1)
                {
                    // 0 position mode
                    // 1 immediate mode
                    // 2 relative mode
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : modes[0] == 1 ? op + 1 : relativeBase + opcodes[op + 1];
                    var i2 = modes[1] == 0 ? opcodes[op + 2] : modes[1] == 1 ? op + 2 : relativeBase + opcodes[op + 2];
                    var i3 = modes[2] == 0 ? opcodes[op + 3] : modes[2] == 1 ? op + 3 : relativeBase + opcodes[op + 3];

                    opcodes[i3] = opcodes[i1] + opcodes[i2];
                    op += 4;
                }
                else if (opcode == 2)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : modes[0] == 1 ? op + 1 : relativeBase + opcodes[op + 1];
                    var i2 = modes[1] == 0 ? opcodes[op + 2] : modes[1] == 1 ? op + 2 : relativeBase + opcodes[op + 2];
                    var i3 = modes[2] == 0 ? opcodes[op + 3] : modes[2] == 1 ? op + 3 : relativeBase + opcodes[op + 3];

                    opcodes[i3] = opcodes[i1] * opcodes[i2];
                    op += 4;
                }
                else if (opcode == 3)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : modes[0] == 1 ? op + 1 : relativeBase + opcodes[op + 1];

                    if (input.Count == 0)
                    {
                        return (op, relativeBase);
                    }

                    opcodes[i1] = input.Dequeue();
                    op += 2;
                }
                else if (opcode == 4)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : modes[0] == 1 ? op + 1 : relativeBase + opcodes[op + 1];

                    output.Enqueue(opcodes[i1]);
                    op += 2;
                }
                else if (opcode == 5)
                {
                    long i1 = modes[0] == 0 ? opcodes[op + 1] : modes[0] == 1 ? op + 1 : relativeBase + opcodes[op + 1];
                    long i2 = modes[1] == 0 ? opcodes[op + 2] : modes[1] == 1 ? op + 2 : relativeBase + opcodes[op + 2];

                    op = opcodes[i1] != 0 ? opcodes[i2] : op + 3;
                }
                else if (opcode == 6)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : modes[0] == 1 ? op + 1 : relativeBase + opcodes[op + 1];
                    var i2 = modes[1] == 0 ? opcodes[op + 2] : modes[1] == 1 ? op + 2 : relativeBase + opcodes[op + 2];

                    op = opcodes[i1] == 0 ? opcodes[i2] : op + 3;
                }
                else if (opcode == 7)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : modes[0] == 1 ? op + 1 : relativeBase + opcodes[op + 1];
                    var i2 = modes[1] == 0 ? opcodes[op + 2] : modes[1] == 1 ? op + 2 : relativeBase + opcodes[op + 2];
                    var i3 = modes[2] == 0 ? opcodes[op + 3] : modes[2] == 1 ? op + 3 : relativeBase + opcodes[op + 3];

                    opcodes[i3] = opcodes[i1] < opcodes[i2] ? 1 : 0;
                    op += 4;
                }
                else if (opcode == 8)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : modes[0] == 1 ? op + 1 : relativeBase + opcodes[op + 1];
                    var i2 = modes[1] == 0 ? opcodes[op + 2] : modes[1] == 1 ? op + 2 : relativeBase + opcodes[op + 2];
                    var i3 = modes[2] == 0 ? opcodes[op + 3] : modes[2] == 1 ? op + 3 : relativeBase + opcodes[op + 3];

                    opcodes[i3] = opcodes[i1] == opcodes[i2] ? 1 : 0;
                    op += 4;
                }
                else if (opcode == 9)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : modes[0] == 1 ? op + 1 : relativeBase + opcodes[op + 1];

                    relativeBase += opcodes[i1];
                    op += 2;
                }
                else
                {
                    throw new Exception("wrong opcode");
                }
            }

            return (-1, relativeBase);
        }
    }
}
