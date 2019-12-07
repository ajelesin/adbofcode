namespace Day07
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

            int maxSignal = int.MinValue;
            int[] seq = new int[5];
            int minPhase = 5, maxPhase = 9;


            for (seq[0] = minPhase; seq[0] <= maxPhase; seq[0] += 1)
                for (seq[1] = minPhase; seq[1] <= maxPhase; seq[1] += 1)
                    for (seq[2] = minPhase; seq[2] <= maxPhase; seq[2] += 1)
                        for (seq[3] = minPhase; seq[3] <= maxPhase; seq[3] += 1)
                            for (seq[4] = minPhase; seq[4] <= maxPhase; seq[4] += 1)
                            {
                                bool unique = true;
                                for (var i = 0; i < 5; i += 1)
                                    for (var j = 0; j < 5; j += 1)
                                    {
                                        if (i == j) continue;
                                        if (seq[i] == seq[j])
                                            unique = false;
                                    }

                                if (!unique) continue;

                                var sig = Proc(seq, opcodes);
                                if (sig > maxSignal) maxSignal = sig;
                            }

            Console.WriteLine(maxSignal);
        }

        static int Proc(int[] seq, int[] opcodes)
        {
            var inputs = new List<Queue<int>>();

            var memories = new List<int[]>();
            for (var k = 0; k < seq.Length; k += 1)
            {
                memories.Add(opcodes.Select(o => o).ToArray());
            }

            var opcodePointers = new List<int>();
            for (var k = 0; k < seq.Length; k += 1)
            {
                opcodePointers.Add(0);
            }

            for (var k = 0; k < seq.Length; k += 1)
            {
                inputs.Add(new Queue<int>());
                inputs[k].Enqueue(seq[k]);
            }

            inputs[0].Enqueue(0);

            int newPointer = 0;
            do
            {
                for (var i = 0; i < seq.Length; i += 1)
                {
                    var memory = memories[i];
                    var pointer = opcodePointers[i];

                    var input = inputs[i];
                    var output = inputs[(i + 1) % seq.Length];

                    newPointer = IntCode(memory, pointer, input, output);
                    opcodePointers[i] = newPointer;
                }
            }
            while (newPointer >= 0);

            var val = inputs[0].Dequeue();
            return val;
        }

        static int IntCode(int[] opcodes, int opcodePointer, Queue<int> input, Queue<int> output)
        {

            var op = opcodePointer;
            var modes = new[] { 0, 0, 0 };

            while (op < opcodes.Length)
            {
                var opcode = opcodes[op] % 100;
                var joinedModes = opcodes[op] / 100;

                if (opcode == 99)
                {
                    return -1;
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
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : op + 1;
                    var i2 = modes[1] == 0 ? opcodes[op + 2] : op + 2;
                    var i3 = modes[2] == 0 ? opcodes[op + 3] : op + 3;

                    opcodes[i3] = opcodes[i1] + opcodes[i2];
                    op += 4;
                }
                else if (opcode == 2)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : op + 1;
                    var i2 = modes[1] == 0 ? opcodes[op + 2] : op + 2;
                    var i3 = modes[2] == 0 ? opcodes[op + 3] : op + 3;

                    opcodes[i3] = opcodes[i1] * opcodes[i2];
                    op += 4;
                }
                else if (opcode == 3)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : op + 1;

                    if (input.Count == 0)
                    {
                        return op;
                    }

                    opcodes[i1] = input.Dequeue();
                    op += 2;
                }
                else if (opcode == 4)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : op + 1;

                    output.Enqueue(opcodes[i1]);
                    op += 2;
                }
                else if (opcode == 5)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : op + 1;
                    var i2 = modes[1] == 0 ? opcodes[op + 2] : op + 2;

                    op = opcodes[i1] != 0 ? opcodes[i2] : op + 3;
                }
                else if (opcode == 6)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : op + 1;
                    var i2 = modes[1] == 0 ? opcodes[op + 2] : op + 2;

                    op = opcodes[i1] == 0 ? opcodes[i2] : op + 3;
                }
                else if (opcode == 7)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : op + 1;
                    var i2 = modes[1] == 0 ? opcodes[op + 2] : op + 2;
                    var i3 = modes[2] == 0 ? opcodes[op + 3] : op + 3;

                    opcodes[i3] = opcodes[i1] < opcodes[i2] ? 1 : 0;
                    op += 4;
                }
                else if (opcode == 8)
                {
                    var i1 = modes[0] == 0 ? opcodes[op + 1] : op + 1;
                    var i2 = modes[1] == 0 ? opcodes[op + 2] : op + 2;
                    var i3 = modes[2] == 0 ? opcodes[op + 3] : op + 3;

                    opcodes[i3] = opcodes[i1] == opcodes[i2] ? 1 : 0;
                    op += 4;
                }
                else
                {
                    throw new Exception("wrong opcode");
                }
            }

            return -1;
        }
    }
}
