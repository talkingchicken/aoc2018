using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	class TreeNode
	{
		public List<TreeNode> childNodes {get; set;}
		public List<int> metadata {get; set;}

		public TreeNode()
		{
			childNodes = new List<TreeNode>();
			metadata = new List<int>();
		}

		public int Total()
		{
			return metadata.Sum() + childNodes.Select(x => x.Total()).Sum();
		}

		public int Value()
		{
			if (childNodes.Count == 0)
			{
				return metadata.Sum();
			}
			else
			{
				int total = 0;
				foreach (int index in metadata)
				{
					if ((index - 1) >= 0 && (index - 1) < childNodes.Count)
					{
						total += childNodes[index - 1].Value();
					}
				}
				return total;
			}
		}
	}
	class DayEight
	{
		public static void PartOne()
		{
			string line;
			StreamReader file = new StreamReader("input/Day8Input.txt");
			
			while ((line = file.ReadLine()) != null)
			{
				string[] nodes = line.Split(" ");

				int index = 0;
				
				TreeNode root = ParseTreeNode(nodes, ref index);
				
				Console.WriteLine("Total is {0}", root.Total());
			}
			
			file.Close();

		}

		static TreeNode ParseTreeNode(string[] nodes, ref int index)
		{
			TreeNode root = new TreeNode();

			int childNodeCount = Convert.ToInt32(nodes[index++]);
			int metaDataCount = Convert.ToInt32(nodes[index++]);

			for (int i = 0; i < childNodeCount; i++)
			{
				root.childNodes.Add(ParseTreeNode(nodes, ref index));
			}

			for (int i = 0; i < metaDataCount; i++)
			{
				root.metadata.Add(Convert.ToInt32(nodes[index++]));
			}

			return root;
		}

		public static void PartTwo()
		{
			string line;
			StreamReader file = new StreamReader("input/Day8Input.txt");
			
			while ((line = file.ReadLine()) != null)
			{
				string[] nodes = line.Split(" ");

				int index = 0;
				
				TreeNode root = ParseTreeNode(nodes, ref index);
				
				Console.WriteLine("Total is {0}", root.Value());
			}
			
			file.Close();
		}
	}
}