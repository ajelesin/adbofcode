using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
{
    class Program
    {
        struct Point
        {
            public long X;
            public long Y;
            public Point(long x, long y)
            {
                X = x;
                Y = y;
            }

            public override string ToString() => $"({X}; {Y})";
        }

        static void Main(string[] args)
        {
            var opcodes = File.ReadAllLines("..\\..\\..\\input.txt")[0]
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            var input = new Queue<long>();
            var output = new Queue<long>();

            var start = new Point(0, 0);
            var finish = new Point(0, 0);
            var finishNotFound = true;

            var visited = new Dictionary<Point, long>();
            var parrent = new Dictionary<Point, Point>();

            var s = new Queue<(Point, long[] opcode)>();

            s.Enqueue((start, opcodes.Select(o => o).ToArray()));
            visited.Add(start, 1);

            while (s.Count > 0)
            {
                var (currPosition, opcode) = s.Dequeue();

                var movements = new Dictionary<int, Point>
                {
                    { 1, new Point(currPosition.X, currPosition.Y + 1) },
                    { 2, new Point(currPosition.X, currPosition.Y - 1) },
                    { 3, new Point(currPosition.X - 1, currPosition.Y) },
                    { 4, new Point(currPosition.X + 1, currPosition.Y) }
                };

                foreach (var direction in movements.Keys)
                {

                    if (!visited.ContainsKey(movements[direction]))
                    {

                        input.Enqueue(direction);
                        var newOpcode = opcode.Select(o => o).ToArray();
                        
                        IntCode(newOpcode, 0, 0, input, output);

                        var type = output.Dequeue();
                        parrent[movements[direction]] = currPosition;
                        visited.Add(movements[direction], type);

                        if (type == 1)
                        {
                            s.Enqueue((movements[direction], newOpcode));
                        }

                        if (type == 2 && finishNotFound)
                        {
                            finish = movements[direction];
                            finishNotFound = false;
                        }
                    }
                }
            }

            var finish1 = finish;
            int commands = 0;
            while (parrent.ContainsKey(finish))
            {
                finish = parrent[finish];
                commands += 1;
            }

            Console.WriteLine("the fewest number of movement commands is " + commands);
            Console.ReadLine();

            var spreaded = new Dictionary<Point, int>();
            var spreadStart = finish1;
            var currMinutes = 0;

            var q1 = new Queue<Point>();

            q1.Enqueue(spreadStart);
            spreaded.Add(spreadStart, currMinutes);

            while (q1.Count > 0)
            {
                var currPoint = q1.Dequeue();
                currMinutes = spreaded[currPoint] + 1;


                var nextPoints = new [] {
                    new Point(currPoint.X, currPoint.Y + 1),
                    new Point(currPoint.X, currPoint.Y - 1),
                    new Point(currPoint.X - 1, currPoint.Y),
                    new Point(currPoint.X + 1, currPoint.Y)
                };

                foreach (var nextPoint in nextPoints)
                {
                    if (visited.ContainsKey(nextPoint) && visited[nextPoint] != 0)
                    {
                        if (!spreaded.ContainsKey(nextPoint))
                        {
                            spreaded.Add(nextPoint, currMinutes);
                            q1.Enqueue(nextPoint);
                        }
                    }
                }
            }

            Print(visited, spreaded);

            Console.WriteLine("it will take " + (currMinutes - 1) + " minutes");
            Console.ReadLine();
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



        static void Print(Dictionary<Point, long> d, Dictionary<Point, int> s)
        {
            for (var y = d.Keys.Max(o => o.Y); y >= d.Keys.Min(o => o.Y); y -= 1)
            {
                for (var x = d.Keys.Min(o => o.X); x <= d.Keys.Max(o => o.X); x += 1)
                {
                    if (d.ContainsKey(new Point(x, y)))
                    {
                        var v = d[new Point(x, y)];
                        
                        if (v == 0)
                        {
                            Console.Write("#\t");
                        }
                        else
                        {
                            Console.Write(s[new Point(x, y)] + "\t");
                        }
                    }
                    else
                    {
                        Console.Write("?\t");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
