using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    class DayTwentyOne
    {
        enum OpCode {
			Addr,
			Addi,
			Mulr,
			Muli,
			Banr,
			Bani,
			Borr,
			Bori,
			Setr,
			Seti,
			Gtir,
			Gtri,
			Gtrr,
			Eqir,
			Eqri,
			Eqrr
		}

        static void EvaluateCommand(List<int> registers, List<int> command)
		{
			switch ((OpCode)command[0])
			{
				case OpCode.Addr:
					registers[command[3]] = registers[command[1]] + registers[command[2]];
					break;

				case OpCode.Addi:
					registers[command[3]] = registers[command[1]] + command[2];
					break;

				case OpCode.Mulr:
					registers[command[3]] = registers[command[1]] * registers[command[2]];
					break;

				case OpCode.Muli:
					registers[command[3]] = registers[command[1]] * command[2];
					break;

				case OpCode.Banr:
					registers[command[3]] = registers[command[1]] & registers[command[2]];
					break;

				case OpCode.Bani:
					registers[command[3]] = registers[command[1]] & command[2];
					break;

				case OpCode.Borr:
					registers[command[3]] = registers[command[1]] | registers[command[2]];
					break;

				case OpCode.Bori:
					registers[command[3]] = registers[command[1]] | command[2];
					break;

				case OpCode.Setr:
					registers[command[3]] = registers[command[1]];
					break;

				case OpCode.Seti:
					registers[command[3]] = command[1];
					break;

				case OpCode.Gtrr:
					registers[command[3]] = (registers[command[1]] > registers[command[2]]) ? 1 : 0;
					break;

				case OpCode.Gtir:
					registers[command[3]] = (command[1] > registers[command[2]]) ? 1 : 0;
					break;

				case OpCode.Gtri:
					registers[command[3]] = (registers[command[1]] > command[2]) ? 1 : 0;
					break;

				case OpCode.Eqrr:
					registers[command[3]] = (registers[command[1]] == registers[command[2]]) ? 1 : 0;
					break;

				case OpCode.Eqir:
					registers[command[3]] = (command[1] == registers[command[2]]) ? 1 : 0;
					break;

				case OpCode.Eqri:
					registers[command[3]] = (registers[command[1]] == command[2]) ? 1 : 0;
					break;
			}
		}
        public static void PartOne()
        {
            List<string> lines = Utils.GetLinesFromFile("input/Day21Input.txt");

            string firstLine = lines[0];

            int instructionRegister = Convert.ToInt32(firstLine.Substring(4));
            
            List<List<int>> program = new List<List<int>>(lines.Skip(1).Select(x => 
            {
                string[] splitLine = x.Split(" ");
                OpCode opCode = StringToOpcode(splitLine[0]);
                List<int> command = new List<int>{(int)opCode, Convert.ToInt32(splitLine[1]), Convert.ToInt32(splitLine[2]), Convert.ToInt32(splitLine[3])};

                return command;
            }));

            List<int> registers = new List<int>(new int[6]);

            while(true)
            {
                if (registers[1] == 28)
                    break;

                EvaluateCommand(registers, program[registers[instructionRegister]]);
                registers[instructionRegister]++;
            }

            Console.WriteLine(registers[5]);
        }

        private static OpCode StringToOpcode(string input)
        {
            switch(input)
            {
                case "addr":
                    return OpCode.Addr;
                case "addi":
                    return OpCode.Addi;
                case "mulr":
                    return OpCode.Mulr;
                case "muli":
                    return OpCode.Muli;
                case "banr":
                    return OpCode.Banr;
                case "bani":
                    return OpCode.Bani;
                case "borr":
                    return OpCode.Banr;
                case "bori":
                    return OpCode.Bori;
                case "setr":
                    return OpCode.Setr;
                case "seti":
                    return OpCode.Seti;
                case "gtir":
                    return OpCode.Gtir;
                case "gtrr":
                    return OpCode.Gtrr;
                case "gtri":
                    return OpCode.Gtri;
                case "eqir":
                    return OpCode.Eqir;
                case "eqri":
                    return OpCode.Eqri;
                case "eqrr":
                    return OpCode.Eqrr;
                default:
                    throw new Exception();
            }
        }
        public static void PartTwo()
        {
            List<string> lines = Utils.GetLinesFromFile("input/Day21Input.txt");

            string firstLine = lines[0];

            int instructionRegister = Convert.ToInt32(firstLine.Substring(4));
            
            List<List<int>> program = new List<List<int>>(lines.Skip(1).Select(x => 
            {
                string[] splitLine = x.Split(" ");
                OpCode opCode = StringToOpcode(splitLine[0]);
                List<int> command = new List<int>{(int)opCode, Convert.ToInt32(splitLine[1]), Convert.ToInt32(splitLine[2]), Convert.ToInt32(splitLine[3])};

                return command;
            }));

            List<int> registers = new List<int>(new int[6]);

            HashSet<int> values = new HashSet<int>();
            int previousValue = 0;
            int iterations = 0;
            while(true)
            {
                if (registers[1] == 28)
                {
                    if (values.Contains(registers[5]))
                    {
                        Console.WriteLine(previousValue);
                        return;
                    }
                    else
                    {
                        if (iterations++ % 100 == 0)
                            Console.WriteLine("{0}: {1}", iterations, registers[5]);

                        values.Add(registers[5]);
                        previousValue = registers[5];
                    }
                }

                EvaluateCommand(registers, program[registers[instructionRegister]]);
                registers[instructionRegister]++;
            }
        }
    }
}