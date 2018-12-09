using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day08
{
    class Program
    {
        static List<int> info = new List<int>();

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")[0];

            //Console.WriteLine(input);

            info = input.Split(' ').Select(i => int.Parse(i)).ToList();
            var root = ProcessNodes();


            Console.WriteLine($"Part 1: {SumAllMetadata(root)}");
            // 42146 (yay!)

            Console.WriteLine($"Part 2: {GetNodeValue(root)}");
            // 26753 (yay!)
        }

        private static int GetNodeValue(Node root)
        {
            int nodeValue = 0;

            if (root.Children.Count == 0)
            {
                return root.Metadata.Sum();
            }

            foreach (var metadata in root.Metadata)
            {
                if (metadata == 0 || metadata > root.Children.Count) continue;
                nodeValue += GetNodeValue(root.Children[metadata - 1]);
            }

            return nodeValue;
        }

        private static int SumAllMetadata(Node root)
        {
            int sumOfMetadata = 0;

            foreach (var child in root.Children)
            {
                sumOfMetadata += SumAllMetadata(child);
            }

            foreach (var metadata in root.Metadata)
            {
                sumOfMetadata += metadata;
            }

            return sumOfMetadata;
        }

        private static Node ProcessNodes()
        {
            var childCount = info[0];
            var metadataCount = info[1];
            info.RemoveRange(0, 2);

            var node = new Node();

            for (int i = 0; i < childCount; i++)
            {
                node.Children.Add(ProcessNodes());
            }

            for (int i = 0; i < metadataCount; i++)
            {
                node.Metadata.Add(info[i]);
            }
            info.RemoveRange(0, metadataCount);

            return node;
        }

    }

    public class Node
    {
        public Node()
        {
            Children = new List<Node>();
            Metadata = new List<int>();
        }

        public List<Node> Children { get; set; }
        public List<int> Metadata { get; set; }
    }
}
