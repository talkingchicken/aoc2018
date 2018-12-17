using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class DaySixteen
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

		static List<OpCode> CheckPossibleOpcodes(List<int> input, List<int> command, List<int> output)
		{
			List<OpCode> returnValue = new List<OpCode>();

			if (output[command[3]] == input[command[1]] + input[command[2]])
			{
				returnValue.Add(OpCode.Addr);
			}

			if (output[command[3]] == input[command[1]] + command[2])
			{
				returnValue.Add(OpCode.Addi);
			}

			if (output[command[3]] == input[command[1]] * input[command[2]])
			{
				returnValue.Add(OpCode.Mulr);
			}

			if (output[command[3]] == input[command[1]] * command[2])
			{
				returnValue.Add(OpCode.Muli);
			}

			if (output[command[3]] == (input[command[1]] & input[command[2]]))
			{
				returnValue.Add(OpCode.Banr);
			}

			if (output[command[3]] == (input[command[1]] & command[2]))
			{
				returnValue.Add(OpCode.Bani);
			}

			if (output[command[3]] == (input[command[1]] | input[command[2]]))
			{
				returnValue.Add(OpCode.Borr);
			}

			if (output[command[3]] == (input[command[1]] | command[2]))
			{
				returnValue.Add(OpCode.Bori);
			}

			if (output[command[3]] == input[command[1]])
			{
				returnValue.Add(OpCode.Setr);
			}

			if (output[command[3]] == command[1])
			{
				returnValue.Add(OpCode.Seti);
			}

			if ((output[command[3]] == 1 && input[command[1]] > input[command[2]]) || (output[command[3]] == 0 && input[command[1]] <= input[command[2]]))
			{
				returnValue.Add(OpCode.Gtrr);
			}

			if ((output[command[3]] == 1 && command[1] > input[command[2]]) || (output[command[3]] == 0 && command[1] <= input[command[2]]))
			{
				returnValue.Add(OpCode.Gtir);
			}

			if ((output[command[3]] == 1 && input[command[1]] > command[2]) || (output[command[3]] == 0 && input[command[1]] <= command[2]))
			{
				returnValue.Add(OpCode.Gtri);
			}

			if ((output[command[3]] == 1 && input[command[1]] == input[command[2]]) || (output[command[3]] == 0 && input[command[1]] != input[command[2]]))
			{
				returnValue.Add(OpCode.Eqrr);
			}

			if ((output[command[3]] == 1 && command[1] == input[command[2]]) || (output[command[3]] == 0 && command[1] != input[command[2]]))
			{
				returnValue.Add(OpCode.Eqir);
			}

			if ((output[command[3]] == 1 && input[command[1]] == command[2]) || (output[command[3]] == 0 && input[command[1]] != command[2]))
			{
				returnValue.Add(OpCode.Eqri);
			}

			return returnValue;
		}

		public static void PartOne()
		{
			string line;
			StreamReader file = new StreamReader("input/Day16Input.txt");
			
			int multiOpcodes = 0;

			while ((line = file.ReadLine()) != null)
			{
				if(line.StartsWith("B"))
				{
					string inputString = line.Substring(9, 10);
					List<string> inputStringList = new List<string>(inputString.Split(", "));
					List<int> input = new List<int>(inputStringList.Select(x => Convert.ToInt32(x)));

					line = file.ReadLine();
					List<string> commandString = new List<string>(line.Split(" "));
					List<int> command = new List<int>(commandString.Select(x => Convert.ToInt32(x)));

					line = file.ReadLine();
					string outputString = line.Substring(9, 10);
					List<string> outputStringList = new List<string>(outputString.Split(", "));
					List<int> output = new List<int>(outputStringList.Select(x => Convert.ToInt32(x)));

					if (CheckPossibleOpcodes(input, command, output).Count >= 3)
					{
						multiOpcodes++;
					}
				}
			}
			
			file.Close();
			
			Console.WriteLine("{0} commands can be three or more opcodes", multiOpcodes);
		}

		public static void PartTwo()
		{
			string line;
			StreamReader file = new StreamReader("input/Day16Input.txt");

			List<List<OpCode>> possibleCommands = new List<List<OpCode>>(new List<OpCode>[16]);

			while ((line = file.ReadLine()) != null)
			{
				if(line.StartsWith("B"))
				{
					string inputString = line.Substring(9, 10);
					List<string> inputStringList = new List<string>(inputString.Split(", "));
					List<int> input = new List<int>(inputStringList.Select(x => Convert.ToInt32(x)));

					line = file.ReadLine();
					List<string> commandString = new List<string>(line.Split(" "));
					List<int> command = new List<int>(commandString.Select(x => Convert.ToInt32(x)));

					line = file.ReadLine();
					string outputString = line.Substring(9, 10);
					List<string> outputStringList = new List<string>(outputString.Split(", "));
					List<int> output = new List<int>(outputStringList.Select(x => Convert.ToInt32(x)));

					List<OpCode> opcodes =  CheckPossibleOpcodes(input, command, output);

					if (possibleCommands[command[0]] != null)
					{
						possibleCommands[command[0]] = new List<OpCode>(possibleCommands[command[0]].Intersect(opcodes));
					}
					else
					{
						possibleCommands[command[0]] = opcodes;
					}
				}
			}
			
			file.Close();

			List<int> registers = new List<int>(new int[4]);

			List<string> commands = Utils.GetLinesFromFile("input/Day16Input2.txt");

			foreach (string commandString in commands)
			{
				List<string> commandStringSplit = new List<String>(commandString.Split(" "));

				List<int> command = new List<int>(commandStringSplit.Select(x => Convert.ToInt32(x)));

				EvaluateCommand(registers, command, possibleCommands);
			}

			Console.WriteLine("Register 0 contains {0}", registers[0]);
		}

		static void EvaluateCommand(List<int> registers, List<int> command, List<List<OpCode>> opCodeMap)
		{
			List<OpCode> opcodes = new List<OpCode>{
				OpCode.Gtir,
				OpCode.Mulr,
				OpCode.Seti,
				OpCode.Gtrr,
				OpCode.Bori,
				OpCode.Borr,
				OpCode.Banr,
				OpCode.Eqri,
				OpCode.Bani,
				OpCode.Addr,
				OpCode.Addi,
				OpCode.Eqrr,
				OpCode.Gtri,
				OpCode.Eqir,
				OpCode.Setr,
				OpCode.Muli };

			switch (opcodes[command[0]])
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
	}
}