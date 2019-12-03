namespace Day02
{
    using System;
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
            var origOpcodes = File.ReadAllLines("..\\..\\..\\input.txt")[0]
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            var opcodes = new int[origOpcodes.Length];

            for (var noun = 0; noun <= 99; noun += 1)
            {
                for (var verb = 0; verb <= 99; verb += 1)
                {
                    Array.Copy(origOpcodes, 0, opcodes, 0, origOpcodes.Length);

                    opcodes[1] = noun;
                    opcodes[2] = verb;


                    for (var i = 0; i < opcodes.Length; i += 4)
                    {
                        if (opcodes[i] == 1)
                        {
                            opcodes[opcodes[i + 3]] = opcodes[opcodes[i + 1]] + opcodes[opcodes[i + 2]];
                        }
                        else if (opcodes[i] == 2)
                        {
                            opcodes[opcodes[i + 3]] = opcodes[opcodes[i + 1]] * opcodes[opcodes[i + 2]];
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (opcodes[0] == 19690720)
                    {
                        Console.WriteLine($"{noun}; {verb}; {100 * noun + verb}");
                    }
                }
            }
        }
    }
}
